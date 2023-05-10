using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Animal
{
    public class AnimalBase : MonoBehaviour
    {
        public NavMeshAgent agent;
        public GameObject homePoint;
        public GameObject loot;
        public int numberLoot;

        public virtual void Die()
        {
            for (int i = 0; i < numberLoot; i++)
            {
                float angle = i * Mathf.PI * 2;
                float x = Mathf.Cos(angle);
                float z = Mathf.Sin(angle);
                Vector3 pos = transform.position + new Vector3(x, 0f, z);
                float angleDegree = -angle * Mathf.Rad2Deg;
                Quaternion rot = Quaternion.Euler(0, angleDegree, 0);
                Instantiate(loot, pos, rot,GameManager.Instance.lootHolder);
            }
            Destroy(gameObject);
        }
    }
}
