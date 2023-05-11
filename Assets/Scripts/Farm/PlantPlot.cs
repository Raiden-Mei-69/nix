using Farm.Plant;
using System;
using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Farm
{
    public class PlantPlot : MonoBehaviour
    {
        public bool ContainPlant = false;
        public Nullable<PlantInfo> plantInfo;
        public Renderer _rend;
        public Material[] _mats;
        public int dayCreation = 0;
        private void Awake()
        {
            _rend = GetComponent<Renderer>();
            _rend.enabled = false;
        }

        public void SetPlant(PlantInfo plant)
        {
            ContainPlant = true;
            dayCreation = Day.DayManager.instance.dayInfo.Day;
            plantInfo = plant;
            _mats = new Material[] { plant.PlantMaterialNotReady, plant.PlantMaterialReady };
            StartCoroutine(GrowPlant());
        }

        IEnumerator GrowPlant()
        {
            _rend.enabled = true;
            _rend.material = plantInfo.Value.PlantMaterialNotReady;
            yield return new WaitUntil(() => dayCreation + plantInfo.Value.dayGrow == Day.DayManager.instance.dayInfo.Day);
            yield return new WaitForSeconds(plantInfo.Value.dayGrow);
            _rend.material = plantInfo.Value.PlantMaterialReady;
        }

        public PlantInfo HarvestPlant()
        {
            ContainPlant = false;
            _rend.enabled = false;
            return plantInfo.Value;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(PlantPlot))]
    public class PlantPlotEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            PlantPlot plantPlot = (PlantPlot)target;
            base.OnInspectorGUI();
        }
    }
#endif
}
