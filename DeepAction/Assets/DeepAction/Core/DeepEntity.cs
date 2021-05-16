using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
namespace DeepAction
{
    public class DeepEntity : SerializedMonoBehaviour, IHit
    {
        ///An action entity can:
        ///
        ///Take Damage
        ///Have Behaviors
        ///Behaviors can do pretty much anything. They are abilities, modifiers, in some cases they can control the action entity.

        ///An action entity is:
        ///
        ///the player
        ///ability projectiles
        ///enemies
        ///structures


        [Title("Deep Entity","Deep Action by @AlanSherba",TitleAlignments.Centered)]

        public Dictionary<D_Resources, DeepResource> resources = new Dictionary<D_Resources, DeepResource>();

        [Title("Attributes","",TitleAlignments.Centered)]

        public Dictionary<D_Attribute, DeepAttribute> customAttributes = new Dictionary<D_Attribute, DeepAttribute>();

        [Title("Behaviors","",TitleAlignments.Centered)]

        [System.NonSerialized, OdinSerialize]
        public List<DeepBehavior> behaviors = new List<DeepBehavior>();

        public void Hit()
        {

        }

        public DeepBehavior AddBehavior(DeepBehavior behavior)
        {
            DeepBehavior b = behavior.Clone();
            b.parent = this;
            behaviors.Add(b);
            return b;
        }
    }
}