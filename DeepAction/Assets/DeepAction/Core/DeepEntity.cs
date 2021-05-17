using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
namespace DeepAction
{
    public class DeepEntity : SerializedMonoBehaviour, IHit
    {
        ///An action entity has:
        ///Resources
        ///Attributes
        ///Behaviors

        ///Behaviors can be:
        ///Abilities
        ///Status Effects
        ///Modifiers
        ///Whatever you can imagine

        ///An action entity is:
        ///
        ///the player
        ///ability projectiles
        ///enemies
        ///structures
        [OnInspectorGUI("InspectorValidate")]

        [Title("Deep Entity", "Deep Action by @AlanSherba", TitleAlignments.Centered)]

        public Dictionary<D_Resource, DeepResource> resources = new Dictionary<D_Resource, DeepResource>();

        [Title("Attributes", "", TitleAlignments.Centered)]

        public Dictionary<D_Attribute, DeepAttribute> attributes = new Dictionary<D_Attribute, DeepAttribute>();

        [Title("Behaviors", "", TitleAlignments.Centered)]

        [System.NonSerialized, OdinSerialize]
        public List<DeepBehavior> behaviors = new List<DeepBehavior>();

        [Title("Preset", "Determines where the entity will pull its start values from", TitleAlignments.Centered)]
        public enum EntityPresetType { UseInsepctor, PresetObject, None }
        [EnumToggleButtons, HideLabel]
        [InfoBox("$EnumExplanation", InfoMessageType.None), HideInPlayMode]
        public EntityPresetType presetType = EntityPresetType.UseInsepctor;

        [ShowIf("presetType", EntityPresetType.PresetObject),HideInPlayMode]
        public DeepEntityPreset preset;

        private Dictionary<D_Resource, DeepResource> inspectorResources = new Dictionary<D_Resource, DeepResource>();
        private Dictionary<D_Attribute, DeepAttribute> inspectorAttributes = new Dictionary<D_Attribute, DeepAttribute>();
        private List<DeepBehavior> inspectorBehaviors = new List<DeepBehavior>();

        private void Awake()
        {
            if (presetType == EntityPresetType.UseInsepctor)
            {
                //because we sometimes Reset entities we need to save their orginal data if we are using the inspector preset type.
                inspectorAttributes = new Dictionary<D_Attribute, DeepAttribute>(attributes);
                inspectorResources = new Dictionary<D_Resource, DeepResource>(resources);
                inspectorBehaviors = new List<DeepBehavior>(behaviors);
            }
            ApplyDefaultValues();
        }

        public void ApplyDefaultValues()
        {
            switch (presetType)
            {
                case EntityPresetType.UseInsepctor:
                    attributes = new Dictionary<D_Attribute, DeepAttribute>(inspectorAttributes);
                    foreach (D_Attribute key in inspectorAttributes.Keys)
                    {
                        attributes[key] = attributes[key].Clone();
                    }
                    resources = new Dictionary<D_Resource, DeepResource>(inspectorResources);
                    foreach (D_Resource key in inspectorResources.Keys)
                    {
                        resources[key] = resources[key].Clone();
                    }
                    behaviors = new List<DeepBehavior>();
                    foreach (DeepBehavior b in inspectorBehaviors)
                    {
                        AddBehavior(b);
                    }
                    break;
                case EntityPresetType.PresetObject:
                    if (preset != null)
                    {
                        attributes = new Dictionary<D_Attribute, DeepAttribute>(preset.attributes);
                        foreach (D_Attribute key in preset.attributes.Keys)
                        {
                            attributes[key] = attributes[key].Clone();
                        }
                        resources = new Dictionary<D_Resource, DeepResource>(preset.resources);
                        foreach (D_Resource key in preset.resources.Keys)
                        {
                            resources[key] = resources[key].Clone();
                        }
                        behaviors = new List<DeepBehavior>();
                        foreach (DeepBehavior b in preset.behaviors)
                        {
                            AddBehavior(b);
                        }
                    }
                    else
                    {
                        attributes = new Dictionary<D_Attribute, DeepAttribute>();
                        resources = new Dictionary<D_Resource, DeepResource>();
                        behaviors = new List<DeepBehavior>();
                    }
                    break;
                case EntityPresetType.None:
                    attributes = new Dictionary<D_Attribute, DeepAttribute>();
                    resources = new Dictionary<D_Resource, DeepResource>();
                    behaviors = new List<DeepBehavior>();
                    break;
                default:
                    break;
            }

            foreach(DeepResource res in resources.Values)
            {
                res.parentEntity = this;
            }

        }

        public DeepBehavior AddBehavior(DeepBehavior behavior)
        {
            DeepBehavior b = behavior.Clone();
            b.parent = this;
            behaviors.Add(b);

            b.IntitializeBehavior();

            return b;
        }


        //End Maintanance stuff.

        /// <summary>
        /// Use to get an attribute when you are ok with 0 as a null
        /// </summary>
        /// <returns>Returns 0 if the attribute is null</returns>
        public float GetAttributeValue(D_Attribute attribute)
        {
            if (attributes.ContainsKey(attribute))
            {
                return attributes[attribute].GetValue();
            }
            return 0f;
        }

        public bool TryGetAttributeValue(D_Attribute attribute, out float value)
        {
            if (attributes.ContainsKey(attribute))
            {
                value = attributes[attribute].GetValue();
                return true;
            }
            value = 0f;
            return false;
        }


        //Start behavior event stuff

        public void Hit(float damage)
        {
            foreach (DeepBehavior b in behaviors)
            {
                b.events.OnTakeDamage?.Invoke(damage);
            }
        }

        //standard unity stuff
        private void Update()
        {

            foreach(DeepResource res in resources.Values)
            {
                res.Tick();
            }

            foreach (DeepBehavior b in behaviors)
            {
                b.events.Update?.Invoke();
            }
        }
        private void FixedUpdate()
        {
            foreach (DeepBehavior b in behaviors)
            {
                b.events.FixedUpdate?.Invoke();
            }
        }
        private void LateUpdate()
        {
            foreach (DeepBehavior b in behaviors)
            {
                b.events.LateUpdate?.Invoke();
            }
        }
        private void OnEnable()
        {
            foreach (DeepBehavior b in behaviors)
            {
                b.events.OnEntityEnable?.Invoke();
            }
        }
        private void OnDisable()
        {
            foreach (DeepBehavior b in behaviors)
            {
                b.events.OnEntityDisable?.Invoke();
            }
        }

        #region InspectorStuff
        private string EnumExplanation()
        {
            switch (presetType)
            {
                case EntityPresetType.UseInsepctor:
                    return "Will use the values that are saved in this inspector.";
                case EntityPresetType.PresetObject:
                    return "Will use the values in the supplied object.";
                case EntityPresetType.None:
                    return "Entity values will be wiped on start.";
                default:
                    return "";
            }
        }

        private void InspectorValidate()
        {
            foreach(DeepResource r in resources.Values)
            {
                if (r.parentEntity == null)
                {
                    r.parentEntity = this;
                }
            }
        }
        #endregion
    }
}