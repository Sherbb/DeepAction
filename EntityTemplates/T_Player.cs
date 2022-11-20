using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepAction
{
    public class T_Player
    {
        public static EntityTemplate BasicPlayer()
        {
            EntityTemplate t = T_Base.Entity();

            t.behaviors = new DeepBehavior[]{
                new PlayerMovement(),
                new PlayerBrain(),
                new PlayerTouch(20f,500f),
                new PlayerAim(),
                new PlayerShoot(.2f,() => DeepEntityPresets.ExamplePlayerProjectile(1)),
                new PlayerShoot(.05f,() => DeepEntityPresets.ExamplePlayerProjectile2(1)),
                new ResourceRegen(D_Resource.Mana,5)
            };

            t.team = D_Team.Player;
            t.type = D_EntityType.Actor;

            return t;
        }
    }
}