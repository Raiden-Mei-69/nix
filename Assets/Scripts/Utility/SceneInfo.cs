using System.Collections.Generic;

namespace Utility
{
    public static class SceneInfo
    {
        /// <summary>
        /// 0:is the scene
        /// 1: the screen
        /// </summary>
        public static string[] loadingInfo = new string[2] { "", "" };

        public static UnityEngine.ResourceManagement.ResourceProviders.SceneInstance? MainMenuSceneInstance;
        public static UnityEngine.ResourceManagement.ResourceProviders.SceneInstance? LoadingSceneInstance;

        public readonly static Dictionary<SceneEnum, int> SceneIndex = new Dictionary<SceneEnum, int>()
        {
            {SceneEnum.BaseScene,0 },
            {SceneEnum.MainMenu,1 },
            {SceneEnum.LoadingScene,2 },
            {SceneEnum.OpenWorldScene,3 }
        };

        public enum SceneEnum { BaseScene, MainMenu, LoadingScene, OpenWorldScene }
    }
}
