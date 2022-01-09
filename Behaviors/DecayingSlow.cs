using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DeepAction
{
    public class DecayingSlow : DeepBehavior
    {
        public float modValue = -.5f;
        DeepAttributeModifier speedMod;
        public float duration = 5f;

        public override void IntitializeBehavior()
        {
            speedMod = new DeepAttributeModifier(0f,modValue,0f);
            parent.attributes[D_Attribute.MoveSpeed].AddModifier(speedMod);
            parent.StartCoroutine(Decay());
        }

        private float timer;
        public IEnumerator Decay()
        {
            while (timer < duration)
            {
                speedMod.multiplier = Mathf.Lerp(modValue,0f,timer/duration);
                timer += Time.deltaTime;
                yield return null;
            }
            parent.RemoveBehavior(this);
        }

        public override void DestroyBehavior()
        {
            parent.attributes[D_Attribute.MoveSpeed].RemoveModifer(speedMod);
        }
    }
}