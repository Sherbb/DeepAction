using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class ActionEntity : SerializedMonoBehaviour , IHit
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

    public Dictionary<string,ResourceStat> resources = new Dictionary<string, ResourceStat>();

    //standard attributes that EVERY ENTITY IN THE GAME HAS
    public BaseAttribute
        moveSpeed = new BaseAttribute(),
        healthRegen = new BaseAttribute(),
        armor = new BaseAttribute();

    public Dictionary<string,BaseAttribute> customAttributes = new Dictionary<string, BaseAttribute>();

    [System.NonSerialized,OdinSerialize]
    public List<DeepBehavior>behaviors = new List<DeepBehavior>();

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