using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Utility
{
    public class BaseManager : MonoBehaviour
    {
        [SerializeField] private GameObject[] keepObjects;
        private void Awake()
        {
            foreach (var item in keepObjects)
            {
                DontDestroyOnLoad(item);
            }
        }

        private IEnumerator Start()
        {
            SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
            yield return new WaitForSeconds(.5f);
            SceneManager.UnloadSceneAsync(0);
        }
    }
}
