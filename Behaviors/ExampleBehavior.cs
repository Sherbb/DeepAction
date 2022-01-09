using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DeepAction
{
    public class ExampleBehavior : DeepBehavior
    {
        public DeepAttributeModifier strMod = new DeepAttributeModifier(10f,0f,0f);

        public override void IntitializeBehavior()
        {
            parent.attributes[D_Attribute.Strength].AddModifier(strMod);
            parent.StartCoroutine(DestroyCo(parent));
        }

        IEnumerator DestroyCo(DeepEntity deepEntity)
        {
            yield return new WaitForSeconds(5f);
            parent.RemoveBehavior(this);
        }
        
        public override void DestroyBehavior()
        {
            parent.attributes[D_Attribute.Strength].RemoveModifer(strMod);
        }
    }
}