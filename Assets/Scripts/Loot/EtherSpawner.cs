using System.Collections;
using UnityEngine;

namespace Loot
{
    public class EtherSpawner : MonoBehaviour
    {
        public float delay = 5f;
        public int maxNumber = 15;
        public float radius = 15f;
        public GameObject Ether;

        private void Start()
        {
            StartCoroutine(Spawn());
        }

        IEnumerator Spawn()
        {
            do
            {
                for (int i = 0; i < maxNumber; i++)
                {
                    Vector3 pos = RandomCircle(transform.position, radius);
                    Quaternion rot = Quaternion.LookRotation(pos - transform.position);
                    Instantiate(Ether, pos, Quaternion.identity);
                }
                yield return new WaitForSeconds(delay);
            } while (true);
        }

        Vector3 RandomCircle(Vector3 center, float radius)
        {
            float ang = Random.value * 360;
            Vector3 pos;
            pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
            pos.y = center.y;
            pos.z = center.z + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
            return pos;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}
