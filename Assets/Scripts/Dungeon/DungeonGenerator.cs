using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor; 
#endif

namespace Dungeon
{
    public class DungeonGenerator : MonoBehaviour
    {
        public Transform container;
        public GameObject baseRoom;

        public Room[] rooms;
        //All door that need to be fill
        public List<Transform> doors = new();
        public List<Room> roomSpawned = new();
        public List<Room> roomFilled = new();
        public List<Room> emptyRoom = new();

        public void Start()
        {
            CreateMainRoom();
        }

        public void Clean()
        {
            if (baseRoom == null)
                return;
            DestroyImmediate(baseRoom);
            baseRoom = null;
        }

        private void CreateMainRoom()
        {
            Room room = Instantiate(rooms.First(), container);
            GameObject baseRoom = room.gameObject;
            baseRoom.name = "Base Room";
            this.baseRoom = baseRoom;
            doors.AddRange(room.doors);
            //StartCoroutine(CreateSubsequentRoom(room));
        }

        private IEnumerator CreateSubsequentRoom(Room room)
        {
            yield return null;

            for (int i = 0; i < room.doors.Count(); i++)
            {
                GameObject nextRoom = Instantiate(rooms.First(), baseRoom.transform).gameObject;
                Room newOne = nextRoom.GetComponent<Room>();
                //newOne.transform.position = room.doors[i].position;
                newOne.PlaceRoom(room.doors[i].position);
                yield return null;
            }
        }

        public void AddRoom()
        {
            //if no door then exit
            if (doors.Count == 0)
                return;
            if (emptyRoom.Count == 0)
            {
                emptyRoom.Add(Instantiate(rooms.First(), baseRoom.transform));
            }
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(DungeonGenerator))]
    public class DungeonGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DungeonGenerator dungeon = (DungeonGenerator)target;
            if (GUILayout.Button("Generate"))
            {
                dungeon.Clean();
                dungeon.Start();
            }
            else if (GUILayout.Button("Clean"))
            {
                dungeon.Clean();
            }
            else if (GUILayout.Button("Add"))
            {
                dungeon.AddRoom();
            }
            base.OnInspectorGUI();
        }
    }
#endif
}
