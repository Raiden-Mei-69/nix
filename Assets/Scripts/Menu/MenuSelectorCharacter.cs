using Player;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Menu.Character
{
    public class MenuSelectorCharacter : MonoBehaviour
    {
        public PlayerController player;
        public SMenuSection[] menuSections;
        private SMenuSection activeSection;
        private int indexTab;

        private void Start()
        {
            activeSection = menuSections.FirstOrDefault();
            activeSection.menuSectionScript.Enable();
            activeSection.m_object.SetActive(true);
        }

        private void OnEnable()
        {

            StartCoroutine(SmartUpdate());
        }

        private void ChangeUsingIndex(int i)
        {
            activeSection.menuSectionScript.Disable();
            activeSection.m_object.SetActive(false);
            activeSection = menuSections[i];
            activeSection.m_object.SetActive(true);
            activeSection.menuSectionScript.Enable();
        }

        public void ChangeSection(MenuSectionIndex section)
        {
            activeSection.menuSectionScript.Disable();
            activeSection.m_object.SetActive(false);
            var s = System.Array.Find(menuSections, (item) => item.menuSection == section);
            int i = System.Array.FindIndex(menuSections, (item) => item.menuSection == section);
            indexTab = i;
            activeSection = s;
            s.m_object.SetActive(true);
            activeSection.menuSectionScript.Enable();
        }

        private void TabLeft()
        {
            indexTab--;
            if (indexTab < 0)
            {
                indexTab = menuSections.Length - 1;
            }
            ChangeUsingIndex(indexTab);
        }

        private void TabRight()
        {
            indexTab++;
            if (indexTab == menuSections.Length)
            {
                indexTab = 0;
            }
            ChangeUsingIndex(indexTab);
        }

        private IEnumerator SmartUpdate()
        {
            do
            {
                if (Gamepad.current != null)
                {
                    if (Gamepad.current.leftShoulder.wasPressedThisFrame)
                    {
                        TabLeft();
                    }
                    else if (Gamepad.current.rightShoulder.wasPressedThisFrame)
                    {
                        TabRight();
                    }
                }
                yield return null;
            } while (gameObject.activeSelf);
        }
    }

    [System.Serializable]
    public struct SMenuSection
    {
        public MenuSectionIndex menuSection;
        public GameObject m_object;
        public MenuSection menuSectionScript;
    }
}