using UnityEngine; 

namespace Enemy.Attack
{
    public class EnemyAttacker : MonoBehaviour
    {
        [SerializeField] private EnemyBase enemy;
        public bool close = false;
        private void Awake()
        {
            enemy = enemy != null ? enemy : GetComponentInParent<EnemyBase>();
        }

        // Start is called before the first frame update
        private void Start()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(TagManager.Instance.PlayerTag))
            {
                close = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag(TagManager.Instance.PlayerTag))
            {
                close = false;
            }
        }
    }
}