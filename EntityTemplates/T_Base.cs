using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DeepAction
{
    /// <summary>
    /// Contains some usefull base or "default" entity templates.
    /// </summary>
    public class T_Base
    {
        public static EntityTemplate Entity()
        {
            var resources = new Dictionary<D_Resource, R>
            {
                {D_Resource.Health, new R(5)},
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
    }
}