using Player;
using Player.Weapon;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Menu.Inventory
{
    public class WeaponInventoryUI : MonoBehaviour
    {
        [SerializeField] PlayerController player;
        //[SerializeField] private List<Cell> cells = new();
        [SerializeField] Transform Content;
        [SerializeField] private WeaponSetting template;
        [SerializeField] private WeaponInventoryCellUI selected;
        [SerializeField] private WeaponInventoryCellUI equiped;
        //[SerializeField] private List<DictCell> dictCells = new();
        [SerializeField] private Vector2Int index;
        [SerializeField] private Vector2Int dimensionGrid;
        [SerializeField] private int columnCount = 4;
        private Dictionary<Vector2Int, Cell> cellMap = new();
        [SerializeField] private List<Cell> cellList = new();
        [SerializeField] private List<DictCell> dictCells = new();
        [SerializeField] private Cell cellSelected;
        private Vector2Int oldCellPos;
        private Vector2Int selCellPos;

        [Header("Selected Section")]
        [SerializeField] private TextMeshProUGUI _selectedWeaponName;
        [SerializeField] private TextMeshProUGUI _selectedBaseAttackValue;
        [SerializeField] private TextMeshProUGUI _selectedSubStatTitle;
        [SerializeField] private TextMeshProUGUI _selectedSubStatValue;
        [SerializeField] private TextMeshProUGUI _selectedLevel;
        [SerializeField] private TextMeshProUGUI _selectedWeaponDescription;
        [SerializeField] private Button _selectedButtonSwitch;

        [Header("Equiped Section")]
        [SerializeField] private TextMeshProUGUI _equipedWeaponName;
        [SerializeField] private TextMeshProUGUI _equipedBaseAttackValue;
        [SerializeField] private TextMeshProUGUI _equipedSubStatTitle;
        [SerializeField] private TextMeshProUGUI _equipedSubStatValue;
        [SerializeField] private TextMeshProUGUI _equipedLevel;
        [SerializeField] private TextMeshProUGUI _equipedWeaponDescription;

        // Use this for initialization
        void Start()
        {

        }

        private void OnEnable()
        {
            //show the inventory with player
            List<Player.Inventory.InventoryWeaponCell> sett = new(player.playerData.inventory.GenerateListWeapons());
            CreateCells(sett);
            index = Vector2Int.zero;
            StartCoroutine(SmartUpdate());
        }

        private void OnDisable()
        {
            //destroy them
            foreach(var kvp in cellMap)
            {
                Destroy(kvp.Value.weaponCell.gameObject);
            }
            //for (int i = 0; i < cells.Count; i++)
            //{
            //    Destroy(cells[i].weaponCell.gameObject);
            //}

            //cells.Clear();
            cellMap.Clear();
            //dictCells.Clear();
        }

        public void LoadWeapons()
        {

        }

        public void CreateCells(List<Player.Inventory.InventoryWeaponCell> settings)
        {
            int x = 0, y = 0;
            for(int i = 0; i < settings.Count; i++)
            {
                var c = WeaponInventoryCellUI.Create(Content, this, settings[i].weaponSetting);
                Cell cell = new(false, false, i, c,new(x,y));
                //cells.Add(cell);
                dictCells.Add(new(x, y, cell));
                cellList.Add(cell);
                cellMap.Add(new(x, y), cell);
                var m=cellMap[new(x, y)];
                m.index = i;
                cellMap[new(x, y)] = m;
                x++;
                if (x == columnCount)
                {
                    x = 0;
                    y++;
                }
            }
            //foreach (var item in settings)
            //{
            //}
            dimensionGrid = new(columnCount,y+1);
        }

        public void Select(WeaponInventoryCellUI cell,Vector2Int pos)
        {
            Cell cellSel = cellMap[pos];
            Cell oldCell = cellMap[oldCellPos];
            //var cellSel = cells.Find((item) => item.weaponCell == cell);
            if (selected != null)
            {
                //var s = cells.Find((item) => item.selected);
                oldCell.selected = false;
                oldCell.weaponCell.border.SetActive(false);
                oldCell.OnDeselect();
                cellMap[oldCellPos] = oldCell;
            }
            Debug.Log(pos);
            selected = cell;
            cellSel.selected = true;
            //cellSel.OnSelect();
            cellSel.weaponCell.border.SetActive(true);
            oldCellPos = pos;
            cellMap[pos] = cellSel;
            WeaponSetting set = cell.setting;
            _selectedWeaponName.text = set.WeaponName;
            _selectedBaseAttackValue.text = $"{set.WeaponDamage}";
            _selectedSubStatTitle.text = $"CRIT Rate";
            _selectedSubStatValue.text = $"{set.WeaponCritChance}%";
            _selectedLevel.text = $"Lv. {set.Level}/90";
            _selectedWeaponDescription.text = set.WeaponDescription;
        }

        /// <summary>
        /// Call when equiped from the menu
        /// </summary>
        public void OnEquip()
        {
            var cell=cellMap[index];
            cell.equiped = true;
            cellMap[selCellPos] = cell;
            //var s = cells.Find((item) => item.weaponCell == selected);
            equiped = selected;
            Debug.Log($"<color=red>{equiped.setting.WeaponName}</color>");
            Debug.Log($"<color=green>{cell.weaponCell.setting.WeaponName}</color>");
            Debug.Log($"<color=orange>{cell.index}</color>");
            WeaponSetting set = equiped.setting;
            _equipedWeaponName.text = set.WeaponName;
            _equipedBaseAttackValue.text = $"{set.WeaponDamage}";
            _equipedSubStatTitle.text = $"CRIT Rate";
            _equipedSubStatValue.text = $"{set.WeaponCritChance}%";
            _equipedLevel.text = $"Lv. {set.Level}/90";
            _equipedWeaponDescription.text = set.WeaponDescription;
            player.playerData.OnWeaponChanged(cell);
        }

        private void ScrollLeft()
        {
            index.x--;
            if (cellMap.ContainsKey(index))
            {
                index.x = index.x;
            }
            else
            {
                index.x++;
            }
            SelectByIndex();
        }

        private void ScrollRight()
        {
            index.x++;
            if (cellMap.ContainsKey(index))
            {
                index.x = index.x;
            }
            else
            {
                index.x--;
            }
            SelectByIndex();
        }

        private void ScrollDown()
        {
            index.y++;
            if (cellMap.ContainsKey(index))
            {
                index.y = index.y;
            }
            else
            {
                index.y--;
            }
            SelectByIndex();
        }

        private void ScrollUp()
        {
            index.y--;
            if (cellMap.ContainsKey(index))
            {
                index.y = index.y;
            }
            else
            {
                index.y++;
            }
            SelectByIndex();
        }

        private void SelectByIndex()
        {
            //Debug.Log(cellMap.Count);
            //if(cellMap.Count==dictCells.Count)
            Select(cellMap[index].weaponCell,index);
        }

        private IEnumerator SmartUpdate()
        {
            do
            {
                if (Gamepad.current != null)
                {
                    var dpad = Gamepad.current.dpad;
                    if (dpad.down.wasPressedThisFrame)
                    {
                        //Debug.Log("Down");
                        ScrollDown();
                    }
                    else if (dpad.up.wasPressedThisFrame)
                    {
                        //Debug.Log("Up");
                        ScrollUp();
                    }
                    else if (dpad.left.wasPressedThisFrame)
                    {
                        ScrollLeft();
                        //Debug.Log("Left");
                    }
                    else if (dpad.right.wasPressedThisFrame)
                    {
                        ScrollRight();
                        //Debug.Log("Right");
                    }
                    else if (Gamepad.current.buttonSouth.wasPressedThisFrame)
                    {
                        OnEquip();
                    }
                }
                yield return null;
            } while (gameObject.activeSelf);
        }
    }

    [System.Serializable]
    public struct Cell
    {
        public bool equiped;
        public bool selected;
        public int index;
        public WeaponInventoryCellUI weaponCell;

        public void OnSelect()
        {
            weaponCell.border.SetActive(selected);
        }

        public void OnDeselect()
        {
            //weaponCell.border.SetActive(false);
        }

        public Cell(bool equiped, bool selected, int index, WeaponInventoryCellUI weapon,Vector2Int pos)
        {
            this.equiped = equiped;
            this.selected = selected;
            this.index = index;
            this.weaponCell = weapon;
            this.weaponCell.pos = pos;
        }
    }

    [System.Serializable]
    public class DictCell
    {
        public int x;
        public int y;
        public Cell cell;

        public DictCell(int x, int y, Cell cell)
        {
            this.x = x;
            this.y = y;
            this.cell = cell;
        }
    }
}