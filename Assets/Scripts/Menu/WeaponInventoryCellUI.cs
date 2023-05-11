using Player.Weapon;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;

namespace Menu.Inventory
{
    public class WeaponInventoryCellUI : MonoBehaviour, IPointerClickHandler
    {
        public WeaponInventoryUI weaponInventory;
        public WeaponSetting setting;
        public GameObject border;
        internal Vector2Int pos;

        public void OnPointerClick(PointerEventData eventData)
        {
            weaponInventory.Select(this, pos);
        }

        public static WeaponInventoryCellUI Create(Transform parent, WeaponInventoryUI weaponInventoryUI, WeaponSetting setting)
        {
            var cellOP = Addressables.InstantiateAsync(AddressablesPath.weaponInventoryCell, parent);
            var cell = cellOP.WaitForCompletion().GetComponent<WeaponInventoryCellUI>();
            cell.weaponInventory = weaponInventoryUI;
            cell.setting = setting;
            return cell;
        }
    }
}