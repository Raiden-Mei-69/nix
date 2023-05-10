using Loot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace Menu.Inventory
{
    public class CharacterInventoryDisplay : MonoBehaviour
    {
        public Player.PlayerController player;
        public int number=100;
        public Transform holder;
        public GridLayoutGroup grid;
        public List<InventorySlotCell> cells=new();
        private float delayClose = .15f;
        private bool canClose = false;

        private InventorySlotCell slotCellSelected;
        public Image IconSelected;
        public TextMeshProUGUI TitleText;
        public TextMeshProUGUI DescText;

        public void OnOpen(IEnumerable<Player.Inventory.InventoryItemSlot> slots)
        {
            canClose=false;
            string content = "";
            foreach(var slot in slots)
            {
                content += $"Name:{slot.Name} Number:{slot.Number}\n";
            }
            Debug.Log(content);
            List<KeyValuePair<string,KeyValuePair<string,LootItemSO>>> collSlots = new();
            foreach(var slot in slots)
            {
                collSlots.Add(new(slot.iconPath, new(slot.Number.ToString(),slot.itemSettings)));
            }
            KeyValuePair<string,string>[] array = new KeyValuePair<string,string>[number];
            Array.Fill<KeyValuePair<string, string>>(array, new(AddressablesPath.ui_inv_meat, "9999"));
            Debug.Log(array.Length);
            //OnActivate(array);
            OnActivate(collSlots);
            StartCoroutine(OnUpdate());
        }

        public void OnClose()
        {
            for (int i = 0; i < cells.Count; i++)
            {
                Destroy(cells[i].gameObject);
            }
            cells.Clear();
        }

        public void OnActivate(IEnumerable<KeyValuePair<string,KeyValuePair<string,LootItemSO>>> keyValuePairs)
        {
            List<AsyncOperationHandle> ops = new();
            foreach(KeyValuePair<string,KeyValuePair<string,LootItemSO>> keyValue in keyValuePairs)
            {
                var cell=InventorySlotCell.OnCreate(holder,keyValue.Value.Value);
                var srHandle = Addressables.LoadAssetAsync<Sprite>(keyValue.Key);
                ops.Add(srHandle);
                cell.Init(srHandle.WaitForCompletion(), keyValue.Value.Key);
                cells.Add(cell);
            }
            ReleaseAll(ops);
            StartCoroutine(DelayClose());
            SelectItem(cells.FirstOrDefault());
        }

        private IEnumerator DelayClose()
        {
            yield return new WaitForSeconds(delayClose);
            canClose = true;
        }

        private IEnumerator OnUpdate()
        {
            while (gameObject.activeSelf)
            {
                yield return null;
                if (Keyboard.current != null)
                {
                    if (Keyboard.current.bKey.wasPressedThisFrame&&canClose)
                    {
                        player.playerUI.ToggleInventory(player.inMenu=!player.inMenu);
                    }
                }
            }
        }

        private void OnEnable()
        {
            OnOpen(player.playerData.inventory.inventoryItemSlots);    
        }

        private void OnDisable()
        {
            OnClose();
        }

        private async void ReleaseAll(List<AsyncOperationHandle> ops)
        {
            for (int i = 0; i < ops.Count; i++)
            {
                Addressables.Release(ops[i]);
            }
            await Task.Yield();
        }

        public void SelectItem(InventorySlotCell cell)
        {
            slotCellSelected = cell;
            TitleText.text = cell.lootItemSO.Name;
            DescText.text = cell.lootItemSO.Description;
            IconSelected.sprite = cell.lootItemSO.Icon;
        }
    }
}
