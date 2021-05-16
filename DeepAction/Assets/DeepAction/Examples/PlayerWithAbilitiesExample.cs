using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DeepAction
{
    [RequireComponent(typeof(DeepEntity))]
    public class PlayerWithAbilitiesExample : MonoBehaviour
    {
        public AbilityObject abilityObject;
        private DeepEntity actionEntity;

        public DeepBehavior ability1;

        private void Start()
        {
            actionEntity = GetComponent<DeepEntity>();

            ability1 = actionEntity.AddBehavior(abilityObject.ability.behavior);
        }


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (ability1.Trigger())
                {
                    //we casted ability
                }
            }
        }

    }
}