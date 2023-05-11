using UnityEngine;

namespace Player.Magic
{
    public class MagicBase : MonoBehaviour
    {
        public PlayerController player;
        public Transform target;
        public int MagicCost = 0;
        public int MagicDamage = 0;
        public float ProjSpeed = 1f;
        public float rotationSpeed = 1f;
        public void OnCreate(PlayerController player)
        {
            this.player = player;
            LaunchMagic();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        public virtual void LaunchMagic()
        {
            if (player.currentTarget != null)
            {
                target = player.currentTarget;
            }
        }
    }
}