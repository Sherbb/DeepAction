using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepAction
{
    public class DeepEntityPresets
    {
        public static EntityTemplate StaticBaseEntity = new EntityTemplate(
            new[] {
                new R(D_Resource.Health,1),
                new R(D_Resource.Mana,1),
                new R(D_Resource.Energy,1),
            },
            new[] {
                new A(D_Attribute.Strength,0),
                new A(D_Attribute.Inteligence,0),
                new A(D_Attribute.Dexterity,0),
            },
            new DeepBehavior[0]
        );



        public EntityTemplate BaseEntity(int lvl)
        {
            R[] resources = {
                new R(D_Resource.Health,lvl),
                new R(D_Resource.Mana,lvl),
                new R(D_Resource.Energy,lvl)
            };

            A[] attributes = {
                new A(D_Attribute.Strength,lvl),
                new A(D_Attribute.Inteligence,lvl),
                new A(D_Attribute.Dexterity,lvl),
            };

            DeepBehavior[] behaviors = {
            };

            return new EntityTemplate(resources, attributes, behaviors);
        }

        public EntityTemplate ExampleEnemy(int lvl)
        {
            EntityTemplate t = BaseEntity(lvl);
            t.behaviors = new[]{
                new DecayingSlow()
            };

            return t;
        }
    }

    //-----------------------------------
    //            Structs
    //-----------------------------------

    public struct R//single letter to make defining a template really clean ^
    {
        public D_Resource type { get; private set; }
        public int baseMax { get; private set; }
        public int baseValue { get; private set; }

        public R(D_Resource type, int baseMax)
        {
            this.type = type;
            this.baseMax = baseMax;
            this.baseValue = baseMax;
        }
        public R(D_Resource type, int baseMax, int startValue)
        {
            this.type = type;
            this.baseMax = baseMax;
            this.baseValue = startValue;
        }
    }

    public struct A
    {
        public D_Attribute type { get; private set; }
        public float baseValue { get; private set; }
        public bool clamp { get; private set; }
        public Vector2 minMax { get; private set; }

        public A(D_Attribute type, float baseValue)
        {
            this.type = type;
            this.baseValue = baseValue;
            this.clamp = false;
            this.minMax = Vector2.zero;
        }
        public A(D_Attribute type, float baseValue, Vector2 minMax)
        {
            this.type = type;
            this.baseValue = baseValue;
            this.clamp = true;
            this.minMax = minMax;
        }
    }

    public struct EntityTemplate
    {
        public R[] resources;
        public A[] attributes;
        public DeepBehavior[] behaviors;

        public EntityTemplate(R[] resources, A[] attributes, DeepBehavior[] behaviors)
        {
            this.resources = resources;
            this.attributes = attributes;
            this.behaviors = behaviors;
        }
    }
}
