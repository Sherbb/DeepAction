using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.VFX;
using DeepAction.Views;

namespace DeepAction
{
    //TODO: -------------------------------------------------------------------

    // 1. need to define collision type somewhere inside deepEntity

    //TODO: -------------------------------------------------------------------

    [RequireComponent(typeof(DeepMovementBody)), RequireComponent(typeof(Rigidbody2D))]
    public class DeepEntity : MonoBehaviour, IHit
    {
        // * Views
        [Title("Views", "", TitleAlignments.Centered), PropertySpace, HideInEditorMode, ShowInInspector]
        public List<DeepViewLink> views = new List<DeepViewLink>();
        // * Resources
        [Title("Resources", "", TitleAlignments.Centered), PropertySpace, HideInEditorMode, ShowInInspector]
        public Dictionary<D_Resource, DeepResource> resources { get; private set; } = new Dictionary<D_Resource, DeepResource>();
        // * Attributes
        [Title("Attributes", "", TitleAlignments.Centered), PropertySpace, HideInEditorMode, ShowInInspector]
        public Dictionary<D_Attribute, DeepAttribute> attributes { get; private set; } = new Dictionary<D_Attribute, DeepAttribute>();
        // * Flags
        [Title("States", "", TitleAlignments.Centered), PropertySpace, HideInEditorMode, ShowInInspector]
        public Dictionary<D_Flag, DeepFlag> flags { get; private set; } = new Dictionary<D_Flag, DeepFlag>();
        // * Behaviors
        [Title("Behaviors", "", TitleAlignments.Centered), PropertySpace, HideInEditorMode, ShowInInspector]
        public List<DeepBehavior> behaviors { get; private set; } = new List<DeepBehavior>();
        // * Events
        public DeepEntityEvents events = new DeepEntityEvents();
        // * Team
        [HideInEditorMode, ShowInInspector]
        public D_Team team { get; private set; }
        // * Type
        [HideInEditorMode, ShowInInspector]
        public D_EntityType type { get; private set; }

        // * Status
        [HideInEditorMode, ShowInInspector]
        public bool dying { get; private set; }//entity will be killed(disabled) next LateUpdate()
        [HideInEditorMode]
        public bool initialized { get; private set; }

        // * Lookups
        [HideInEditorMode, ShowInInspector]
        //abilities are a subset of behaviors
        public List<DeepAbility> abilities { get; private set; } = new List<DeepAbility>();

        ////////////////////////////////////////////////////////////////////////////////////////////////

        public Rigidbody2D rb { get; private set; }
        public CircleCollider2D col { get; private set; }
        public DeepMovementBody mb { get; private set; }
        public Vector2 aimDirection { get; set; }

        private Dictionary<Collider2D, DeepEntity> activeCollisions = new Dictionary<Collider2D, DeepEntity>();

        public DeepEntity Initialize(EntityTemplate template)
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

            team = template.team;
            type = template.type;

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
            //add behaviors from template
            foreach (DeepBehavior b in template.behaviors)
            {
                this.AddBehavior(b);
            }

            rb.velocity = Vector2.zero;
            mb.SetVelocity(Vector2.zero);

            // * Kill entity when health runs out
            //! remember to undo this when we re-use resources
            resources[D_Resource.Health].onDeplete += Die;

            initialized = true;
            //OnEnable gets called before this, so we need to initialize here when entities are created.
            App.state.game.RegisterEntity(this);
            events.OnEntityEnable?.Invoke();
            RefreshColliderSize();
            gameObject.SetActive(true);
            return this;
        }

        /*
        private void OnEnable()
        {
            dying = false;

            if (initialized)
            {
                foreach (KeyValuePair<D_Resource, R> r in template.resources)
                {
                    resources[r.Key].SetValue(r.Value.baseValue);
                }
                App.state.game.RegisterEntity(this);
                events?.OnEntityEnable?.Invoke();
            }
        }
        */

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
            foreach (Damage d in hits)
            {
                if (d.target == D_Resource.Health)
                {
                    //Game dependant, you might want a totally different damage calc.
                    int sr = resources[D_Resource.Shield].Consume(d.damage);
                    int hr = resources[D_Resource.Health].Consume(sr);
                    DamageNumbers(d.damage - hr);
                    //todo use the right damage
                    events.OnTakeDamage?.Invoke(d.damage - hr);
                    return;
                }
                resources[d.target].Consume(d.damage);
            }
        }

        private void DamageNumbers(int num)
        {
            if (DeepVFX.Pull("damageNumbers", out VisualEffect vfx, out VFXEventAttribute att))
            {
                att.SetInt("num", num - 1);
                att.SetVector3("position", transform.position);
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
            activeCollisions.Add(col, null);
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

        public void CheckCollisionStay()
        {
            foreach (KeyValuePair<Collider2D, DeepEntity> col in activeCollisions)
            {
                events.OnTriggerStay2D?.Invoke(col.Key);
                if (col.Value != null)
                {
                    events.OnEntityCollisionStay?.Invoke(col.Value);
                }
            }
        }

        public void SetAimDirection(Vector2 aimDirection)
        {
            this.aimDirection = aimDirection;
        }

        //-----------------------------------
        //            CREATE
        //-----------------------------------

        public static DeepEntity Create(EntityTemplate template, Vector3 position, Quaternion rotation, params string[] views)
        {
            DeepEntity e = DeepManager.instance.PullEntity();
            e.transform.position = position;
            e.transform.rotation = rotation;
            e.Initialize(template);
            foreach (string view in views)
            {
                e.AddView(view);
            }
            return e;
        }
    }
}
