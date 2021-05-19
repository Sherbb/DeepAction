using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;

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
        [OnInspectorGUI("InspectorValidate")]//prolly remove this


        //[Title("Preset", "Determines where the entity will pull its start values from", TitleAlignments.Centered)]
        [BoxGroup("Settings")]
        public OnDeathBehavior deathBehavior = OnDeathBehavior.Destroy;
        public enum OnDeathBehavior{Disable,Destroy,Nothing}
        [HideInInspector]
        public Action OnDeath;

        public enum InitializeOnEnable {Always, AfterDeath, Never}
        
        [HorizontalGroup("Settings/hoz")][Tooltip("AfterDeath = After being intialized once, the entity must DIE to be initialized on enable again.")]
        public InitializeOnEnable onEnableInitialize = InitializeOnEnable.AfterDeath;
        [HorizontalGroup("Settings/hoz")][ToggleLeft][Tooltip("This will completly reset all resources, attributes, and behaviors. This will create a lot of garbage! If you code your behaviors nicely you usually won't need this. Entities will always pull from preset on AWAKE.")]
        public bool applyPreset = false;
        private bool hasDied;//used to track for after death
        [BoxGroup("Settings")]//[Title("Preset Type","Determines where the entity will draw its default values from")]
        public enum EntityPresetType { UseInspector, PresetObject, CompositeObjects, None }
        [BoxGroup("Settings")][Tooltip("Determines where the entity will draw its default values from")]
        /*[InfoBox("$EnumExplanation", InfoMessageType.None)]*/[ HideInPlayMode]
        public EntityPresetType presetType = EntityPresetType.UseInspector;
        [BoxGroup("Settings")]
        [ShowIf("@presetType == EntityPresetType.PresetObject &&  !UnityEngine.Application.isPlaying"),InlineEditor]
        public DeepEntityPreset preset;




        [Space]
        [ShowIf("@presetType == EntityPresetType.UseInspector || UnityEngine.Application.isPlaying")]
        [Title("Resources", "", TitleAlignments.Centered)]
        public Dictionary<D_Resource, DeepResource> resources = new Dictionary<D_Resource, DeepResource>();
        [ShowIf("@presetType == EntityPresetType.UseInspector || UnityEngine.Application.isPlaying")]
        [Tooltip("The order in which resources will be drained when the enity takes damage. Starting from the top, and working down. If the bottom resources in the heirarchy is drained, the entity will Die()")]
        public D_Resource[] damageHeirarchy;

        [Space]
        [ShowIf("@presetType == EntityPresetType.UseInspector || UnityEngine.Application.isPlaying")]
        [Title("Attributes", "", TitleAlignments.Centered)]
        public Dictionary<D_Attribute, DeepAttribute> attributes = new Dictionary<D_Attribute, DeepAttribute>();

        [Space]
        [ShowIf("@presetType == EntityPresetType.UseInspector || UnityEngine.Application.isPlaying")]
        [Title("Behaviors", "", TitleAlignments.Centered),ListDrawerSettings(NumberOfItemsPerPage = 1)]
        [System.NonSerialized, OdinSerialize]
        public List<DeepBehavior> behaviors = new List<DeepBehavior>();

        #region inspectorPreset
        private Dictionary<D_Resource, DeepResource> inspectorResources = new Dictionary<D_Resource, DeepResource>();
        private Dictionary<D_Attribute, DeepAttribute> inspectorAttributes = new Dictionary<D_Attribute, DeepAttribute>();
        private List<DeepBehavior> inspectorBehaviors = new List<DeepBehavior>();
        private D_Resource[] inspectorDamageHeirarchy;
        #endregion


        //other
        




        private void Awake()
        {
            if (presetType == EntityPresetType.UseInspector)
            {
                //because we sometimes Reset entities we need to save their orginal data if we are using the inspector preset type.
                inspectorAttributes = new Dictionary<D_Attribute, DeepAttribute>(attributes);
                inspectorResources = new Dictionary<D_Resource, DeepResource>(resources);
                inspectorBehaviors = new List<DeepBehavior>(behaviors);
                inspectorDamageHeirarchy = (D_Resource[])damageHeirarchy.Clone();
            }
            InitializeEntity(true);
        }

        public void InitializeEntity(bool doPresetApply)
        {
            if (doPresetApply)
            {
                ApplyPreset();
            }
            foreach(DeepResource res in resources.Values)
            {
                res.parentEntity = this;
                res.SetValueWithRatio(res.defaultRatio);
            }
            hasDied = false;
        }

        public void ApplyPreset()
        {
            switch (presetType)
            {
                case EntityPresetType.UseInspector:
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
                    damageHeirarchy = inspectorDamageHeirarchy;
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
                        damageHeirarchy = preset.damageHeirarchy;
                    }
                    else
                    {
                        attributes = new Dictionary<D_Attribute, DeepAttribute>();
                        resources = new Dictionary<D_Resource, DeepResource>();
                        behaviors = new List<DeepBehavior>();
                        damageHeirarchy = new D_Resource[0];
                    }
                    break;
                case EntityPresetType.None:
                    attributes = new Dictionary<D_Attribute, DeepAttribute>();
                    resources = new Dictionary<D_Resource, DeepResource>();
                    behaviors = new List<DeepBehavior>();
                    damageHeirarchy = new D_Resource[0];
                    break;
                default:
                    break;
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

        /// <summary>
        /// Removes the behavior from the entity and calls DestroyAction() on all actions.
        /// Remember that the object won't actually be destroyed if you are still holding a reference to it somewhere.
        /// </summary>
        /// <returns>Returns true if the enity had the behavior</returns>
        public bool RemoveBehavior(DeepBehavior behavior)
        {
            if (!behaviors.Contains(behavior))
            {
                return false;
            }

            behavior.RemoveBehavior();
            behaviors.Remove(behavior);
            return true;
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
            float d = damage;
            foreach (DeepBehavior b in behaviors)
            {
                b.events.OnTakeDamage?.Invoke(d);
                b.events.OnTakeDamageRef?.Invoke(ref d);
            }

            if (damageHeirarchy.Length == 0)
            {
                Die();
            }

            for (int i = 0; i < damageHeirarchy.Length; i++)
            {
                d = resources[damageHeirarchy[i]].ConsumeWithRemainder(d);
                if (d <= 0f)
                {
                    break;
                }
            }

            if (resources[damageHeirarchy[damageHeirarchy.Length-1]].GetValue() <= 0)
            {
                Die();
            }
        }

        public void Die()
        {
            OnDeath?.Invoke();

            foreach (DeepBehavior b in behaviors)
            {
                b.events.OnEntityDie?.Invoke();
            }

            hasDied = true;

            switch (deathBehavior)
            {
                case OnDeathBehavior.Disable:
                    gameObject.SetActive(false);
                    break;
                case OnDeathBehavior.Destroy:
                    Destroy(gameObject);
                    break;
                case OnDeathBehavior.Nothing:
                    //uhh
                    break;
            }



        }

        //standard unity stuff
        private void Update()
        {

            foreach(DeepResource res in resources.Values)
            {
                res.Tick();
            }
            if (resources[damageHeirarchy[damageHeirarchy.Length-1]].GetValue() <= 0)
            {
                //resources can have - regen. 
                //So if your hp has negative regen we wanna see if you died here
                Die();
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
            switch (onEnableInitialize)
            {
                case InitializeOnEnable.Always:
                    InitializeEntity(applyPreset);
                    break;
                case InitializeOnEnable.AfterDeath:
                    if (hasDied)
                    {
                        InitializeEntity(applyPreset);
                    }
                    break;
                case InitializeOnEnable.Never:
                    //nope
                    break;
            }


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
                case EntityPresetType.UseInspector:
                    return "Will use the values that are saved in this inspector.";
                case EntityPresetType.PresetObject:
                    return "Will use the values in the supplied object.";
                case EntityPresetType.CompositeObjects:
                    return "Will use the multiple supplied objects to build the preset.";
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