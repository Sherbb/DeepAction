using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace DeepAction
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

    //example
    public class SimpleSparks : DeepVFXAction
    {
        public SimpleSparks(int count, Color color, float radius)
        {
            if (DeepVFX.Pull("sparks", out _effect, out _attribute))
            {
                _attribute.SetVector3("color", new Vector4(color.r, color.g, color.b));
                _attribute.SetFloat("radius", radius);
                _attribute.SetFloat("spawnCount", count);
            }
        }
    }
}
