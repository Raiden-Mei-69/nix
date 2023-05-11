using System;
using UnityEngine;

namespace Player.Data
{
    using Inventory;
    using Player.PlayerStat;
    using Player.Stat;
    using Player.Talents;
    using Player.Weapon;
    using UnityEngine.AddressableAssets;

    [Serializable]
    public class PlayerData
    {
        internal PlayerController player;
        public long id;
        public string CharacterName;
        [NonSerialized] public Stat.PlayerStat playerStat;
        public TalentsTable talentTable;
        public int XP;
        public int Level = 1;
        public Vector3 Position;
        public int _health = 100;
        public int MaxHealth = 100;
        public int Health { get => _health; set { _health = Mathf.Clamp(value, 0, ModifiedMaxHealth); } }
        public int MaxMagic = 100;
        public int _magic = 100;
        public int Magic { get => _magic; set { _magic = Mathf.Clamp(value, 0, MaxMagic); } }

        [Header("Base Stat")]
        public int BaseMaxHealth = 100;
        public int BaseAttack = 100;
        public int BaseDef = 10;
        public float CritRate = 5f;
        public float CritDamage = 50f;

        [Header("Modified Stat")]
        public int ModifiedMaxHealth = 100;
        public int ModifiedAttack = 10;
        public int ModifiedDef = 10;
        public float ModifiedCritRate;
        public float ModifiedCritDamage;

        public EquipedWeapon equipedWeapon;
        public PlayerInventory inventory;

        public bool Load(string json, PlayerController player)
        {
            this.player = player;
            inventory.data = this;
            if (Save.SaveManager.FileExist())
            {
                JsonUtility.FromJsonOverwrite(json, this);
                inventory.LoadInv();
                inventory.LoadWeapons();
                equipedWeapon.Load();
                ChangeWeapon();
                SetBaseStat();
                //TODO: load the weapon equiped
                //inventory.weaponCells.Load(equipedWeapon.weaponSetting);
                return true;
            }
            return false;
        }

        public string Stringify() =>
            JsonUtility.ToJson(this, true);

        public void AddXP(int amount)
        {
            XP += amount;
            ValidateLevel();
        }

        private void ValidateLevel()
        {
            int xpCap = PlayerXPCapstone.XpTable[Level];
            if (XP >= xpCap)
            {
                XP -= xpCap;
                Level++;
            }
        }

        public bool CanUseMagic(int cost) =>
            Magic >= cost;

        public int DamageDealt()
        {
            int dmg = ModifiedAttack;
            bool isCrit = UnityEngine.Random.Range(0, 101) <= ModifiedCritRate;
            if (isCrit)
                dmg += Mathf.RoundToInt(dmg * ModifiedCritDamage / 100);
            return dmg;
        }

        public void OnWeaponChanged(Menu.Inventory.Cell newWeapon)
        {
            InventoryWeaponCell newWeap = inventory.weaponCells.Find((item) => item.index == newWeapon.index);
            InventoryWeaponCell oldWeap = equipedWeapon;
            oldWeap.equiped = false;
            Debug.Log($"<color=yellow>{newWeap.Name}</color>");
            equipedWeapon = new(newWeap.weaponSetting);
            ChangeWeapon();
        }

        public void ChangeWeapon()
        {
            UnityEngine.Object.Destroy(player.playerWeapon.gameObject);
            var go = Addressables.InstantiateAsync(equipedWeapon.pathGO, player.WeaponHolder.transform).WaitForCompletion();
            go.transform.localScale = new(go.transform.localScale.x / player.scaleFactor.x, go.transform.localScale.x / player.scaleFactor.x, go.transform.localScale.x / player.scaleFactor.x);
            player.EquipWeapon(go.GetComponent<PlayerWeapon>());
        }

        public void SetBaseStat()
        {
            LevelStatIndicator stat = player.characterStat.levelStats[Level - 1];
            BaseMaxHealth = stat.MaxHealth;
            BaseAttack = stat.BaseAttack;
            BaseDef = stat.BaseDef;
            CritRate = stat.BaseCritRate;
            CritDamage = stat.BaseCritDamage;
            SetModifiedStat();
        }

        public void SetModifiedStat()
        {
            ModifiedAttack = player.playerWeapon.weaponSO.WeaponDamage + BaseAttack;
            ModifiedCritRate = player.playerWeapon.weaponSO.WeaponCritChance + CritRate;
            ModifiedCritDamage = player.playerWeapon.weaponSO.WeaponCritDamage + CritDamage;
            ModifiedMaxHealth = BaseMaxHealth;
            ModifiedDef = BaseDef;
        }

        public int TalentOutput()
        {
            switch (talentTable.stat)
            {
                case StatScaling.HP:
                    {
                        return Mathf.RoundToInt(ModifiedMaxHealth * talentTable.scalingRatio / 100f);
                    }
                case StatScaling.DEF:
                    {
                        return Mathf.RoundToInt(ModifiedDef * talentTable.scalingRatio / 100f);
                    }
                case StatScaling.ATK:
                    {
                        return Mathf.RoundToInt(ModifiedAttack * talentTable.scalingRatio / 100f);
                    }
            }
            return 0;
        }
    }

    [Serializable]
    public class EquipedWeapon : InventoryWeaponCell
    {
        public EquipedWeapon(WeaponSetting weaponSetting) : base(weaponSetting)
        {
        }

        public void Load()
        {
            var op = Addressables.LoadAssetAsync<WeaponSetting>(pathSO);
            weaponSetting = op.WaitForCompletion();
            Load(weaponSetting);
            equiped = true;
            AddressablesPath.ClearOps(op);
        }
    }
}
