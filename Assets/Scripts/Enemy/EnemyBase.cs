using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using Utility;

namespace Enemy
{
    using Detector;
    using Attack;
    using UI;
    using Unity.Transforms;
    using Loot;
    using UnityEngine.ResourceManagement.AsyncOperations;
    using UnityEngine.AddressableAssets;
    using Enemy.Spawn;
    using Unity.Services.Analytics;

    public class EnemyBase : MonoBehaviour, ITargetable
    {
        [Header("Component")]
        public EnemySpawnerHome home;
        [SerializeField] EnemyUI enemyUI;
        [SerializeField] internal Animator animator;
        [SerializeField] internal EnemyDetector enemyDetector;
        [SerializeField] internal EnemyAttacker enemyAttacker;
        [SerializeField] internal NavMeshAgent _agent;
        [SerializeField] internal Player.PlayerController player;
        [SerializeField] internal Transform target;
        [SerializeField] internal EnemyAttackPos attackPos;
        public Renderer[] renderers;
        public Collider[] colliders;
        public Transform bodyHolder;
        [Space(5)]

        [Header("Target")]
        public Transform targetTransformLocal;
        public bool targetableLocal = true;
        Transform ITargetable.targetTranform { get => targetTransformLocal; }
        bool ITargetable.targetable=>targetableLocal;

        [Space(20)]

        [SerializeField] internal string EnemyName = "";
        [SerializeField] internal int xpToGive = 10;
        private bool _isTarget = false;
        internal bool isTarget { get=>_isTarget; set { _isTarget = value; StartCoroutine(enemyUI.ShowingCanvasTarget()); } }
        internal bool _alive = true;
        private int _health = 100;
        [SerializeField] internal int MaxHealth = 100;
        public int Health { get => _health; set { _health = Mathf.Clamp(value, 0, MaxHealth); } }
        [SerializeField] private float _invicibilityDuration = .5f;
        [SerializeField] internal bool invincible = false;
        [SerializeField] internal bool _attacking = false;
        [SerializeField] internal int Damage = 5;

        [SerializeField] internal float _minDelay = .2f;
        [SerializeField] internal float _maxDelay=1f;
        [SerializeField] internal Collider _collider;
        internal bool _readyToDie = false;

        [Header("Loot")]
        public Vector3 offsetLootPos;
        public LootCell[] loots;
        [Space(20)]

        [SerializeField] internal Renderer SkinnedMeshRenderer;
        [SerializeField] internal bool facingPlayer = false;
        [SerializeField] internal float _delayBetweenAttack = 2f;
        [SerializeField] internal bool _canAttack = true;
        [SerializeField] internal float _moveSpeed=1f;
        private Vector3 point;

        [SerializeField] internal Transform[] _waypoints;
        public float tresholdDistance = .5f;
        [Header("Events")]
        public UnityEngine.Events.UnityEvent idleMethod;
        public UnityEngine.Events.UnityEvent runningMethod;
        private Utility.Damage.UI.DamagePopup _damagePopup;

        internal virtual void Start()
        {
            Health = MaxHealth;
            home ??= GetComponentInParent<EnemySpawnerHome>();
            enemyDetector = enemyDetector!=null?enemyDetector:GetComponentInChildren<EnemyDetector>();
            //SkinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
            attackPos ??= GetComponentInChildren<EnemyAttackPos>();
            _alive = true;
            _agent.speed = _moveSpeed;
            point = transform.position;
            enemyUI.OnStart();
        }

        public virtual void TakeDamage(int value,Player.PlayerController player)
        {
            this.player = this.player != null ? this.player : player;
            if (!invincible)
            {
                invincible = true; 
                StartCoroutine(Invincibility());
                Health -= value;
                enemyUI.UpdateLife();
                if (_damagePopup != null)
                    Destroy(_damagePopup.gameObject);
                _damagePopup=Utility.Damage.UI.DamagePopup.Create(bodyHolder.position,transform,value.ToString(),false);
            }
            if (Health == 0&&_alive)
            {
                _alive = false;
                StartCoroutine(Death());
            }
        }

        public void OnPlayerTooFar()
        {

        }

        private IEnumerator Invincibility()
        {
            invincible = true;
            yield return new WaitForSeconds(_invicibilityDuration);
            invincible = false;
        }

        internal virtual IEnumerator Death()
        {
            home.OnDeath(this);
            targetableLocal = false;
            player.GiveXP(xpToGive);
            _alive = false;
            yield return null;
        }

        internal IEnumerator DestroySelf()
        {
            foreach (var rend in renderers)
            {
                rend.enabled = false;
            }
            foreach (var coll in colliders)
            {
                coll.enabled = false;
            }
            enemyUI.canvas.enabled = false;
            yield return new WaitForSeconds(5f);
            Destroy(gameObject);
        }

        internal virtual IEnumerator Attack()
        {
            yield return null; 
        }

        internal IEnumerator DelayAttack()
        {
            yield return new WaitUntil(() => !_attacking);
            yield return new WaitForSeconds(UnityEngine.Random.Range(_minDelay, _maxDelay));
            //Debug.Log("Delay attack exiting");
        }

        internal virtual IEnumerator DropLoot()
        {
            List<AsyncOperationHandle> ops = new();
            foreach (var item in loots)
            {
                KeyValuePair<LootDrop, int> drop = item.DropList();
                //Debug.Log(drop.Value);
                if (drop.Value > 0)
                {
                    var loot=Addressables.InstantiateAsync(drop.Key.GetItems().Key.pathGO, new(transform.position.x+offsetLootPos.x, transform.position.y+offsetLootPos.y, transform.position.z+offsetLootPos.z), Quaternion.identity, GameManager.Instance.lootHolder);
                    loot.WaitForCompletion().GetComponent<LootDrop>().Number=drop.Value;
                    ops.Add(loot);
                }
            }
            yield return null;
            //AddressablesPath.ClearOps(ops);
        }

        internal virtual IEnumerator FacePlayer()
        {
            do
            {
                Vector3 dir = target.position - transform.position;
                dir.y = 0;
                var rot = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime*2f);
                yield return null;
            } while (facingPlayer);
        }

        public bool AtHome() =>
            Vector3.Distance(transform.position,point)<=tresholdDistance;

        public bool CanAttack() =>
            enemyAttacker.close;

        internal virtual IEnumerator DecideToAttack()
        {
            do
            {
                _attacking = true;
                yield return StartCoroutine(Attack());
                yield return new WaitForSeconds(_delayBetweenAttack);
                _attacking = false;
            } while (_alive&&CanAttack());
        }

        internal virtual IEnumerator MoveTowardTarget()
        {
            do
            {
                _agent.SetDestination(target.position);
                yield return null;
            } while (!_attacking&&_alive&&!CanAttack()&&target!=null);
        }

        internal virtual IEnumerator MoveToObjective()
        {
            if (_waypoints.Length > 0)
            {
                _agent.SetDestination(_waypoints[Random.Range(0, _waypoints.Length)].position);
                yield return new WaitUntil(() => _agent.remainingDistance<=_agent.stoppingDistance||target!=null);
                //Debug.Log("Path Pending");
            }
            yield return null;
        }

        internal virtual IEnumerator MoveToHome()
        {
            _agent.SetDestination(point);
            yield return null;
        }

        public void ShowUI()
        {
            StartCoroutine(enemyUI.ShowingCanvasTarget());
        }
    }
}
