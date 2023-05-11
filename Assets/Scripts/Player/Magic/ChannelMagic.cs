using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Magic
{
    public class ChannelMagic : MonoBehaviour
    {
        public PlayerController player;
        public int Damage = 5;
        public ParticleSystem part;
        public List<ParticleCollisionEvent> collisionEvents = new();
        public int emitNumber = 25;
        private void Start()
        {
            part = part != null ? part : GetComponent<ParticleSystem>();
        }

        private void FixedUpdate()
        {
            if (Keyboard.current.aKey.isPressed)
            {
                part.Emit(emitNumber);
            }
        }

        private void OnParticleCollision(GameObject other)
        {
            int num = part.GetCollisionEvents(other, collisionEvents);
            for (int i = 0; i < part.GetCollisionEvents(other, collisionEvents); i++)
            {

            }
            if (other.gameObject.CompareTag("Player"))
            {


            }
            else if (other.CompareTag(TagManager.Instance.EnemyTag))
            {
                other.GetComponent<Enemy.EnemyBase>().TakeDamage(Damage, player);
            }
        }
    }
}
