using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace DeepAction
{
    [CreateAssetMenu(menuName ="DeepAction/EntityPreset")]
    public class DeepEntityPreset : SerializedScriptableObject
    {
        [Title("Resources", "Deep Action by @AlanSherba", TitleAlignments.Centered)]
        public Dictionary<D_Resource, DeepResource> resources = new Dictionary<D_Resource, DeepResource>();
        [Tooltip("The order in which resources will be drained when the enity takes damage. Starting from the top, and working down. If all resources are drained the entity will Die()")]
        public D_Resource[] damageHeirarchy;

        [Title("Attributes", "", TitleAlignments.Centered)]
        public Dictionary<D_Attribute, DeepAttribute> attributes = new Dictionary<D_Attribute, DeepAttribute>();

        [Title("Behaviors", "", TitleAlignments.Centered),ListDrawerSettings(NumberOfItemsPerPage = 1)]
        [System.NonSerialized, OdinSerialize]
        public List<DeepBehavior> behaviors = new List<DeepBehavior>();
    }
}