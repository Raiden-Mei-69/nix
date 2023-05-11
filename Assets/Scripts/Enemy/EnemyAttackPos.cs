using System.Collections.Generic;
using UnityEngine;

namespace Enemy.Attack
{
    public class EnemyAttackPos : MonoBehaviour
    {
        [SerializeField] private EnemyBase enemy;
        Collider coll;
        internal List<Player.PlayerController> players = new();

        private void Awake()
        {
            enemy ??= GetComponentInParent<EnemyBase>();
            coll = coll != null ? coll : GetComponent<Collider>();
            coll.isTrigger = true;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        public void Attack()
        {
            foreach (var player in players)
            {
                player.TakeDamage(enemy.Damage);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(TagManager.Instance.PlayerTag) && enemy._attacking)
            {
                var player = other.GetComponentInParent<Player.PlayerController>();
                if (!players.Contains(player))
                {
                    Debug.Log($"<color=red>player:{other.gameObject.name}</color>");
                    players.Add(player);
                    player.TakeDamage(enemy.Damage);
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag(TagManager.Instance.PlayerTag) && enemy._attacking)
            {
                Debug.Log($"<color=red>player:{other.gameObject.name} with:{enemy.Damage}</color>");
                other.GetComponentInParent<Player.PlayerController>().TakeDamage(enemy.Damage);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag(TagManager.Instance.PlayerTag))
            {
                var player = other.GetComponentInParent<Player.PlayerController>();
                if (players.Contains(player))
                {
                    players.Remove(player);
                }
            }

        }
    }
}