using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using Utility;

namespace Menu.Loading
{
    public class LoadingScreen : MonoBehaviour
    {
        public Vector2Int ScreenSize;
        [SerializeField] private RawImage image;
        [SerializeField] private RenderTexture renderTexture;
        [SerializeField] private VideoPlayer videoPlayer;
        [SerializeField] private Slider progressBar;

        private void Awake()
        {
            //Addressables.UnloadSceneAsync(SceneInfo.MainMenuSceneInstance.Value);
            //OnCreate(SceneInfo.loadingInfo[1], SceneInfo.loadingInfo[0]);
            OnCreate(AddressablesPath.conduit_video, "");
        }

        private void Start()
        {
           // SceneManager.UnloadSceneAsync(SceneInfo.SceneIndex[SceneInfo.SceneEnum.MainMenu]);
        }

        public void OnCreate(string targetVideo,string targetScene)
        {
            ScreenSize = new(Screen.width,Screen.height);
            SetImageScale();
            //SetVideoLoading(targetVideo);
            //StartCoroutine(LoadSceneBackground(targetScene));
        }

        private void SetImageScale()
        {
            renderTexture = new(ScreenSize.x, ScreenSize.y, 24);
            videoPlayer.targetTexture = renderTexture;
            image.rectTransform.anchoredPosition = Vector3.zero;
            image.rectTransform.sizeDelta = ScreenSize;
            image.texture = renderTexture;
        }

        private void SetVideoLoading(string targetVideo)
        {
            var op = Addressables.LoadAssetAsync<VideoClip>(targetVideo);
            VideoClip clip = op.Result;
            videoPlayer.clip = clip;
            videoPlayer.Play();
            videoPlayer.isLooping = true;
            Addressables.Release(op);
        }

        private IEnumerator LoadSceneBackground(string targetScene)
        {
            yield return new WaitForSeconds(2f);
            var op = Addressables.LoadSceneAsync(targetScene, UnityEngine.SceneManagement.LoadSceneMode.Additive);
            while (op.PercentComplete<1)
            {
                progressBar.value = op.PercentComplete;
                yield return null;
            }
            image.gameObject.SetActive(false);
            Unload(op);
        }

        private void Unload(AsyncOperationHandle<SceneInstance> op)
        {
            Addressables.Release(op);
            SceneManager.UnloadSceneAsync(SceneInfo.SceneIndex[SceneInfo.SceneEnum.LoadingScene]);
            //Addressables.UnloadSceneAsync(SceneInfo.LoadingSceneInstance.Value,true);
        }
    }
}