using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace DeepAction.Views
{
    public class HitReactionView : MonoBehaviour
    {
#if ODIN_INSPECTOR
        [SuffixLabel("Null = this")]
#endif
        public DeepViewLink link;

#if ODIN_INSPECTOR
        [Title("Scale")]
#endif
        [SerializeField]
        private bool scaleOnHit;
        private int scaleID;
        private Vector3 defaultScale;
#if ODIN_INSPECTOR
        [ShowIf("scaleOnHit")]
#endif
        public Vector3 hitScale;
#if ODIN_INSPECTOR
        [ShowIf("scaleOnHit")]
#endif
        public float scaleResetTime;
#if ODIN_INSPECTOR
        [ShowIf("scaleOnHit")]
#endif
        public LeanTweenType scaleCurve = LeanTweenType.easeOutCubic;

#if ODIN_INSPECTOR
        [Title("Material")]
#endif
        [SerializeField]
        private bool materialOnHit;
        private int materialID;
#if ODIN_INSPECTOR
        [ShowIf("materialOnHit")]
#endif
        public float materialResetTime;
#if ODIN_INSPECTOR
        [ShowIf("materialOnHit"), SuffixLabel("Null = this")]
#endif
        [SerializeField]
        private MeshRenderer mr;
#if ODIN_INSPECTOR
        [ShowIf("materialOnHit")]
#endif
        public Material hitMaterial;
#if ODIN_INSPECTOR
        [ShowIf("materialOnHit"), SuffixLabel("Null = default")]
#endif
        public Material defaultMaterial;


        private void Awake()
        {
            if (link == null)
            {
                link = GetComponent<DeepViewLink>();
            }

            link.onSetup += Setup;
            link.onStartReturn += Teardown;

            if (mr == null && TryGetComponent(out mr))
            {
                materialOnHit = false;
            }
            else if (defaultMaterial == null)
            {
                defaultMaterial = mr.material;
            }

            defaultScale = transform.localScale;
        }

        private void Setup()
        {
            link.entity.events.OnTakeDamage += OnHit;
        }

        private void Teardown()
        {
            link.entity.events.OnTakeDamage -= OnHit;
        }

        private void OnHit(float damage)
        {
            if (materialOnHit)
            {
                LeanTween.cancel(materialID);
                materialID = LeanTween.value(gameObject, ScaleAnimate, 0f, 1f, scaleResetTime).setEase(scaleCurve).id;
                mr.material = hitMaterial;
            }
            if (scaleOnHit)
            {
                LeanTween.cancel(scaleID);
                scaleID = LeanTween.delayedCall(materialResetTime, MaterialTimer).id;
            }
        }

        private void ScaleAnimate(float t)
        {
            transform.localScale = Vector3.Lerp(hitScale, defaultScale, t);
        }

        private void MaterialTimer()
        {
            mr.material = defaultMaterial;
        }
    }
}
