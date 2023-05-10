using Enemy;
using System.Collections;
using UnityEngine;
using Utility;

namespace Player.Detector
{
    [RequireComponent(typeof(SphereCollider),typeof(Rigidbody))]
    public class PlayerTargetDetector : MonoBehaviour
    {
        [SerializeField] private PlayerController player;
        [SerializeField] private Collider _coll;
        [SerializeField] private Rigidbody _rigidbody;
        // Use this for initialization
        void Start()
        {
            _coll=_coll!=null?_coll:GetComponent<Collider>();
            _rigidbody=_rigidbody!=null?_rigidbody:GetComponent<Rigidbody>();
            _coll.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(TagManager.Instance.EnemyTag))
            {
                EnemyBase enemy = other.gameObject.GetComponentInParent<EnemyBase>();
                if (!player.PotentialTargets.Contains(enemy.targetTransformLocal))
                {
                    if(enemy.targetableLocal)
                        player.PotentialTargets.Add(enemy.targetTransformLocal);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag(TagManager.Instance.EnemyTag))
            {
                EnemyBase enemy = other.gameObject.GetComponentInParent<EnemyBase>();
                if (player.currentTarget == enemy.targetTransformLocal)
                {
                    player.currentTarget = null;
                    enemy.isTarget = false;
                }
                if (player.PotentialTargets.Contains(enemy.targetTransformLocal))
                {
                    player.PotentialTargets.Remove(enemy.targetTransformLocal);
                }
            }
        }
    }
}