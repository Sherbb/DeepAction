using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace DeepAction
{
    public class DeepVFX : MonoBehaviour
    {
        public static DeepVFX instance;

        public static Dictionary<string, VisualEffect> vfxPool { get; private set; } = new Dictionary<string, VisualEffect>();

        void Awake()
        {
            instance = this;
        }

        //attribute is included in out just to save a line everytime we use this...
        public static bool Pull(string vfx, out VisualEffect effect, out VFXEventAttribute eventAttribute)
        {
            if (instance == null)
            {
                Debug.LogError("VFXManager missing");
                effect = null;
                eventAttribute = null;
                return false;
            }

            if (vfxPool.ContainsKey(vfx))
            {
                effect = vfxPool[vfx];
                if (effect == null)
                {
                    Debug.LogError("Unable to find VFX: [" + vfx + "] in resources");
                    eventAttribute = null;
                    return false;
                }
                eventAttribute = effect.CreateVFXEventAttribute();
                return true;
            }

            var effectAsset = Resources.Load(vfx) as VisualEffectAsset;
            if (effectAsset == null)
            {
                Debug.LogError("Unable to find VFX: [" + vfx + "] in resources");
                effect = null;
                eventAttribute = null;
                return false;
            }

            GameObject g = new GameObject("vfx_" + vfx);
            effect = g.AddComponent<VisualEffect>();
            g.transform.parent = instance.transform;
            g.transform.position = Vector3.zero;//should already be set, just in case THIS object is accidentally moved

            effect.visualEffectAsset = effectAsset;

            vfxPool.Add(vfx, effect);
            eventAttribute = effect.CreateVFXEventAttribute();
            return true;
        }
    }
}
