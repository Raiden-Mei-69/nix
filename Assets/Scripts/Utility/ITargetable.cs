using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Utility
{
    public interface ITargetable
    {
        public bool targetable { get; }
        public Transform targetTranform { get; }
    }
}
