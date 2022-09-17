using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using System.Linq;

namespace DeepAction
{
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

        ////////////////////////////////////////////////////////////////////////////////////////////////

        public static readonly D_Resource[] damageHeirarchy = { D_Resource.Health };//Damage is done from left to right       

        public Rigidbody2D rb { get; private set; }
        private EntityTemplate template;

        public DeepEntity Initialize(EntityTemplate t)
        {
            template = t;

            events = new DeepEntityEvents();
            attributes = new Dictionary<D_Attribute, DeepAttribute>();
            resources = new Dictionary<D_Resource, DeepResource>();
            flags = new Dictionary<D_Flag, DeepFlag>();
            behaviors = new List<DeepBehavior>();
            rb = gameObject.GetComponent<Rigidbody2D>();

            team = t.team;
            type = t.type;

            foreach (A att in template.attributes)
            {
                this.AddAttribute(att);
            }
            //fill in missing attributes
            foreach (D_Attribute a in Enum.GetValues(typeof(D_Attribute)))
            {
                if (!attributes.ContainsKey(a))
                {
                    this.AddAttribute(a, new DeepAttribute(0f));
                }
            }
            foreach (R res in template.resources)
            {
                this.AddResource(res);
            }
            //fill in missing resources
            foreach (D_Resource r in Enum.GetValues(typeof(D_Resource)))
            {
                if (!resources.ContainsKey(r))
                {
                    this.AddResource(r, new DeepResource(1, 0));
                }
            }
            foreach (D_Flag s in Enum.GetValues(typeof(D_Flag)))
            {
                this.AddFlag(s);
            }
            foreach (DeepBehavior b in template.behaviors)
            {
                this.AddBehavior(b);
            }

            // * Kill entity when health runs out
            resources[D_Resource.Health].onDeplete += Die;

            initialized = true;
            //OnEnable gets called before this, so we need to initialize here when entities are created.
            App.state.game.RegisterEntity(this);
            return this;
        }

        private void OnEnable()
        {
            dying = false;

            if (initialized)
            {
                foreach (R r in template.resources)
                {
                    resources[r.type].SetValue(r.baseValue);
                }
                App.state.game.RegisterEntity(this);
            }
        }

        void OnDisable()
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
                    int r = resources[D_Resource.Shield].Consume(d.damage);
                    resources[D_Resource.Health].Consume(r);
                    return;
                }
                resources[d.target].Consume(d.damage);
            }
        }

        //todo maybe private
        public void Die()
        {
            if (dying) return;

            events.OnEntityDie?.Invoke();
            dying = true;
        }
    }
}
