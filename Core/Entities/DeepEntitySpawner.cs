using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepAction
{
    public class E_Player
    {

        public DeepEntity Player()
        {
            string playerPath = "Player";

            DeepEntity e = GameObject.Instantiate(Resources.Load(playerPath) as GameObject, DeepManager.instance.transform).GetComponent<DeepEntity>();
            e.Initialize(DeepEntityPresets.ExamplePlayer());
            return null;
        }
    }
}
