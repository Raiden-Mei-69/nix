using Loot;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace World.Destructable
{
    public class DestructableBase : MonoBehaviour
    {
        public Loot.LootCell[] loots;
        public int maxHealth = 3;
        private int _health;
        public int Health { get => _health; set { _health = Mathf.Clamp(_health, 0, maxHealth); } }
        public Vector3 offsetLootPos;
        public Renderer[] rends;
        public Collider[] colls;
        private bool invincible = false;
        public float invincibilityTime = .5f;
        public float respawnTime = 5f;

        public virtual void TakeDamage(int value)
        {
            if (!invincible)
            {
                Health -= value;
                StartCoroutine(Invincibility());
            }
            if (Health == 0)
            {
                OnDeath();
            }
        }

        public virtual void DropLoot()
        {
            List<AsyncOperationHandle> ops = new();
            foreach (var item in loots)
            {
                KeyValuePair<LootDrop, int> drop = item.DropList();
                //Debug.Log(drop.Value);
                if (drop.Value > 0)
                {
                    var loot = Addressables.InstantiateAsync(drop.Key.GetItems().Key.pathGO, new(transform.position.x + offsetLootPos.x, transform.position.y + offsetLootPos.y, transform.position.z + offsetLootPos.z), Quaternion.identity, GameManager.Instance.lootHolder);
                    loot.WaitForCompletion().GetComponent<LootDrop>().Number = drop.Value;
                    ops.Add(loot);
                }
            }
            //AddressablesPath.ClearOps(ops);
        }

        public virtual void OnDeath()
        {
            Debug.Log("Tree Dead");
            ToggleElements(false);
            DropLoot();
            StartCoroutine(Respawn());
        }

        public virtual void OnSpawn()
        {
            Health = maxHealth;
            ToggleElements(true);
        }

        public virtual void ToggleElements(bool state)
        {
            foreach (var item in rends)
            {
                item.enabled = state;
            }
            foreach (var item in colls)
            {
                item.enabled = state;
            }
        }

        public virtual IEnumerator Invincibility()
        {
            invincible = true;
            yield return new WaitForSeconds(invincibilityTime);
            invincible = false;
        }

        public virtual IEnumerator Respawn()
        {
            yield return new WaitForSeconds(respawnTime);
            OnSpawn();
        }
    }
}
