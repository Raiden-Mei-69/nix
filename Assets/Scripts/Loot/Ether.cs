using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Rendering;
using UnityEngine;

namespace Loot
{
    public class Ether : MonoBehaviour
    {
        [SerializeField] private Collider _coll;
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private float speed=1f;
        private Player.PlayerController target;
        private bool reachedDestination = false;
        public int _number = 1;

        private void Awake()
        {
            _coll = _coll == null ? GetComponent<Collider>() : _coll;
            _coll.isTrigger = false;
        }

        public void Collect(Player.PlayerController target)
        {
            _rb.useGravity = false;
            _coll.isTrigger = true;
            this.target = target;
            reachedDestination = false;
            StartCoroutine(MoveToTarget());
        }

        private IEnumerator MoveToTarget()
        {
            do
            {
                transform.position = Vector3.Slerp(transform.position, target.transform.position, speed*Time.deltaTime);
                speed += Time.deltaTime;
                yield return null;
            } while (!reachedDestination);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(TagManager.Instance.PlayerTag))
            {
                Collected();
            }
        }

        private void Collected()
        {
            reachedDestination = true;
            target.playerData.inventory.AddEther(_number);
            Destroy(gameObject);
        }
    }
}
