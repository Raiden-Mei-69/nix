using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Player.Weapon
{
    public class WeaponHolder : MonoBehaviour
    {
        public PlayerController player;
        public PlayerWeapon PlayerWeapon;
        public Vector3 rotOffset;
        public Vector3 posWeaponOffset;

        private void Awake()
        {
            player = player == null ? GetComponentInParent<PlayerController>() : player;
            PlayerWeapon = PlayerWeapon == null ? PlayerWeapon.GetComponentInChildren<PlayerWeapon>() : PlayerWeapon;
            //transform.rotation = Quaternion.identity;
        }

        public void OnWeaponChange(PlayerWeapon weapon)
        {
            PlayerWeapon = weapon;
            weapon.transform.localRotation = Quaternion.Euler(rotOffset);
        }

        public void DrawWeapon(PlayerWeapon weapon)
        {
            weapon.transform.SetParent(transform);
            weapon.transform.localPosition = posWeaponOffset;
            weapon.transform.localRotation = Quaternion.Euler(rotOffset);
            weapon.OnActive();
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(WeaponHolder))]
    public class WeaponHolderEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            WeaponHolder weaponHolder = (WeaponHolder)target;
            if (GUILayout.Button("A"))
            {
                //Vector3 rot = weaponHolder.PlayerWeapon.transform.rotation;
                weaponHolder.rotOffset = new Vector3(16.1106567f, 143.70369f, 303.722473f);
            }

            base.OnInspectorGUI();
        }
    }
#endif
}
