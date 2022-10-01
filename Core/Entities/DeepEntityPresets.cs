using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepAction
{
    public class DeepEntityPresets
    {
        //example
        public static EntityTemplate StaticBaseEntity = new EntityTemplate(
            new[] {
                new R(D_Resource.Health,1),
                new R(D_Resource.Shield,1,0),
                new R(D_Resource.Mana,1),
            },
            new[] {
                new A(D_Attribute.Strength,0),
                new A(D_Attribute.Inteligence,1),
                new A(D_Attribute.Dexterity,0),
            },
            new DeepBehavior[0],
            D_Team.Neutral,
            D_EntityType.Actor
        );

        //example
        public static EntityTemplate BaseEntity()
        {
            R[] resources = {
                new R(D_Resource.Health,3),
                new R(D_Resource.Mana,1),
                new R(D_Resource.Shield,1,0)
            };

            A[] attributes = {
                new A(D_Attribute.Strength,1),
                new A(D_Attribute.Inteligence,1),
                new A(D_Attribute.Dexterity,1),
                new A(D_Attribute.MoveSpeed,40f),
                new A(D_Attribute.MaxMoveSpeed,40f),
                new A(D_Attribute.Drag,0f),
                new A(D_Attribute.Bounciness,1f),
                new A(D_Attribute.SlideFriction,.1f),
                new A(D_Attribute.MovementRadius,1f),
            };

            DeepBehavior[] behaviors = {
            };

            return new EntityTemplate(resources, attributes, behaviors, D_Team.Neutral, D_EntityType.Actor);
        }

        public static EntityTemplate ExampleEnemy()
        {
            EntityTemplate t = BaseEntity();
            t.behaviors = new DeepBehavior[]{
                new MoveTowardsPlayer()
            };
            t.team = D_Team.Enemy;
            t.type = D_EntityType.Actor;

            return t;
        }

        public static EntityTemplate ExamplePlayer()
        {
            EntityTemplate t = BaseEntity();

            t.behaviors = new DeepBehavior[]{
                new PlayerMovement(),
                new PlayerTouch(20f,500f),
                new PlayerAim(),
                new PlayerShoot(),
            };

            t.team = D_Team.Player;
            t.type = D_EntityType.Actor;

            return t;
        }

        public static EntityTemplate ExamplePlayerProjectile()
        {
            EntityTemplate t = BaseEntity();

            t.behaviors = new DeepBehavior[]{
                new BasicProjectile(1,D_Team.Enemy),
                new MoveForwards(),
            };

            t.team = D_Team.Player;
            t.type = D_EntityType.Projectile;

            return t;
        }
    }

    //-----------------------------------
    //            Structs
    //-----------------------------------

    //resource template
    public struct R//single letter to make defining a template really clean ^ if this annoys you change
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

    //attribute template
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
        public D_Team team;
        public D_EntityType type;

        public EntityTemplate(R[] resources, A[] attributes, DeepBehavior[] behaviors, D_Team team, D_EntityType type)
        {
            this.resources = resources;
            this.attributes = attributes;
            this.behaviors = behaviors;
            this.team = team;
            this.type = type;
        }
    }
}
