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

        /// <summary>
        /// Returns an event attribute and reference for the given VFX.
        /// </summary>
        /// <returns>true if the VFX exists</returns>
        public static bool Pull(string vfx, out VisualEffect effect, out VFXEventAttribute eventAttribute)
        {
            if (instance == null)
            {
                Debug.LogError("VFXManager missing");
                effect = null;
                eventAttribute = null;
                return false;
            }

            //todo TRYGET
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

            GameObject g = new GameObject("DeepVFX_" + vfx);
            effect = g.AddComponent<VisualEffect>();
            g.layer = 12; //VFX layer
            g.transform.parent = instance.transform;
            g.transform.position = Vector3.zero;

            effect.visualEffectAsset = effectAsset;

            vfxPool.Add(vfx, effect);
            eventAttribute = effect.CreateVFXEventAttribute();
            return true;
        }
    }
}
