using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
