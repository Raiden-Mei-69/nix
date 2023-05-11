using System.Linq;
using UnityEngine;

namespace Loot.Weapon
{
    public class LootWeapon : LootDrop
    {
        // Use this for initialization
        void Start()
        {

        }

        private void OnCollisionEnter(Collision collision)
        {
            if (TagManager.Instance.groundTags.Contains(collision.gameObject.tag))
            {
                m_rigibody.useGravity = false;
                m_rigibody.isKinematic = true;
                m_rigibody.constraints = RigidbodyConstraints.FreezeAll;
            }
        }
    }
}