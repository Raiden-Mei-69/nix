using Unity.Entities;
using Unity.Burst;
using Unity.Jobs;
using Unity.Scenes;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine.InputSystem;

namespace World.Scene
{
    public class SubSceneManager : ComponentSystem
    {
        //public SubScene SubScene;
        public SceneSystem SceneSystem;

        protected override void OnCreate()
        {
            //SceneSystem=World.GetOrCreateSystem<SceneSystem>();
        }

        protected override void OnUpdate()
        {
            if (Keyboard.current.digit0Key.wasPressedThisFrame)
            {
                //LoadSubScene(SubScene);
            }
        }

        public void LoadSubScene(SubScene subScene)
        {
            SceneSystem.LoadSceneAsync(subScene.SceneGUID);
        }
    }

    [System.Serializable]
    public struct SubSceneManagerComponent : IComponentData
    {
        //public SubScene SubScene1;
    }

    public partial class SubSceneManagerSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            
        }
    }
}
