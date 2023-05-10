using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Player.Weapon
{
    public class SheathingWeaponHolder : MonoBehaviour
    {
        public Vector3 weaponRotOffset;
        public Vector3 weaponPosOffset;

        public void OnSeathing(PlayerWeapon weapon)
        {
            weapon.transform.SetParent(transform);
            weapon.transform.localPosition = weaponPosOffset;
            weapon.transform.localRotation = Quaternion.Euler(weaponRotOffset);
            weapon.OnHide();
        }
    }
}
