using System;
using System.Collections;
using UnityEngine;

namespace Farm.Plant
{
    public class Plant : MonoBehaviour
    {
        public PlantInfo info;




        IEnumerator GrowPlant()
        {
            yield return null;
        }
    }

    [Serializable]
    public struct PlantInfo
    {
        public string PlantName;
        public int CollectNumber;
        public int MaxStack;

        public Material PlantMaterialNotReady;
        public Material PlantMaterialReady;

        public bool _fullGrow;
        public int dayGrow;
        public bool canBeHarvest;
        public bool multipleHarvest;
        public int dayMultipleHarvest;
    }
}
