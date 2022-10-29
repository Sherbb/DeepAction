using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.VFX;

namespace DeepAction
{
    [RequireComponent(typeof(DeepMovementBody)), RequireComponent(typeof(Rigidbody2D))]
    public class DeepEntity : MonoBehaviour, IHit
    {
        // * Resources
        [Title("Resources", "", TitleAlignments.Centered), PropertySpace, HideInEditorMode, ShowInInspector]
        public Dictionary<D_Resource, DeepResource> resources { get; private set; }
        // * Attributes
        [Title("Attributes", "", TitleAlignments.Centered), PropertySpace, HideInEditorMode, ShowInInspector]
        public Dictionary<D_Attribute, DeepAttribute> attributes { get; private set; }
        // * Flags
        [Title("States", "", TitleAlignments.Centered), PropertySpace, HideInEditorMode, ShowInInspector]
        public Dictionary<D_Flag, DeepFlag> flags { get; private set; }
        // * Behaviors
        [Title("Behaviors", "", TitleAlignments.Centered), PropertySpace, HideInEditorMode, ShowInInspector]
        public List<DeepBehavior> behaviors { get; private set; }
        // * Events
        public DeepEntityEvents events;
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
        public List<DeepBehavior> castableBehaviors { get; private set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////

        public Rigidbody2D rb { get; private set; }
        public DeepMovementBody mb { get; private set; }
        public Vector2 aimDirection { get; set; }

        private EntityTemplate template;

        private Dictionary<Collider2D, DeepEntity> activeCollisions = new Dictionary<Collider2D, DeepEntity>();

        public DeepEntity Initialize(EntityTemplate t)
        {
            template = t;

            events = new DeepEntityEvents();
            attributes = new Dictionary<D_Attribute, DeepAttribute>();
            resources = new Dictionary<D_Resource, DeepResource>();
            flags = new Dictionary<D_Flag, DeepFlag>();
            behaviors = new List<DeepBehavior>();
            castableBehaviors = new List<DeepBehavior>();
            rb = gameObject.GetComponent<Rigidbody2D>();

            if (rb == null)
            {
                Debug.LogError("DeepEntity does not have a rigidbody: " + this.gameObject.name, this.gameObject);
            }
            //If you arent using movementBody you probably don't need this.
            if (rb.bodyType != RigidbodyType2D.Kinematic)
            {
                Debug.LogError("DeepEntity has non-kinematic rigidbody: " + this.gameObject.name, this.gameObject);
            }

            mb = gameObject.GetComponent<DeepMovementBody>();
            mb.entity = this;

            team = t.team;
            type = t.type;

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

            // * Kill entity when health runs out
            resources[D_Resource.Health].onDeplete += Die;

            initialized = true;
            //OnEnable gets called before this, so we need to initialize here when entities are created.
            App.state.game.RegisterEntity(this);
            events.OnEntityEnable?.Invoke();
            return this;
        }

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

        private void OnDisable()
        {
            dying = false;
            events.OnEntityDisable?.Invoke();
            App.state.game.DeregisterEntity(this);
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
                    return;
                }
                resources[d.target].Consume(d.damage);
            }
        }

        private void DamageNumbers(int num)
        {
            Debug.LogError("damage: " + num);
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
            if (dying) return;

            events.OnEntityDie?.Invoke();
            dying = true;
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
    }
}
