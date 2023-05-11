using UnityEngine;
using Utility;

namespace Assets.Scripts.Enemy
{
    public class Targetable : MonoBehaviour, ITargetable
    {
        [SerializeField] private bool targetable = true;
        [SerializeField] private Transform targetTransform;


        Transform ITargetable.targetTranform { get => targetTransform; }

        bool ITargetable.targetable { get => targetable; }
    }
}
