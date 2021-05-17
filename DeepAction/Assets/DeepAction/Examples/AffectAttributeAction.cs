using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
namespace DeepAction
{
    public class AffectAttributeAction : DeepAction
    {
        public DeepAttributeModifier modifier = new DeepAttributeModifier();
        public D_Attribute attributeToModify;

        public DeepAttributeModifier mod;

        public bool random;

        public override void IntitializeAction()
        {
            if (behavior.parent.attributes.ContainsKey(attributeToModify))
            {
                mod = behavior.parent.attributes[attributeToModify].AddModifier(new DeepAttributeModifier(modifier));
                mod.source += "/" + behavior.behaviorID + "/" + behavior.parent.name;//if would be nice if we could do this automatically...ohwell
            }
            behavior.events.Update += Update;
        }

        private void Update()
        {
            if(random) mod.multiplier = Random.Range(0f,1f);
        }
    }
}