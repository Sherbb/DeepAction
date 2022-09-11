using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace DeepAction
{
    public class App : MonoBehaviour
    {
        [ShowInInspector]
        private static S_App _state;
        public static S_App state
        {
            get
            {
                if (_state == null)
                {
                    _state = new S_App();
                }
                return _state;
            }
        }
    }
}
