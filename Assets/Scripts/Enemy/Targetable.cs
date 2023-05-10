using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Utility;

namespace Assets.Scripts.Enemy
{
    public class Targetable : MonoBehaviour, ITargetable
    {
        [SerializeField] private bool targetable = true;
        [SerializeField] private Transform targetTransform;


        Transform ITargetable.targetTranform { get => targetTransform; }

        bool ITargetable.targetable {get=>targetable;}
    }
}
