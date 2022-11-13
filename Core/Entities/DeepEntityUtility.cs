using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DeepAction.Views;

namespace DeepAction
{
    /// <summary>
    /// Extension methods and utilities for DeepEntity
    /// </summary>
    public static class DeepEntityUtility
    {
        //-----------------------------------
        //            RESOURCES
        //-----------------------------------

        public static bool AddResource(this DeepEntity e, D_Resource type, DeepResource resource)
        {
            e.resources.Add(type, resource);
            return true;
        }

        public static bool AddResource(this DeepEntity e, D_Resource type, R resourceTemplate)
        {
            if (e.resources.ContainsKey(type))
            {
                return false;
            }
            e.resources.Add(type, new DeepResource(resourceTemplate.baseMax, resourceTemplate.baseValue));
            return true;
        }

        //-----------------------------------
        //            ATTRIBUTES
        //-----------------------------------

        public static bool AddAttribute(this DeepEntity e, D_Attribute type, DeepAttribute attribute)
        {
            if (e.attributes.ContainsKey(type))
            {
                return false;
            }
            e.attributes.Add(type, attribute);
            return true;
        }

        public static bool AddAttribute(this DeepEntity e, D_Attribute type, A attributeTemplate)
        {
            if (e.attributes.ContainsKey(type))
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

            e.attributes.Add(type, attribute);
            return true;
        }

        //-----------------------------------
        //            FLAGS
        //-----------------------------------

        public static bool SetupFlag(this DeepEntity e, D_Flag s)
        {
            if (e.flags.ContainsKey(s))
            {
                return false;
            }
            e.flags.Add(s, new DeepFlag());
            return true;
        }

        //-----------------------------------
        //            BEHAVIORS
        //-----------------------------------

        public static DeepBehavior AddBehavior<T>(this DeepEntity e) where T : DeepBehavior
        {
            DeepBehavior b = (DeepBehavior)Activator.CreateInstance(typeof(T));
            return e.AddBehavior(b);
        }

        public static DeepBehavior AddBehavior(this DeepEntity e, Type behavior)
        {
            if (!typeof(DeepBehavior).IsAssignableFrom(behavior))
            {
                Debug.LogError("Non DeepBehavior type passed to AddBehavior: " + behavior.ToString());
                return null;
            }
            DeepBehavior b = (DeepBehavior)Activator.CreateInstance(behavior);
            return e.AddBehavior(b);
        }

        public static DeepBehavior AddBehavior(this DeepEntity e, DeepBehavior behavior)
        {
            behavior.parent = e;
            e.behaviors.Add(behavior);
            behavior.InitializeBehavior();
            if (behavior is DeepAbility a)
            {
                e.abilities.Add(a);
            }
            return behavior;
        }

        public static bool RemoveBehavior<T>(this DeepEntity e) where T : DeepBehavior
        {
            foreach (T b in e.behaviors.OfType<T>())
            {
                //the .contains is not neccesary if you are feeling brave
                b.DestroyBehavior();
                if (b is DeepAbility a && e.abilities.Contains(a))
                {
                    e.abilities.Remove(a);
                }
                e.behaviors.Remove(b);
                return true;
            }
            return false;
        }

        public static bool RemoveBehavior(this DeepEntity e, DeepBehavior b)
        {
            if (!e.behaviors.Contains(b))
            {
                return false;
            }
            b.DestroyBehavior();
            //the .contains is not neccesary if you are feeling brave
            if (b is DeepAbility a && e.abilities.Contains(a))
            {
                e.abilities.Remove(a);
            }
            e.behaviors.Remove(b);
            return true;
        }


        //todo optimize this somehow
        public static bool HasBehavior(this DeepEntity e, Type behavior)
        {
            foreach (DeepBehavior b in e.behaviors)
            {
                if (b.GetType() == behavior)
                {
                    return true;
                }
            }
            return false;
        }

        //-----------------------------------
        //            CASTING
        //-----------------------------------

        public static bool TryToCast(this DeepEntity e, int index)
        {
            if (e.abilities.Count > index)
            {
                return e.abilities[index].Trigger();
            }
            Debug.LogError("Tried to cast missing index on: " + e.name + " Index: " + index);
            return false;
        }

        //-----------------------------------
        //            VIEWS
        //-----------------------------------

        public static DeepViewLink AddView(this DeepEntity entity, string view)
        {
            if (!DeepViewManager.instance.viewPool.ContainsKey(view) && !DeepViewManager.instance.RegisterView(view))
            {
                Debug.LogError("Failed to add view to entity");
                return null;
            }

            if (DeepViewManager.instance.viewPool[view].Count < 1)
            {
                DeepViewManager.instance.RegisterView(view, 1);
            }

            DeepViewLink v = DeepViewManager.PullView(view);
            v.transform.parent = entity.transform;
            v.transform.localPosition = Vector3.zero;
            v.transform.localRotation = Quaternion.identity;
            v.transform.localScale = Vector3.one;
            v.gameObject.SetActive(true);
            v.Setup(entity, view);
            entity.RefreshColliderSize();
            return v;
        }
    }
}
