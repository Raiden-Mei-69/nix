using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Enemy.Spawn
{
    public class EnemySpawnerHome : MonoBehaviour
    {
        public List<string> allPathEnemies = new();
        public List<EnemyBase> enemies=new();
        private Vector3[] poses;
        [SerializeField] private float respawnTime = 5f;

        private void Awake()
        {
            enemies = GetComponentsInChildren<EnemyBase>().ToList();
            poses = new Vector3[enemies.Count];
            for (int i = 0; i < enemies.Count; i++)
            {
                poses[i] = enemies[i].transform.position;
            }
        }

        // Use this for initialization
        private IEnumerator Start()
        {
            yield return null;
            StartCoroutine(CleanListOnDeath());
        }

        public async void Spawn()
        {
            List<AsyncOperationHandle> ops = new();
            for (int i = 0; i < allPathEnemies.Count; i++)
            {
                var op = Addressables.InstantiateAsync(allPathEnemies[i], poses[i], Quaternion.identity, transform);
                enemies.Add(op.WaitForCompletion().GetComponent<EnemyBase>());
                ops.Add(op);
            }
            await Task.Yield();
            StartCoroutine(CleanListOnDeath());
            AddressablesPath.ClearOps(ops);
        }

        private IEnumerator CleanListOnDeath()
        {
            Debug.Log("<color=green>Camp is filled</color>");
            yield return new WaitUntil(()=>enemies.Count==0);
            Debug.Log("<color=red>Camp is empty!</color>");
            yield return new WaitForSeconds(respawnTime);
            Spawn();
        }

        public void OnDeath(EnemyBase enemy)
        {
            enemies.Remove(enemy);
        }
    }

    [System.Serializable]
    public struct SpawnData
    {
        public string enemyPath;
        public EnemyBase enemy;
    }
}