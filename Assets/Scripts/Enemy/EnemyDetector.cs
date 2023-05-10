using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Enemy.Detector
{
    public class EnemyDetector : MonoBehaviour
    {
        private EnemyBase enemy;

        private void Awake()
        {
            enemy = GetComponentInParent<EnemyBase>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(TagManager.Instance.PlayerTag))
            {
                enemy.target = other.gameObject.GetComponent<Player.PlayerController>().TargetPoint;
                enemy.facingPlayer = true;
                enemy.StartCoroutine(enemy.FacePlayer());
                enemy.ShowUI();
            }    
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag(TagManager.Instance.PlayerTag))
            {
                enemy.target = null;
                enemy.facingPlayer = false;
                enemy.OnPlayerTooFar();
            }
        }
    }
}
