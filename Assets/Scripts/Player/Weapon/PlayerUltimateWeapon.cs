using Enemy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Player.Ultimate
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerUltimateWeapon : MonoBehaviour
    {
        public PlayerController player;
        public PlayerUltimateWeaponHolder drawWeapon;
        public PlayerUltimateWeaponHolder sheathedWeapon;
        public float moveSpeedDuringUltimate = 1f;
        public Vector3 drawScale;
        public Vector3 sheathScale;

        private bool attacking = false;

        private void Start()
        {
            
        }

        public void OnStart()
        {
            transform.SetParent(drawWeapon.transform);
            transform.localRotation = Quaternion.Euler(drawWeapon.rotOffset);
            transform.localPosition = drawWeapon.posOffset;
            transform.localScale = drawScale;
            attacking = true;
        }

        public void OnStop()
        {
            transform.SetParent(sheathedWeapon.transform);
            transform.localRotation = Quaternion.Euler(sheathedWeapon.rotOffset);
            transform.localPosition=sheathedWeapon.posOffset;
            transform.localScale = sheathScale;
            attacking = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(TagManager.Instance.EnemyTag)&&attacking)
            {
                other.gameObject.GetComponentInParent<EnemyBase>().TakeDamage(player.playerData.DamageDealt(), player);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, .25f);
        }
    }
}
