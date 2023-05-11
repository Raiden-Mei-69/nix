using Loot;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace Menu.Inventory
{
    public class InventorySlotCell : MonoBehaviour
    {
        public Image iconImage;
        public TextMeshProUGUI textQuantity;
        public LootItemSO lootItemSO;

        public void Init(Sprite icon, string quantity)
        {
            iconImage.sprite = icon;
            textQuantity.text = quantity;
        }

        public static InventorySlotCell OnCreate(Transform parent, LootItemSO lootItem)
        {
            var goCell = Addressables.InstantiateAsync(AddressablesPath.inventorySlotCellPath, parent);
            InventorySlotCell cell = goCell.WaitForCompletion().GetComponent<InventorySlotCell>();
            //Addressables.Release(goCell);
            cell.lootItemSO = lootItem;
            return cell;
        }
    }
}
