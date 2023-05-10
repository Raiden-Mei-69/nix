using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;

namespace Player.Inventory
{
    [Serializable]
    public class PlayerPowerSlot
    {
        public string PowerName="";
        public string PowerPath = "";

        public int Damage = 0;
        public int MagicCost = 0;
        public int EmitNumber = 25;

        public void OnStart()
        {

        }
    }
}
