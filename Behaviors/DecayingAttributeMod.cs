using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A generic example of the decaying slow

namespace DeepAction
{
    public class DecayingAttributeMod : DeepBehavior
    {
        public Vector3 modBase;
        public float duration = 1f;
        public D_Attribute attribute;

        private DeepAttributeModifier attMod;
        private float timer;

        public override void IntitializeBehavior()
        {
            attMod = new DeepAttributeModifier(modBase.x, modBase.y, modBase.z);
            parent.attributes[attribute].AddModifier(attMod);
            parent.StartCoroutine(Decay());
        }

        public IEnumerator Decay()
        {
            while (timer < duration)
            {
                attMod.UpdateModifier(
                Mathf.Lerp(modBase.x, 0f, timer / duration),
                Mathf.Lerp(modBase.y, 0f, timer / duration),
                Mathf.Lerp(modBase.z, 0f, timer / duration)
                );

                timer += Time.deltaTime;
                yield return null;
            }
            parent.RemoveBehavior(this);
        }

        public override void DestroyBehavior()
        {
            parent.attributes[attribute].RemoveModifer(attMod);
        }

        /*
        private void ExampleUsage()
        {
            DeepEntity e = new DeepEntity();

            DecayingAttributeMod b = new DecayingAttributeMod();
            b.modBase = new Vector3(0f,.5f,0f);
            b.attribute = D_Attribute.MoveSpeed;
            b.duration = 2f;

            //add a 50% move speed buff to entity
            //the buff will decay over 2 seconds.
            e.AddBehavior(b);
        }
        */
    }
}