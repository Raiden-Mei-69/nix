using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.AddressableAssets;
using Loot;

namespace Menu.Inventory
{
    public class InventorySlotCell : MonoBehaviour
    {
        public Image iconImage;
        public TextMeshProUGUI textQuantity;
        public LootItemSO lootItemSO;

        public void Init(Sprite icon,string quantity)
        {
            iconImage.sprite = icon;
            textQuantity.text = quantity;
        }

        public static InventorySlotCell OnCreate(Transform parent,LootItemSO lootItem)
        {
            var goCell = Addressables.InstantiateAsync(AddressablesPath.inventorySlotCellPath,parent);
            InventorySlotCell cell = goCell.WaitForCompletion().GetComponent<InventorySlotCell>();
            //Addressables.Release(goCell);
            cell.lootItemSO = lootItem;
            return cell;
        }
    }
}
