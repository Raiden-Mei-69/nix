using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Menu
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private Transform[] elementsHiding;
        [SerializeField] private Player.PlayerController player;
        private readonly Dictionary<Transform, bool> defaultValue = new();
        [SerializeField] TextInteractable[] subs;
        private int indexSelected;
        private TextInteractable selectText;
        private void Start()
        {
            selectText = subs.FirstOrDefault();
            selectText.ForceState(true);
            gameObject.SetActive(false);
        }

        public void ChangeState(bool state)
        {
            gameObject.SetActive(state);
            player.inMenu = state;
            if (state)
            {
                player.canMove = false;
                StartCoroutine(SmartUpdate());
                foreach (var element in elementsHiding)
                {
                    defaultValue.Add(element, element.gameObject.activeSelf);
                    element.gameObject.SetActive(!state);
                }
            }
            else
            {
                foreach (var kvp in defaultValue)
                {
                    kvp.Key.gameObject.SetActive(kvp.Value);
                }
                defaultValue.Clear();
                player.canMove = true;
                //player.OnPauseMenuClose();
            }
            GameManager.Instance.PauseGame(state);
        }

        public void ResumeGame()
        {
            ChangeState(false);
        }

        public void OptionMenu()
        {

        }

        public void Quit()
        {
            player.Quitting();
        }

        private void ScrollUp()
        {
            indexSelected--;
            if (indexSelected == -1)
                indexSelected = subs.Length - 1;
            Debug.Log($"Scroll up {indexSelected}");
            Selected();
        }
        private void ScrollDown()
        {
            indexSelected++;
            if (indexSelected == subs.Length)
                indexSelected = 0;
            Debug.Log($"Scroll down {indexSelected}");
            Selected();
        }

        private void Selected()
        {
            selectText.ForceState(false);
            selectText=subs[indexSelected];
            selectText.ForceState(true);
        } 

        private void OnClick()
        {
            selectText.methodEvent.Invoke();
        }

        private IEnumerator SmartUpdate()
        {
            while (gameObject.activeSelf)
            {
                if(Keyboard.current != null)
                {
                    if (Keyboard.current.sKey.wasPressedThisFrame || Keyboard.current.downArrowKey.wasPressedThisFrame)
                    {
                        ScrollDown();
                    }
                    else if (Keyboard.current.wKey.wasPressedThisFrame || Keyboard.current.upArrowKey.wasPressedThisFrame)
                    {
                        ScrollUp();
                    }
                    else if(Keyboard.current.fKey.wasPressedThisFrame || Keyboard.current.enterKey.wasPressedThisFrame || Keyboard.current.numpadEnterKey.wasPressedThisFrame)
                    {
                        OnClick();
                    }
                }
                if(Gamepad.current != null)
                {
                    if (Gamepad.current.dpad.down.wasPressedThisFrame)
                    {
                        ScrollDown();
                    }
                    else if (Gamepad.current.dpad.up.wasPressedThisFrame)
                    {
                        ScrollUp();
                    }
                    else if (Gamepad.current.buttonSouth.wasPressedThisFrame)
                    {
                        OnClick();
                    }
                    else if (Gamepad.current.buttonEast.wasPressedThisFrame)
                    {
                        ResumeGame();
                    }
                }
                yield return null;
            }
            player.canMove = true;
        }
    }
}
