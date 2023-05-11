using System.Collections.Generic;
using UnityEngine;

namespace Farm.Plant
{
    public class PlantInfoList : MonoBehaviour
    {
        public static PlantInfoList Instance;
        public List<PlantInfo> plants = new();
        private void Awake()
        {
            if (Instance != null)
                Destroy(this);
            Instance = this;
        }
    }
}
