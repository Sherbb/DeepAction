using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepAction
{
    /// <summary>
    /// Extension methods and utilities for DeepEntity
    /// </summary>
    public static class DeepEntityUtility
    {
        public static bool AddResource(this DeepEntity e, D_Resource type, DeepResource resource)
        {
            e.resources.Add(type, resource);
            return true;
        }
        public static bool AddResource(this DeepEntity e, R resourceTemplate)
        {
            if (e.resources.ContainsKey(resourceTemplate.type))
            {
                return false;
            }
            e.resources.Add(resourceTemplate.type, new DeepResource(resourceTemplate.baseMax, resourceTemplate.baseValue));
            return true;
        }

        public static bool AddAttribute(this DeepEntity e, D_Attribute type, DeepAttribute attribute)
        {
            if (e.attributes.ContainsKey(type))
            {
                return false;
            }
            e.attributes.Add(type, attribute);
            return true;
        }

        public static bool AddAttribute(this DeepEntity e, A attributeTemplate)
        {
            if (e.attributes.ContainsKey(attributeTemplate.type))
            {
                return false;
            }
            DeepAttribute attribute;

            if (attributeTemplate.clamp)
            {
                attribute = new DeepAttribute(attributeTemplate.baseValue, attributeTemplate.minMax);
            }
            else
            {
                attribute = new DeepAttribute(attributeTemplate.baseValue);
            }

            e.attributes.Add(attributeTemplate.type, attribute);
            return true;
        }

        public static bool AddState(this DeepEntity e, D_State s)
        {
            if (e.flags.ContainsKey(s))
            {
                return false;
            }
            e.flags.Add(s, new DeepFlag());
            return true;
        }

        //-----------------------------------
        //            Behaviors
        //-----------------------------------

        public static DeepBehavior AddBehavior<T>(this DeepEntity e) where T : DeepBehavior
        {
            DeepBehavior b = (DeepBehavior)Activator.CreateInstance(typeof(T));
            return e.AddBehavior(b);
        }

        public static DeepBehavior AddBehavior(this DeepEntity e, Type behavior)
        {
            if (!typeof(DeepBehavior).IsAssignableFrom(behavior)) return null;// >:(
            DeepBehavior b = (DeepBehavior)Activator.CreateInstance(behavior);
            return e.AddBehavior(b);
        }

        public static DeepBehavior AddBehavior(this DeepEntity e, DeepBehavior behavior)
        {
            behavior.parent = e;
            e.behaviors.Add(behavior);
            behavior.IntitializeBehavior();
            return behavior;
        }

        public static bool RemoveBehavior<T>(this DeepEntity e) where T : DeepBehavior
        {
            foreach (T b in e.behaviors.OfType<T>())
            {
                b.DestroyBehavior();
                e.behaviors.Remove(b);
                return true;
            }
            return false;
        }

        public static bool RemoveBehavior(this DeepEntity e, DeepBehavior behavior)
        {
            if (!e.behaviors.Contains(behavior))
            {
                return false;
            }
            behavior.DestroyBehavior();
            e.behaviors.Remove(behavior);
            return true;
        }
    }
}
