using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Farm.Plant;
using Utility;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Farm
{
    public class PlantPlotHolder : MonoBehaviour
    {
        public int numberFrameDelay = 3;
        public List<PlantPlot> plantPlots = new();
        int index = 0;
        public List<PlantPlot> harvestablePlantPlot = new();
        public Vector2 GridSize = new(6, 6);
        [SerializeField] private PlantPlot prefabPlot;

        private void Start()
        {
            ChangeGrid();
        }

        private void Update()
        {

        }

        public void ChangeGrid()
        {
            harvestablePlantPlot.Clear();
            plantPlots.ForEach((item) =>
            {
                Destroy(item);
            });
            plantPlots.Clear();

            for (int x = 0; x < GridSize.x; x++)
            {
                for (int y = 0; y < GridSize.y; y++)
                {
                    plantPlots.Add(Instantiate(prefabPlot, new(x + .5f, 0f, y + .5f), Quaternion.identity, transform));
                }
            }
        }

        public void Plant()
        {
            StartCoroutine(PlantStuff());
            //plantPlots[index].SetPlant(PlantInfoList.Instance.plants[UnityEngine.Random.Range(0,PlantInfoList.Instance.plants.Count)]);
            //index++;
            //if (index >= plantPlots.Count())
            //    index = 0;
        }

        IEnumerator PlantStuff()
        {
            List<PlantPlot> plants = plantPlots;
            plants.Shuffle();
            foreach (var item in plants)
            {
                if (!item.ContainPlant)
                {
                    item.SetPlant(PlantInfoList.Instance.plants[UnityEngine.Random.Range(0, PlantInfoList.Instance.plants.Count)]);
                    for (int i = 0; i < numberFrameDelay; i++)
                    {
                        yield return null;
                    }
                }
            }
            yield return null;
        }

        public void Harvest()
        {
            StartCoroutine(HarvestStuff());
        }

        IEnumerator HarvestStuff()
        {
            List<PlantPlot> plants = plantPlots;
            plants.Shuffle();
            foreach (var item in plantPlots)
            {
                if (item.ContainPlant)
                    harvestablePlantPlot.Add(item);
            }
            foreach (var item in plantPlots)
            {
                var collect = item.HarvestPlant();
                item.plantInfo = null;
                for (int i = 0; i < numberFrameDelay; i++)
                {
                    yield return null;
                }
            }
            yield return null;
            harvestablePlantPlot.Clear();
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(PlantPlotHolder))]
    public class PlantPlotHolderEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            PlantPlotHolder plantPlotHolder = (PlantPlotHolder)target;
            if (GUILayout.Button("GetData"))
            {
                plantPlotHolder.plantPlots = plantPlotHolder.GetComponentsInChildren<PlantPlot>().ToList();
            }
            base.OnInspectorGUI();
        }
    }
#endif
}
