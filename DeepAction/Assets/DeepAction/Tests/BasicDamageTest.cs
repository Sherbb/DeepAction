using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
namespace DeepAction
{
    public class BasicDamageTest : MonoBehaviour
    {
        public float damage;
        private DeepEntity entity;

        private void Awake()
        {
            entity = GetComponent<DeepEntity>();
        }

        [Button]
        public void TestDamage()
        {
            entity.Hit(damage);
        }


    }
}
