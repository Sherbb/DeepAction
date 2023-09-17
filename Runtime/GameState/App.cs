using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace DeepAction
{
    [DefaultExecutionOrder(-500)]
    public class App : MonoBehaviour
    {
#if ODIN_INSPECTOR
        [ShowInInspector]
#endif
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
