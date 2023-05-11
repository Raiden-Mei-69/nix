using Player.Data;
using Player.Stat;
using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Player.UI
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] private PlayerController player;
        [SerializeField] private Canvas _canvas;
        [Space(10)]
        [SerializeField] private TextMeshProUGUI FPSCounter_Text;
        [SerializeField] private Slider _healthBar;
        [SerializeField] private Slider _magicBar;
        [SerializeField] private Slider _xpSlider;

        [Header("Character Tab")]
        public bool TabActive = false;
        [SerializeField] internal GameObject _characterTabHolder;
        [SerializeField] private TextMeshProUGUI _characterName;
        [SerializeField] private TextMeshProUGUI _characterLevel;
        [SerializeField] private Slider _characterXpSlider;
        [SerializeField] private TextMeshProUGUI _characterMaxHp;
        [SerializeField] private TextMeshProUGUI _characterAttack;
        [SerializeField] private TextMeshProUGUI _characterDef;
        [SerializeField] private TextMeshProUGUI _characterCritRate;
        [SerializeField] private TextMeshProUGUI _characterCritDamage;
        [SerializeField] private Camera _characterTabCamera;
        private readonly int display1 = 0;
        private readonly int display2 = 1;

        [Header("Inventory")]
        [SerializeField] internal Menu.Inventory.CharacterInventoryDisplay characterInventoryDisplay;


        [Header("Collectable")]
        public GameObject _collectableHolder;
        [SerializeField] private TextMeshProUGUI _collectableName;
        [SerializeField] private Image keyImg;

        // Use this for initialization
        void Start()
        {
            _collectableHolder.SetActive(false);
            StartCoroutine(ShowFPS());
            InitBar();
            _characterTabHolder.SetActive(false);
            _characterTabCamera.targetDisplay = display2;
            characterInventoryDisplay.gameObject.SetActive(false);
        }

        private IEnumerator ShowFPS()
        {
            float counter;
            do
            {
                counter = 1f / Time.unscaledDeltaTime;
                FPSCounter_Text.text = $"{counter.ToString("f2")} FPS";
                yield return new WaitForSeconds(.1f);
            } while (true);
        }

        private void InitBar()
        {
            _healthBar.maxValue = player.playerData.ModifiedMaxHealth;
            _healthBar.value = player.playerData.Health;

            _magicBar.maxValue = player.playerData.MaxMagic;
            _magicBar.value = player.playerData.Magic;

            _xpSlider.maxValue = PlayerXPCapstone.XpTable[player.playerData.Level];
            _xpSlider.value = player.playerData.XP;
        }

        public void UpdateBar()
        {
            _healthBar.value = player.playerData.Health;
            _magicBar.value = player.playerData.Magic;
            _xpSlider.maxValue = PlayerXPCapstone.XpTable[player.playerData.Level];
            _xpSlider.value = player.playerData.XP;
        }

        public void ShowCharacterDetail(bool state)
        {
            _characterTabHolder.SetActive(state);
            //_characterTabCamera.gameObject.SetActive(state);
            _characterTabCamera.targetDisplay = state ? display1 : display2;
            //Debug.Log(_characterTabCamera.targetDisplay);
            _canvas.enabled = !state;
            TabActive = state;
            Debug.Log(state);
            if (state)
            {
                PlayerData data = player.playerData;
                _characterName.text = data.CharacterName;
                _characterLevel.text = $"Level {data.Level} / 100";
                _characterXpSlider.maxValue = PlayerXPCapstone.XpTable[data.Level];
                _characterXpSlider.value = data.XP;
                int[] hp = SplitNumber(data.ModifiedMaxHealth);
                _characterMaxHp.text = $"{string.Join(",", hp)}";
                _characterAttack.text = $"{string.Join(",", SplitNumber(data.ModifiedAttack))}";
                _characterDef.text = $"{string.Join(",", SplitNumber(data.ModifiedDef))}";
                _characterCritRate.text = $"{data.ModifiedCritRate} %";
                _characterCritDamage.text = $"{data.ModifiedCritDamage} %";
                //Debug.Log($"Name:{data.CharacterName}\nLevel:{data.Level}/100\nHP:{string.Join(",",SplitNumber(data.MaxHealth))}\nAttack:{string.Join(",",SplitNumber(data.BaseAttack))}");
            }
        }

        public void ShowCollectable(bool state, string collName = "")
        {
            _collectableHolder.SetActive(state);
            if (!state)
                return;
            _collectableName.text = collName;
        }

        public void ToggleInventory(bool state)
        {
            characterInventoryDisplay.gameObject.SetActive(state);
            _canvas.enabled = !state;
            if (state)
            {
                characterInventoryDisplay.OnOpen(player.playerData.inventory.inventoryItemSlots);
            }
            else
            {
                characterInventoryDisplay.OnClose();
            }
        }

        private int[] SplitNumber(int value)
        {
            int length = (int)(1 + Math.Log(value, 1000));
            var result = from n in Enumerable.Range(1, length)
                         select ((int)(value / Math.Pow(1000, length - n))) % 1000;
            return result.ToArray();
        }
    }
}