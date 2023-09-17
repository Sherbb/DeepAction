using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepAction
{

    public class PlayerOrbitCamera : MonoBehaviour
    {
        public float rotationStrength;
        public float scrollSpeed;
        public Vector2 zoomLimit = new Vector2(100f, 1000f);
        private float currentZoom;
        public float zoomLerp = 5f;

        public Transform zoomTransform;

        private void Start()
        {
            currentZoom = 150f;
        }

        private void LateUpdate()
        {
            currentZoom = Mathf.Clamp(currentZoom + -Input.mouseScrollDelta.y * scrollSpeed * (currentZoom / 100f), zoomLimit.x, zoomLimit.y);
            zoomTransform.localPosition = Vector3.Lerp(zoomTransform.localPosition, new Vector3(0f, 0f, -currentZoom), Time.deltaTime * zoomLerp);
            if (App.state.game.entityByTeamAndTypeLookup[D_Team.Player][D_EntityType.Actor].list.Count > 0)
            {
                //todo derived state
                Vector3 playerPos = App.state.game.entityByTeamAndTypeLookup[D_Team.Player][D_EntityType.Actor][0].transform.position;

                transform.rotation = Quaternion.Euler(-playerPos.y * rotationStrength, playerPos.x * rotationStrength, 0f);
                transform.position = playerPos;
            }
        }
    }
}
