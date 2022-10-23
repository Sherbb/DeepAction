using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace DeepAction.VFX
{
    public abstract class DeepVFXAction
    {
        protected VFXEventAttribute _attribute;
        protected VisualEffect _effect;
        public void Execute(Vector3 position)
        {
            if (_effect == null)
            {
                return;
            }
            _attribute.SetVector3("position", position);
            _effect.SendEvent("OnPlay", _attribute);
        }
    }

    public class Sparks : DeepVFXAction
    {
        public Sparks(Color color, int count = 10)
        {
            if (DeepVFX.Pull("sparks", out _effect, out _attribute))
            {
                _attribute.SetVector3("color", new Vector3(color.r, color.g, color.b));
                _attribute.SetFloat("spawnCount", count);
            }
        }
    }

    public class CirclePop : DeepVFXAction
    {
        public CirclePop(Color color, float radius = 1f, float lifetime = .25f)
        {
            if (DeepVFX.Pull("circlePop", out _effect, out _attribute))
            {
                _attribute.SetVector3("color", new Vector3(color.r, color.g, color.b));
                _attribute.SetFloat("radius", radius);
                _attribute.SetFloat("lifetime", lifetime);
            }
        }
    }

    public class SquarePop : DeepVFXAction
    {
        public SquarePop(Color color, float radius = 1f, float lifetime = .25f)
        {
            if (DeepVFX.Pull("squarePop", out _effect, out _attribute))
            {
                _attribute.SetVector3("color", new Vector3(color.r, color.g, color.b));
                _attribute.SetFloat("radius", radius);
                _attribute.SetFloat("lifetime", lifetime);
            }
        }
    }
}
