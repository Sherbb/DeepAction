using System;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace DeepAction
{
    [Serializable]
    public class S_App
    {
#if ODIN_INSPECTOR
        [ShowInInspector]
#endif
        public S_Game game { get; private set; } = new S_Game();
#if ODIN_INSPECTOR
        [ShowInInspector]
#endif
        public S_UI ui { get; private set; } = new S_UI();
    }
}
