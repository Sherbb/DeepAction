using System;
using Sirenix.OdinInspector;

namespace DeepAction
{
    [Serializable]
    public class S_App
    {
        [ShowInInspector]
        public S_Game game { get; private set; } = new S_Game();
        [ShowInInspector]
        public S_UI ui { get; private set; } = new S_UI();
    }
}
