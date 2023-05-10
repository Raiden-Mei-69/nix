using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Player
{
    public interface ITarget 
    {
        public Transform TargetPoint { get; }
    }
}
