using System.Collections;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace World.Time
{

    public class DayNightCycleManager : MonoBehaviour
    {
        [Header("Component")]
        public Light m_light;
        public float ValueIntensity = 0;

        [Header("Light Intensity")]
        [SerializeField] internal float nightIntensity;
        [SerializeField] internal float dayIntensity;

        // Use this for initialization
        void Start()
        {
            StartCoroutine(ChangeTimeIntensity(nightIntensity));
        }

        internal void ChangeLight(float intensity)
        {
            m_light.intensity = intensity;
        }

        internal IEnumerator ChangeTimeIntensity(float intensity)
        {
            float value = m_light.intensity > intensity ? 1f : -0.1f;
            float timeToChange = m_light.intensity - intensity;
            while (m_light.intensity != intensity)
            {
                m_light.intensity += value;
                ValueIntensity = m_light.intensity;
                yield return null;
            }
            Debug.Log("Transition completed");
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(DayNightCycleManager))]
    public class DayNightCycleManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DayNightCycleManager manager = (DayNightCycleManager)target;
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Night"))
            {
                manager.ChangeLight(manager.nightIntensity);
            }
            else if (GUILayout.Button("Day"))
            {
                manager.ChangeLight(manager.dayIntensity);
            }
            GUILayout.EndHorizontal();
            //GUILayout.BeginHorizontal();
            //if(GUILayout.Button("Trans Night"))
            //{
            //    manager.StartCoroutine(manager.ChangeTimeIntensity(manager.nightIntensity));
            //}
            //else if(GUILayout.Button("Trans Day"))
            //{
            //    manager.StartCoroutine(manager.ChangeTimeIntensity(manager.dayIntensity));
            //}
            //GUILayout.EndHorizontal();
            base.OnInspectorGUI();
        }
    }
#endif
}