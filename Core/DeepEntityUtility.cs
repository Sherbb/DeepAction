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
            if (e.states.ContainsKey(s))
            {
                return false;
            }
            e.states.Add(s, new DeepState());
            return true;
        }
    }
}
