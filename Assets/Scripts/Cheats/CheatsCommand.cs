using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cheats.Command
{
    public static class CheatsCommand
    {
        public static string commandSave = "Save";



        public static string[] AllCommands;
        public static void InitializeCommandList()
        {
            var props = typeof(CheatsCommand).GetFields();
            UnityEngine.Debug.Log(props.Length);
            AllCommands=new string[props.Length-1];
            for (int i = 0; i < props.Length-1; i++)
            {
                AllCommands[i] = props[i].GetValue(typeof(CheatsCommand)).ToString();
            }
        }
    }
}
