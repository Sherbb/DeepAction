using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ActionEntity))]
public class PlayerWithAbilitiesExample : MonoBehaviour
{
    public AbilityObject abilityObject;
    private ActionEntity actionEntity;

    public DeepBehavior ability1;

    private void Start()
    {
        actionEntity = GetComponent<ActionEntity>();

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
