using Player.Data;
using Player.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerHolder : MonoBehaviour
    {
        public PlayerController playerController;
        
        public PlayerListCharacter ListCharacter;

        public void OnCharacterChange(string target)
        {

        }

        private void Update()
        {
            if (Keyboard.current.equalsKey.wasPressedThisFrame)
            {
                SaveList();
            }
        }

        public void SaveList()
        {
            Debug.Log($"<color=red>{Save.SaveManager.SaveFlut(ListCharacter.Stringify())}</color>");
        }
    }
}
