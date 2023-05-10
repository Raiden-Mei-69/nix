using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace World.Destructable.Nature
{
    public class Tree : DestructableBase
    {
        private void Start()
        {
            Health = maxHealth;
        }
    }
}