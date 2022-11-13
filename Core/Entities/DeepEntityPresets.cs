using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepAction
{
    public class DeepEntityPresets
    {
        //example
        public static EntityTemplate StaticBaseEntity = new EntityTemplate(
            new Dictionary<D_Resource, R>
            {
                {D_Resource.Health, new R(3)},
                {D_Resource.Mana, new R(3)},
                {D_Resource.Shield, new R(3,0)},
            },
            new Dictionary<D_Attribute, A>
            {
                {D_Attribute.Strength,new A(0)},
                {D_Attribute.Dexterity,new A(0)},
                {D_Attribute.Inteligence,new A(0)},
            },
            new DeepBehavior[0],
            D_Team.Neutral,
            D_EntityType.Actor
        );

        //example
        public static EntityTemplate BaseEntity()
        {
            var resources = new Dictionary<D_Resource, R>
            {
                {D_Resource.Health, new R(10)},
                {D_Resource.Mana, new R(10)},
                {D_Resource.Shield, new R(3,0)},
            };

            var attributes = new Dictionary<D_Attribute, A>
             {
                {D_Attribute.Strength,new A(1)},
                {D_Attribute.Inteligence,new A(1)},
                {D_Attribute.Dexterity,new A(1)},
                {D_Attribute.MoveSpeed,new A(40)},
                {D_Attribute.MaxMoveSpeed,new A(40)},
                {D_Attribute.Drag,new A(0)},
                {D_Attribute.Bounciness,new A(1)},
                {D_Attribute.SlideFriction,new A(1)},
                {D_Attribute.MovementRadius,new A(1)},
             };

            DeepBehavior[] behaviors = {
            };

            return new EntityTemplate(resources, attributes, behaviors, D_Team.Neutral, D_EntityType.Actor);
        }

        public static EntityTemplate BaseProjectile()
        {
            var resources = new Dictionary<D_Resource, R>
            {
                {D_Resource.Health, new R(3)},
                {D_Resource.Mana, new R(3)},
                {D_Resource.Shield, new R(3,0)},
            };

            var attributes = new Dictionary<D_Attribute, A>
            {
                {D_Attribute.Strength,new A(1)},
                {D_Attribute.Inteligence,new A(1)},
                {D_Attribute.Dexterity,new A(1)},
                {D_Attribute.MoveSpeed,new A(150)},
                {D_Attribute.MaxMoveSpeed,new A(200)},
                {D_Attribute.Drag,new A(0)},
                {D_Attribute.Bounciness,new A(1)},
                {D_Attribute.SlideFriction,new A(1)},
                {D_Attribute.MovementRadius,new A(1)},
            };

            DeepBehavior[] behaviors = {
            };

            return new EntityTemplate(resources, attributes, behaviors, D_Team.Neutral, D_EntityType.Actor);
        }

        public static EntityTemplate ExampleEnemy()
        {
            EntityTemplate t = BaseEntity();

            t.attributes[D_Attribute.MoveSpeed] = new A(Random.Range(20f, 40f));
            t.attributes[D_Attribute.MaxMoveSpeed] = new A(Random.Range(20f, 40f));

            t.behaviors = new DeepBehavior[]{
                new MoveTowardsPlayer(),
                new AvoidOtherEntities(D_Team.Enemy,D_EntityType.Actor,60f),
                new VFXOnDeath(
                    new VFX.Sparks(new Color(1f,.256f,.256f),5),
                    new VFX.SquarePop(new Color(1f,.256f,.256f),5f,.2f)
                ),
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
                new PlayerBrain(),
                new PlayerTouch(20f,500f),
                new PlayerAim(),
                new PlayerShoot(1,() => ExamplePlayerProjectile(1)),
                new PlayerShoot(.1f,() => ExamplePlayerProjectile2(3)),
                new ResourceRegen(D_Resource.Mana,5)
            };

            t.team = D_Team.Player;
            t.type = D_EntityType.Actor;

            return t;
        }

        public static EntityTemplate ExamplePlayerProjectile(int damage)
        {
            EntityTemplate t = BaseProjectile();

            float aoeRadius = 1f;

            t.behaviors = new DeepBehavior[]{
                new BasicProjectile(damage,D_Team.Enemy),
                new MoveForwards(),
                new DieOnBounce(),
                new VFXOnDeath(
                    new VFX.Sparks(Color.black,5),
                    new VFX.CirclePop(Color.black,aoeRadius,.2f)
                ),
            };

            t.team = D_Team.Player;
            t.type = D_EntityType.Projectile;

            return t;
        }

        public static EntityTemplate ExamplePlayerProjectile2(int damage)
        {
            EntityTemplate t = BaseProjectile();

            float aoeRadius = 4f;

            t.behaviors = new DeepBehavior[]{
                new BasicProjectile(0,D_Team.Enemy),
                new MoveForwards(),
                new DieOnBounce(),
                //new AreaDamageOnDeath(aoeRadius,new Damage(damage),D_Team.Enemy),
                new AreaImpulseOnDeath(aoeRadius, 150f,D_Team.Enemy),
                new AreaBehaviorOnDeath(aoeRadius,new PopOnDamageExample(aoeRadius,damage,D_Team.Enemy),D_Team.Enemy,false),
                new VFXOnDeath(
                    new VFX.Sparks(Color.black,5),
                    new VFX.CirclePop(Color.black,aoeRadius,.2f)
                ),
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
    public struct R//single letter to make defining a template really clean 
    {
        public int baseMax { get; private set; }
        public int baseValue { get; private set; }

        public R(int baseMax)
        {
            this.baseMax = baseMax;
            this.baseValue = baseMax;
        }
        public R(int baseMax, int startValue)
        {
            this.baseMax = baseMax;
            this.baseValue = startValue;
        }
    }

    //attribute template
    public struct A
    {
        public float baseValue { get; private set; }
        public bool clamp { get; private set; }
        public Vector2 minMax { get; private set; }

        public A(float baseValue)
        {
            this.baseValue = baseValue;
            this.clamp = false;
            this.minMax = Vector2.zero;
        }
        public A(float baseValue, Vector2 minMax)
        {
            this.baseValue = baseValue;
            this.clamp = true;
            this.minMax = minMax;
        }
    }

    public struct EntityTemplate
    {
        public Dictionary<D_Resource, R> resources;
        public Dictionary<D_Attribute, A> attributes;
        public DeepBehavior[] behaviors;
        public D_Team team;
        public D_EntityType type;

        public EntityTemplate(Dictionary<D_Resource, R> resources, Dictionary<D_Attribute, A> attributes, DeepBehavior[] behaviors, D_Team team, D_EntityType type)
        {
            this.resources = resources;
            this.attributes = attributes;
            this.behaviors = behaviors;
            this.team = team;
            this.type = type;
        }
    }
}
