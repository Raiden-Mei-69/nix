using Menu.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Menu
{
    public class MenuSection : MonoBehaviour, IPointerClickHandler, ISerializationCallbackReceiver
    {
        public MenuSectionIndex menuSectionIndex;
        public MenuSelectorCharacter MenuSelectorCharacter;
        public Image image;
        public static Color selectedColor;
        public Color _selectedColor;

        private void Awake()
        {
            image ??= GetComponent<Image>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            MenuSelectorCharacter.ChangeSection(menuSectionIndex);
        }

        public void Disable()
        {
            image.color = Color.white;
        }

        public void Enable()
        {
            image.color = selectedColor;
        }

        public void OnBeforeSerialize()
        {
            _selectedColor = selectedColor;
        }

        public void OnAfterDeserialize()
        {
            selectedColor = _selectedColor;
        }
    }

    public enum MenuSectionIndex{CharacterDetail,ItemInventory,WeaponInventory}
}
