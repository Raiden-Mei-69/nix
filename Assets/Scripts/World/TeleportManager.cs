using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace World
{
    public class TeleportManager : MonoBehaviour
    {
        public static TeleportManager Instance;
        public List<Teleporter> Teleporters = new();
        int indexTeleport = -1;
        private void Awake()
        {
            Instance = this;
            //get the Teleporter on the map
            Teleporters.AddRange(FindObjectsOfType<Teleporter>());
            //and then we clean the list of the duplicate
            Teleporters=new(Teleporters.Distinct());
        }

        public Vector3 GetTeleporter(bool next = true)
        {
            if (next)
            {
                indexTeleport++;
                if (indexTeleport == Teleporters.Count)
                {
                    indexTeleport = 0;
                }
            }
            else
            {
                --indexTeleport;
                if (indexTeleport < 0)
                {
                    indexTeleport=Teleporters.Count-1;
                }
            }
            Debug.Log(Teleporters[indexTeleport].gameObject.name);
            return Teleporters[indexTeleport].transform.position;
        }
    }
}
