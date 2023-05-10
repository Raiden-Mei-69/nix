using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

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
