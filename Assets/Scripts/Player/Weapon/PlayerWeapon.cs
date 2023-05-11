using Enemy;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace Player.Weapon
{
    public class PlayerWeapon : MonoBehaviour
    {
        public PlayerController player;
        internal bool IsAttacking => player._isAttacking;
        public WeaponSetting weaponSO;
        public GameObject[] trails;
        public VisualEffect[] effects;
        public List<GameObject> hits = new();

        private void Awake()
        {
            player = player != null ? player : GetComponentInParent<PlayerController>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        public void ActivateVFX()
        {
            foreach (var item in effects)
            {
                item.Play();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(TagManager.Instance.EnemyTag) && IsAttacking)
            {
                if (!hits.Contains(other.gameObject))
                {
                    //Debug.Log($"Detected collision with:{other.gameObject.name}");
                    other.gameObject.GetComponentInParent<EnemyBase>().TakeDamage(player.playerData.DamageDealt(), player);
                    hits.Add(other.gameObject);
                }
            }
            else if (other.gameObject.CompareTag("Tree"))
            {
                Debug.Log("Tree");
            }
            else if (other.gameObject.CompareTag(TagManager.Instance.destructableTag) && IsAttacking)
            {
                Debug.Log("Why");
                other.gameObject.GetComponent<World.Destructable.DestructableBase>().TakeDamage(1);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, .25f);
        }

        public void OnActive()
        {
            foreach (var trail in trails)
            {
                trail.SetActive(true);
            }
        }

        public void OnHide()
        {
            foreach (var trail in trails)
            {
                trail.SetActive(false);
            }
        }
    }
}