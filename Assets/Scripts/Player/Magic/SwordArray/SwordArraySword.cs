using System.Collections;
using UnityEngine;

namespace Player.Magic
{
    public class SwordArraySword : MonoBehaviour
    {
        private PlayerController player;
        private SwordArray father;
        public Rigidbody rb;
        public float speed;
        public int damage;
        public Transform target;
        public float rotationSpeed;
        public float duration = 5f;
        public float arcFactor = .5f;
        public Vector3 angleChangingSpeed;
        private WaitForSeconds ws;
        public void OnCreate(int damage, Transform target, float speed, float rotationSpeed, float[] delay, SwordArray father, PlayerController player)
        {
            this.player = player;
            this.father = father;
            ws = new(Random.Range(delay[0], delay[1]));
            rb = rb != null ? rb : GetComponentInChildren<Rigidbody>();
            this.rotationSpeed = rotationSpeed;
            this.speed = speed;
            this.damage = damage;
            this.target = target;
            StartCoroutine(Move());
        }

        public IEnumerator Move()
        {
            yield return ws;
            do
            {
                Vector3 targetDir = target.position - transform.position;
                //transform.rotation = Quaternion.LookRotation(targetDir,Vector3.up);
                transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

                //Vector3 targetDir = (target.position - transform.position).normalized;
                //float rotationValue = Vector3.Cross(targetDir, transform.forward).z;
                //Vector3 crossProduct = Vector3.Cross(targetDir, transform.position);
                //float magnitude = crossProduct.magnitude;
                //rb.angularVelocity = crossProduct.normalized*speed*magnitude;
                //rb.velocity = transform.forward * speed;
                //Quaternion quaternion = Quaternion.LookRotation(targetDir);
                //transform.SetPositionAndRotation(Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime), Quaternion.RotateTowards(transform.rotation, quaternion, rotationSpeed*Time.deltaTime));
                yield return null;
            } while (gameObject != null && target != null);
            Destroy(gameObject);
        }

        Vector3 CalculateBezierPoint(float t, Vector3 startPosition, Vector3 endPosition, Vector3 controlPoint)
        {
            float u = 1 - t;
            float uu = u * u;

            Vector3 point = uu * startPosition;
            point += 2 * u * t * controlPoint;
            point += t * t * endPosition;

            return point;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(TagManager.Instance.EnemyTag))
            {
                other.gameObject.GetComponentInParent<Enemy.EnemyBase>().TakeDamage(damage, player);
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            father.OnDestroyChild();
        }
    }
}
