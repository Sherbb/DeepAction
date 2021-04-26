using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ActionEntity : SerializedMonoBehaviour
{
    ///An action entity can:
    ///
    ///Take Damage
    ///Have and cast abilites
    ///Have Modifiers (Status effects)

    public ResourceStat health;
    
    public Dictionary<string,ResourceStat> resources;

    public Dictionary<AttributeType,BaseAttribute> attributes;

    public Ability[] abilities;

    public virtual void Hit()
    {

    }
}

public enum AttributeType
{
    strength,
    intelligence,
    dexterity,
    movespeed,
    etc,

}