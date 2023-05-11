using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Collections;
using Unity.Burst;
using System.Linq;
using Loot.Item;
using Player.Weapon;

namespace Loot
{
    public class LootDrop : MonoBehaviour
    {
        public LootType _type;
        public float speed = 1;
        public Rigidbody m_rigibody;
        public Quaternion m_quaternion;

        [Header("Loot Setting")]
        [SerializeField] LootItemSO itemSO;
        [SerializeField] WeaponSetting weaponSO;

        public int Number;

        // Start is called before the first frame update
        void Start()
        {
            transform.rotation = Quaternion.identity;
            //transform.Rotate(m_quaternion.eulerAngles);
            transform.rotation = m_quaternion;
            m_rigibody = GetComponent<Rigidbody>();
        }

        public WeaponSetting GetWeapon()=>
            weaponSO;

        public KeyValuePair<LootItemSO, int> GetItems() =>
            new(itemSO, Number);

        private void OnCollisionEnter(Collision collision)
        {
            if (TagManager.Instance.groundTags.Contains(collision.gameObject.tag))
            {
                m_rigibody.useGravity = false;
                m_rigibody.isKinematic = true;
                m_rigibody.constraints = RigidbodyConstraints.FreezeAll;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, .2f);
        }
    }

    [System.Serializable]
    public struct LootInfo
    {
        public LootItemSO _itemSO;
        public int Number;
    }

    public enum LootType { Item, Weapon }
}