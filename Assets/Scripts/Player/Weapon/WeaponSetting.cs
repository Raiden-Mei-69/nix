using UnityEngine;

namespace Player.Weapon
{
    [CreateAssetMenu(fileName = "")]
    public class WeaponSetting : ScriptableObject
    {
        public string WeaponName;
        public WeaponType weaponType;
        public int WeaponDamage = 100;
        [Range(0, 100f)] public float WeaponCritChance = 5f;
        public float WeaponCritDamage = 50f;
        public int Level;
        public string WeaponDescription;
        public string path = "";
        public string pathGO = "";
        public Sprite icon;
    }

    public enum WeaponType { Greatsword, Sword, Spear, TwinBlade }
}