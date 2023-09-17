using System;
using System.Collections.Generic;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine.VFX;
using DeepAction.Views;

namespace DeepAction
{
    //TODO: -------------------------------------------------------------------

    // 1. need to define collision type somewhere inside deepEntity (currently everything is set to bounce)
    // 2. Movement body reads from attributes.movementRadius which needs to be removed.

    //TODO: -------------------------------------------------------------------

    [RequireComponent(typeof(DeepMovementBody)), RequireComponent(typeof(Rigidbody2D))]
    public class DeepEntity : MonoBehaviour, IHit
    {
        // * Views
#if ODIN_INSPECTOR
        [Title("Views", "", TitleAlignments.Centered), PropertySpace, HideInEditorMode, ShowInInspector]
#endif
        public List<DeepViewLink> views = new List<DeepViewLink>();
        // * Resources
#if ODIN_INSPECTOR
        [Title("Resources", "", TitleAlignments.Centered), PropertySpace, HideInEditorMode, ShowInInspector]
#endif
        public Dictionary<D_Resource, DeepResource> resources { get; private set; } = new Dictionary<D_Resource, DeepResource>();
        // * Attributes
#if ODIN_INSPECTOR
        [Title("Attributes", "", TitleAlignments.Centered), PropertySpace, HideInEditorMode, ShowInInspector]
#endif
        public Dictionary<D_Attribute, DeepAttribute> attributes { get; private set; } = new Dictionary<D_Attribute, DeepAttribute>();
        // * Flags
#if ODIN_INSPECTOR
        [Title("States", "", TitleAlignments.Centered), PropertySpace, HideInEditorMode, ShowInInspector]
#endif
        public Dictionary<D_Flag, DeepFlag> flags { get; private set; } = new Dictionary<D_Flag, DeepFlag>();
        // * Behaviors
#if ODIN_INSPECTOR
        [Title("Behaviors", "", TitleAlignments.Centered), PropertySpace, HideInEditorMode, ShowInInspector]
#endif
        public List<DeepBehavior> behaviors { get; private set; } = new List<DeepBehavior>();
        // * Events
        public DeepEntityEvents events = new DeepEntityEvents();
        // * Team
#if ODIN_INSPECTOR
        [HideInEditorMode, ShowInInspector]
#endif
        public D_Team team { get; private set; }
        // * Type
#if ODIN_INSPECTOR
        [HideInEditorMode, ShowInInspector]
#endif
        public D_EntityType type { get; private set; }

        // * Status
#if ODIN_INSPECTOR
        [HideInEditorMode, ShowInInspector]
#endif
        public bool dying { get; private set; }//entity will be killed(disabled) next LateUpdate()
#if ODIN_INSPECTOR
        [HideInEditorMode]
#endif
        public bool initialized { get; private set; }

        // * Ownership
#if ODIN_INSPECTOR
        [HideInEditorMode, ShowInInspector]
#endif
        public DeepEntity creator;
#if ODIN_INSPECTOR
        [HideInEditorMode, ShowInInspector]
#endif
        public DeepEntity owner;
        public bool hasOwner => owner != null;

        // * Lookups
#if ODIN_INSPECTOR
        [HideInEditorMode, ShowInInspector]
#endif
        //abilities are a subset of behaviors
        public List<DeepAbility> abilities { get; private set; } = new List<DeepAbility>();

        ////////////////////////////////////////////////////////////////////////////////////////////////

        public Rigidbody2D rb { get; private set; }
        public CircleCollider2D col { get; private set; }
        public DeepMovementBody mb { get; private set; }
        public Vector2 aimDirection { get; set; }
        public Transform cachedTransform { get; private set; }//slightly faster than mono.transform

        public Dictionary<Collider2D, DeepEntity> activeCollisions { get; private set; } = new Dictionary<Collider2D, DeepEntity>();

        public DeepEntity Initialize(EntityTemplate template, DeepEntity creator = null)
        {
            //pointless GC! optimize this! Requires replacing addAtt with setAtt etc below.
            views.Clear();
            attributes.Clear();
            resources.Clear();
            flags.Clear();

            behaviors.Clear();
            abilities.Clear();

            if (rb == null)
            {
                rb = gameObject.GetComponent<Rigidbody2D>();
            }
            if (col == null)
            {
                col = gameObject.GetComponent<CircleCollider2D>();
            }
            if (mb == null)
            {
                mb = gameObject.GetComponent<DeepMovementBody>();
                mb.entity = this;
            }

            cachedTransform = transform;

            team = template.team;
            type = template.type;

            this.creator = creator;
            owner = creator;

            //get attributes from template
            foreach (KeyValuePair<D_Attribute, A> attPair in template.attributes)
            {
                this.AddAttribute(attPair.Key, attPair.Value);
            }
            //fill in missing attributes
            foreach (D_Attribute att in Enum.GetValues(typeof(D_Attribute)))
            {
                if (!attributes.ContainsKey(att))
                {
                    this.AddAttribute(att, new DeepAttribute(0f));
                }
            }
            //get resources from template
            foreach (KeyValuePair<D_Resource, R> res in template.resources)
            {
                this.AddResource(res.Key, res.Value);
            }
            //fill in missing resources
            foreach (D_Resource res in Enum.GetValues(typeof(D_Resource)))
            {
                if (!resources.ContainsKey(res))
                {
                    this.AddResource(res, new DeepResource(1, 0));
                }
            }
            //fill in flags (they are all false by default)
            foreach (D_Flag flag in Enum.GetValues(typeof(D_Flag)))
            {
                this.SetupFlag(flag);
            }

            rb.velocity = Vector2.zero;
            mb.SetVelocity(Vector2.zero);

            // Kill entity when health runs out
            resources[D_Resource.Health].onDeplete += Die;

            initialized = true;
            //OnEnable gets called before this, so we need to initialize here when entities are created.
            App.state.game.RegisterEntity(this);
            RefreshColliderSize();

            //* ACTIVE
            gameObject.SetActive(true);

            //add behaviors from template. We do this later so that coroutines will work.
            foreach (DeepBehavior b in template.behaviors)
            {
                this.AddBehavior(b, owner != null ? owner : this);
            }

            //Important we do this after adding behaviors ^ 
            events.OnEntityEnable?.Invoke();

            foreach (string v in template.views)
            {
                this.AddView(v);
            }
            return this;
        }

        private void OnDisable()
        {
            dying = false;
            events.OnEntityDisable?.Invoke();
        }

        /// <summary>
        /// Apply damage to an entity. Damage can be applied to ANY resource, but note that HEALTH directly affects
        /// the life of an entity, and SHIELD will be consumed instead of HEALTH by default if possible. 
        /// </summary>
        public void Hit(params Damage[] hits)
        {
            Hit(null, hits);
        }
        public void Hit(DeepEntity damageDealer, params Damage[] hits)
        {
            foreach (Damage d in hits)
            {
                if (d.target == D_Resource.Health)
                {
                    //Shield absorption. Game dependant
                    int sr = resources[D_Resource.Shield].Consume(d.damage);
                    int hr = resources[D_Resource.Health].Consume(sr);
                    int damageToHP = d.damage - hr;
                    DamageNumbers(damageToHP, d.color);
                    //todo use the right damage
                    events.OnTakeDamage?.Invoke(damageToHP);
                    damageDealer?.events.OnDealDamage?.Invoke(damageToHP);
                    return;
                }
                resources[d.target].Consume(d.damage);
            }
        }

        private void DamageNumbers(int num, Color color)
        {
            if (DeepVFX.Pull("damageNumbers", out VisualEffect vfx, out VFXEventAttribute att))
            {
                att.SetInt("num", num - 1);//custom att specific to damageNumbers.vfx
                att.SetVector3("position", transform.position);
                att.SetVector3("color", new Vector3(color.r, color.g, color.b));
                att.SetFloat("spawnCount", 1);
                vfx.SendEvent("OnPlay", att);
            }
        }

        //todo consider adding a source entity to this.
        public void Die()
        {
            if (dying)
            {
                return;
            }

            events.OnEntityDie?.Invoke();
            dying = true;
        }

        public void RefreshColliderSize()
        {
            float r = 0f;
            foreach (DeepViewLink view in views)
            {
                if (!view.viewAffectsHitbox)
                {
                    continue;
                }
                r = Mathf.Max(r, view.viewRadius);
            }
            col.radius = r;
        }

        //////////////////////////////////////////////////////////////

        private void OnTriggerEnter2D(Collider2D col)
        {
            events.OnTriggerEnter2D?.Invoke(col);
            if (col.gameObject.TryGetComponent(out DeepEntity e))
            {
                activeCollisions[col] = e;
                events.OnEntityCollisionEnter?.Invoke(e);
            }
        }

        private void OnTriggerExit2D(Collider2D col)
        {
            events.OnTriggerExit2D?.Invoke(col);
            activeCollisions.Remove(col);
            if (col.gameObject.TryGetComponent(out DeepEntity e))
            {
                events.OnEntityCollisionExit?.Invoke(e);
            }
        }

        public void SetAimDirection(Vector2 aimDirection)
        {
            this.aimDirection = aimDirection;
        }

        //-----------------------------------
        //            CREATE
        //-----------------------------------

        public static DeepEntity Create(EntityTemplate template, Vector3 position, Quaternion rotation, params string[] extraViews)
        {
            return Create(template, null, position, rotation, Vector3.one, extraViews);
        }
        public static DeepEntity Create(EntityTemplate template, DeepEntity creator, Vector3 position, Quaternion rotation, params string[] extraViews)
        {
            return Create(template, creator, position, rotation, Vector3.one, extraViews);
        }

        public static DeepEntity Create(EntityTemplate template, DeepEntity creator, Vector3 position, Quaternion rotation, Vector3 scale, params string[] extraViews)
        {
            DeepEntity e = DeepManager.instance.PullEntity();
            e.transform.position = position;
            e.transform.rotation = rotation;
            e.transform.localScale = scale;
            e.Initialize(template, creator);
            foreach (string view in extraViews)
            {
                e.AddView(view);
            }
            return e;
        }
    }
}
