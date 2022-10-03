using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A generic example of the decaying slow

namespace DeepAction
{
    public class DecayingAttributeMod : DeepBehavior
    {
        private D_Attribute _attribute;
        private ModValues _modBase;
        private float _duration = 1f;

        private DeepAttributeModifier attMod;
        private float timer;

        public DecayingAttributeMod(D_Attribute attribute, ModValues mod, float duration)
        {
            _attribute = attribute;
            _modBase = mod;
            _duration = duration;
        }

        public override void InitializeBehavior()
        {
            attMod = new DeepAttributeModifier(_modBase);
            parent.attributes[_attribute].AddModifier(attMod);
            parent.StartCoroutine(Decay());
        }

        public IEnumerator Decay()
        {
            while (timer < _duration)
            {
                attMod.UpdateModifier(
                Mathf.Lerp(_modBase.baseAdd, 0f, timer / _duration),
                Mathf.Lerp(_modBase.multiplier, 0f, timer / _duration),
                Mathf.Lerp(_modBase.postAdd, 0f, timer / _duration)
                );

                timer += Time.deltaTime;
                yield return null;
            }
            parent.RemoveBehavior(this);
        }

        public override void DestroyBehavior()
        {
            parent.attributes[_attribute].RemoveModifer(attMod);
        }
    }
}