using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepAction
{
    public struct EntityTemplate
    {
        public Dictionary<D_Resource, R> resources;
        public Dictionary<D_Attribute, A> attributes;
        public DeepBehavior[] behaviors;
        public D_Team team;
        public D_EntityType type;
        //Defining views in the template is optional. Sometimes its easier to define here, sometimes easier when creating entities.
        public string[] views;

        public EntityTemplate(Dictionary<D_Resource, R> resources, Dictionary<D_Attribute, A> attributes, DeepBehavior[] behaviors, D_Team team, D_EntityType type, params string[] extraViews)
        {
            this.resources = resources;
            this.attributes = attributes;
            this.behaviors = behaviors;
            this.team = team;
            this.type = type;
            this.views = extraViews;
        }
    }

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
}
