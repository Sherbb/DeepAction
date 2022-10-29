using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepAction
{

    public class PlayerOrbitCamera : MonoBehaviour
    {
        public float rotationStrength;

        private void Update()
        {
            if (App.state.game.entityByTeamAndTypeLookup[D_Team.Player][D_EntityType.Actor].list.Count > 0)
            {
                //todo derived state
                Vector3 playerPos = App.state.game.entityByTeamAndTypeLookup[D_Team.Player][D_EntityType.Actor][0].transform.position;

                transform.rotation = Quaternion.Euler(-playerPos.y * rotationStrength, playerPos.x * rotationStrength,0f);
            }
        }
    }
}
