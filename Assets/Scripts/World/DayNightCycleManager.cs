using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace World.Time
{

    public class DayNightCycleManager : MonoBehaviour
    {
        public WorldInfo WorldInfo;
        public bool doingCycle = false;
        [Header("Component")]
        public Light m_light;
        public float ValueIntensity = 0;
        private float __timeScape = 1;
        public float TimeScape { get => __timeScape; set { __timeScape = Mathf.Clamp(value, 1, 20);UnityEngine.Time.timeScale = TimeScape; } }

        // Use this for initialization
        void Start()
        {
            if(doingCycle)
            {
                StartCoroutine(DayNightCycle());
            }
        }

        internal void ChangeLight(float intensity)
        {
            m_light.intensity = intensity;
        }

        private IEnumerator DayNightCycle()
        {
            WorldInfo.currentTime = 0f;
            do
            {

                yield return new WaitForSeconds(WorldInfo.tick);
                var kvp = WorldInfo.AdvanceTime(WorldInfo.currentTimeDay);
                if (kvp.Key)
                {
                    WorldInfo.currentTimeDay = kvp.Value;
                }

            } while (gameObject.activeSelf);
        }

        internal IEnumerator ChangeTimeIntensity(float intensity)
        {
            yield return null;
            Debug.Log("Transition completed");
        }

        private void Update()
        {
            if (Keyboard.current.leftBracketKey.wasPressedThisFrame)
            {
                TimeScape--;
            }
            else if(Keyboard.current.rightBracketKey.wasPressedThisFrame)
            {
                TimeScape++;
            }
        }
    }
}