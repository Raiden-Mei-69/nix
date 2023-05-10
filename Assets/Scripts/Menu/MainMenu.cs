using DiscordRich;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Utility;

namespace Menu
{
    public class MainMenu : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private TextMeshProUGUI txt;
        [SerializeField] private Slider progressBar;
        private bool canClick = false;
        private bool clicked = false;
        private IEnumerator Start()
        {
            progressBar.gameObject.SetActive(false);
            yield return new WaitUntil(() => DiscordRichManager.Instance.ready);
            canClick = true;
            txt.text = "Press to Play";
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (canClick)
            {
                //StartCoroutine(Loading());
                Play();
            }
        }

        private void Update()
        {
            if (clicked)
                return;
            if(Keyboard.current is not null)
            {
                if (Keyboard.current.anyKey.wasPressedThisFrame && canClick)
                {
                    Play();
                    clicked = true;
                }
            }
            if(Gamepad.current is not null)
            {
                //TODO: do the detection of multiple input
                //if (Gamepad.current.allControls.Count > 0)
                //    Play();
            }
        }

        private void Play()
        {
            txt.gameObject.SetActive(false);
            SceneInfo.loadingInfo = new string[2] { AddressablesPath.OpenWorldScene, AddressablesPath.conduit_video };
            SceneManager.LoadScene(SceneInfo.SceneIndex[SceneInfo.SceneEnum.LoadingScene]);
            //SceneInfo.LoadingSceneInstance = Addressables.LoadSceneAsync(AddressablesPath.LoadingScene, LoadSceneMode.Additive).WaitForCompletion();
        }
    }
}
