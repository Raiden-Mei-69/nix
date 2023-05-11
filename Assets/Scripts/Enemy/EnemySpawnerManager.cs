using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Enemy.Spawn
{
    public class EnemySpawnerManager : MonoBehaviour
    {
        public List<EnemySpawnerHome> spawners = new();

        // Execute before the Start
        private void Awake()
        {
            spawners = GetComponentsInChildren<EnemySpawnerHome>().ToList();
        }

        // Use this for initialization
        private void Start()
        {

        }
    }
}