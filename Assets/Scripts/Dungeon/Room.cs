using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Dungeon
{
    public class Room : MonoBehaviour
    {
        public Transform[] doors;
        public List<Transform> freeDoor = new();
        private void Start()
        {
            
        }

        public bool PlaceRoom(Vector3 pos)
        {
            Transform door=freeDoor.First();
            Vector3 diff = door.position - transform.position;
            transform.position = pos + diff;
            freeDoor.Remove(door);
            return freeDoor.Count > 0;
        }
    }
}