using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace DeepAction
{
    public class DeepEntityBuilder : MonoBehaviour
    {
        private const string baseEntityPath = "BaseEntity";
        private GameObject _baseEntity;
        private GameObject baseEntity
        {
            get
            {
                if (_baseEntity == null)
                {
                    _baseEntity = Resources.Load(baseEntityPath) as GameObject;
                }

                return _baseEntity;
            }
        }
        [Button]
        public DeepEntity CreateFromPrefab()
        {
            DeepEntity e = GameObject.Instantiate(baseEntity, DeepManager.instance.transform).GetComponent<DeepEntity>();
            e.Initialize(DeepEntityPresets.StaticBaseEntity);

            return e;
        }

        [Button]
        public DeepEntity CreateFromScratch()
        {
            GameObject g = new GameObject();
            g.transform.parent = DeepManager.instance.transform;
            DeepEntity e = g.AddComponent<DeepEntity>();
            e.Initialize(DeepEntityPresets.StaticBaseEntity);

            return e;
        }
    }
}
