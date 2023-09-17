using UnityEngine;

namespace DeepAction
{
    public class AvoidOtherEntities : DeepBehavior
    {
        //* Optimization settings.
        private const int UPDATE_EVERY = 3;
        private const int MAX_COLLISIONS = 3;//how many valid collisions we consider per update.

        public D_Team teamToAvoid;
        public D_EntityType typeToAvoid;
        public float avoidStrength;

        private Vector3 nudge;
        private Vector3 parentPos;
        private Vector3 dir;
        private int frame;
        private float deltaTimeAcrossFrames;

        public AvoidOtherEntities(D_Team team, D_EntityType type, float force)
        {
            teamToAvoid = team;
            typeToAvoid = type;
            avoidStrength = force;
        }

        public override void InitializeBehavior()
        {
            parent.events.OnEntityCollisionStay += Avoid;
        }

        public override void DestroyBehavior()
        {
            parent.events.OnEntityCollisionStay -= Avoid;
        }

        private void Avoid()
        {
            deltaTimeAcrossFrames += Time.deltaTime;
            if (frame % UPDATE_EVERY != 0)
            {
                frame++;
                return;
            }
            frame++;

            nudge = Vector3.zero;
            parentPos = parent.transform.position;
            float deltaStrength = deltaTimeAcrossFrames * avoidStrength;
            int i = 0;
            foreach (DeepEntity entity in parent.activeCollisions.Values)
            {
                if (entity.team == teamToAvoid || entity.type == typeToAvoid)
                {
                    dir = parentPos - entity.transform.position;
                    //slightly faster normalize. (we might run thousands per frame)
                    nudge += (dir / Mathf.Sqrt(dir.sqrMagnitude)) * deltaStrength;
                    i++;
                    if (i > MAX_COLLISIONS)
                    {
                        continue;
                    }
                }
            }
            parent.mb.AddForce(nudge);
            deltaTimeAcrossFrames = 0f;
        }
    }
}
