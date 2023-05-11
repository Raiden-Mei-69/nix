using Loot;
using Newtonsoft.Json;
using Player.Data;
using Player.Magic;
using Player.Weapon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Player.Inventory
{
    [Serializable]
    public class PlayerInventory
    {
        [NonSerialized, JsonIgnore] public PlayerData data;
        [SerializeField] private int _etherAmout = 0;
        public int EtherAmout { get => _etherAmout; private set { _etherAmout = Mathf.Clamp(value, 0, int.MaxValue); } }
        public List<InventoryItemSlot> inventoryItemSlots = new();
        public PlayerPowerSlot UltimatePowerSlot;
        public List<InventoryWeaponCell> weaponCells = new();
        public void AddEther(int value)
        {
            EtherAmout += value;
        }

        public void RemoveEther(int value)
        {
            EtherAmout -= value;
        }

        public bool EnoughEther(int amount) => EtherAmout >= amount;

        public bool CanTakeItem() =>
            true;

        public void GatherItem(KeyValuePair<LootItemSO, int> kvp)
        {
            if (inventoryItemSlots.Exists((item) => item.Name == kvp.Key.Name))
            {
                InventoryItemSlot element = inventoryItemSlots.Find((item) => item.Name == kvp.Key.Name);
                element.Number = Mathf.Clamp(element.Number + kvp.Value, 0, element.stack);
            }
            else
            {
                inventoryItemSlots.Add(new(kvp.Key, kvp.Value, kvp.Key.iconPath));
            }
        }

        public void CollectWeaponLoot(WeaponSetting setting)
        {
            weaponCells.Add(new(setting));
        }

        public void ChangeUltimate(UltimateMagic ult)
        {
            this.UltimatePowerSlot = ult.ultimateData;
        }

        public void LoadInv()
        {
            List<AsyncOperationHandle> ops = new();
            for (int i = 0; i < inventoryItemSlots.Count(); i++)
            {
                var op = Addressables.LoadAssetAsync<LootItemSO>(inventoryItemSlots[i].path);
                inventoryItemSlots[i].itemSettings = op.WaitForCompletion();
                ops.Add(op);
            }
            CleanHandle(ops);
        }

        private async void CleanHandle(IEnumerable<AsyncOperationHandle> ops)
        {
            await Task.Yield();
        }

        public IEnumerable<InventoryWeaponCell> GenerateListWeapons()
        {
            List<InventoryWeaponCell> s = new();
            //s.Add(data.equipedWeapon.weaponSetting);
            var cell = weaponCells.Find((item) => item.equiped);
            cell.index = 0;
            s.Add(cell);
            int i = 1;
            foreach (var item in weaponCells)
            {
                if (!item.equiped)
                {
                    s.Add(item);
                    item.index = i;
                    i++;
                }
            }
            return s;
        }

        public void LoadWeapons()
        {
            List<AsyncOperationHandle> ops = new();
            foreach (var item in weaponCells)
            {
                var op = Addressables.LoadAssetAsync<WeaponSetting>(item.pathSO);
                item.weaponSetting = op.WaitForCompletion();
                ops.Add(op);
            }
            CleanHandle(ops);
        }
    }

    [Serializable]
    public class InventoryItemSlot
    {
        public LootItemSO itemSettings;
        public string Name;
        public int stack;
        public int Number = 0;
        public string path;
        public string iconPath;

        public InventoryItemSlot(LootItemSO itemSettings, int Number, string iconPath)
        {
            this.itemSettings = itemSettings;
            this.Number = Number;
            Name = itemSettings.Name;
            stack = itemSettings.MaxStack;
            path = itemSettings.path;
            this.iconPath = iconPath;
        }

        public LootItemSO GetSett() =>
            itemSettings;
    }

    [Serializable]
    public class InventoryWeaponCell
    {
        [NonSerialized] public WeaponSetting weaponSetting;
        public bool equiped;
        public string Name;
        [Range(1, 90)] public int Level;
        public WeaponType weaponType;
        public int weaponDamage;
        public float weaponCritChance;
        public float weaponCritDamage;
        public string pathGO;
        public string pathSO;
        [NonSerialized] public int index;

        public InventoryWeaponCell(WeaponSetting weaponSetting)
        {
            this.weaponSetting = weaponSetting;
            Name = weaponSetting.WeaponName;
            weaponType = weaponSetting.weaponType;
            weaponDamage = weaponSetting.WeaponDamage;
            weaponCritChance = weaponSetting.WeaponCritChance;
            weaponCritDamage = weaponSetting.WeaponCritDamage;
            pathGO = weaponSetting.pathGO;
            pathSO = weaponSetting.path;
        }

        public void Load(WeaponSetting weaponSetting)
        {
            this.weaponSetting = weaponSetting;
            Name = weaponSetting.WeaponName;
            weaponType = weaponSetting.weaponType;
            weaponDamage = weaponSetting.WeaponDamage;
            weaponCritChance = weaponSetting.WeaponCritChance;
            weaponCritDamage = weaponSetting.WeaponCritDamage;
            pathGO = weaponSetting.pathGO;
            pathSO = weaponSetting.path;
        }
    }
}
