using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Cheats
{
    public class CheatsConsole : MonoBehaviour
    {
        public Player.PlayerController player;
        [SerializeField] private TMP_InputField _inputField;
        bool activate = false;

        public void ChangeState()
        {
            activate = !activate;
            player.inMenu = activate;
            _inputField.gameObject.SetActive(activate);
            if (activate)
            {
                _inputField.ActivateInputField();
            }
            else
            {
                _inputField.DeactivateInputField();
            }
        }

        public void ForceClose()
        {
            activate = false;
            player.inMenu = false;
            _inputField.gameObject.SetActive(false);
            _inputField.DeactivateInputField();
        }

        public void EndEditConsole(string text)
        {
            _inputField.text = "";
        }

        public void SetText(string text)
        {
            _inputField.text = text;
        }
    }
}
