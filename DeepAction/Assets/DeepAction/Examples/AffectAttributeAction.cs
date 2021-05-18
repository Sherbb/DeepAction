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

        [HideInEditorMode,ReadOnly]
        public DeepAttributeModifier activeModifier;

        public override void IntitializeAction()
        {
            if (behavior.parent.attributes.ContainsKey(attributeToModify))
            {
                activeModifier = behavior.parent.attributes[attributeToModify].AddModifier(new DeepAttributeModifier(modifier));
                activeModifier.source += "/" + behavior.behaviorID + "/" + behavior.parent.name;//it would be nice if we could do this automatically...ohwell
            }
            else
            {
                activeModifier = null;
            }
        }

        public override void DestroyAction()
        {
            if (activeModifier != null)
            {
                if (behavior.parent.attributes.ContainsKey(attributeToModify))
                {
                    behavior.parent.attributes[attributeToModify].RemoveModifer(activeModifier);
                }
            }
            activeModifier = null;
        }
    }
}