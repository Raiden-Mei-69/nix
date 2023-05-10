using Enemy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Enemy.Projectile
{
    [RequireComponent(typeof(Rigidbody))]
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rb;
        private EnemyBase enemy;

        public void OnSpawn(float speed,EnemyBase enemy)
        {
            this.enemy = enemy;
            Physics.IgnoreLayerCollision(gameObject.layer, enemy.gameObject.layer);
            _rb.AddForce(transform.forward * speed, ForceMode.Impulse);
            _rb.AddForce(Vector3.up * .5f);
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log($"Coll:{other.gameObject.name}");
            if (other.gameObject.CompareTag(TagManager.Instance.PlayerTag))
            {
                Debug.Log("<color=red>Ouch!</color>");
                other.gameObject.GetComponentInParent<Player.PlayerController>().TakeDamage(enemy.Damage);
            }
            Destroy(gameObject);
        }
    }
}
