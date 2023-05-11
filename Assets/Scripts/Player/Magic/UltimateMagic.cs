using System.Collections.Generic;
using UnityEngine;

namespace Player.Magic
{
    using Inventory;

    public class UltimateMagic : MonoBehaviour
    {
        public PlayerController player;
        public PlayerPowerSlot ultimateData;

        public ParticleSystem part;
        public List<ParticleCollisionEvent> collisionEvents = new();

        private void Awake()
        {
            part = part != null ? part : GetComponent<ParticleSystem>();
            player = player != null ? player : GetComponentInParent<PlayerController>();
        }

        public void Attack()
        {
            part.Emit(ultimateData.EmitNumber);
        }

        private void OnParticleCollision(GameObject other)
        {
            //Debug.Log($"Coll with:{other.name}");
            int num = part.GetCollisionEvents(other, collisionEvents);
            for (int i = 0; i < num; i++)
            {
                if (other.CompareTag("Untagged"))
                {
                    //Physics.IgnoreCollision(part,other.GetComponent<Collider>());   
                }
            }

            if (other.CompareTag(TagManager.Instance.EnemyTag))
            {
                other.GetComponentInParent<Enemy.EnemyBase>().TakeDamage(ultimateData.Damage, player);
            }
        }
    }
}
