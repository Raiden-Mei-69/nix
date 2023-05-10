using Enemy.Types;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class Bear : MeleeEnemyBase
    {
        
        #region Animation
        private int _deathID;
        private int _jumpID;
        private int _takeDamageID;
        private int _eatID;
        private int _sleepID;
        private int _sitID;
        private int _stunnedID;
        private int _buffID;
        private int _idleID;
        private int _attack1ID;
        private int _attack2ID;
        private int _attack3ID;
        private int _attack4ID;
        private int _runID;
        private int _combatIdleID;
        private int _comboID;

        private int[] _comboAttack;
        #endregion
        private bool _isSleeping=false;
        //public bool IsSleeping { get=>_isSleeping; set { _isSleeping = value;Sleeping(); } }



        // Start is called before the first frame update
        internal override void Start()
        {
            base.Start();
            GetAnimID();
            StartCoroutine(LoopAction());
            _isSleeping = true;
            //Sleeping();
        }

        private void GetAnimID()
        {
            _idleID = Animator.StringToHash("Idle");
            _combatIdleID = Animator.StringToHash("Combat Idle");
            _runID = Animator.StringToHash("Run Forward");
            _jumpID = Animator.StringToHash("Jump");
            _attack1ID = Animator.StringToHash("Attack1");
            _attack2ID = Animator.StringToHash("Attack2");
            _attack3ID = Animator.StringToHash("Attack3");
            _attack4ID = Animator.StringToHash("Attack5");
            _buffID = Animator.StringToHash("Buff");
            _takeDamageID = Animator.StringToHash("GetHitFront");
            _stunnedID = Animator.StringToHash("Stunned Loop");
            _eatID = Animator.StringToHash("Eat");
            _sitID = Animator.StringToHash("Sit");
            _sleepID = Animator.StringToHash("Sleep");
            _deathID = Animator.StringToHash("Death");
            _comboID = Animator.StringToHash("Combo");

            _comboAttack =new int[] { _attack1ID, _attack2ID, _attack3ID, _attack4ID };
        }

        public override void TakeDamage(int value, PlayerController player)
        {
            base.TakeDamage(value, player);
            if(!invincible)
                animator.SetTrigger(_takeDamageID);
        }

        IEnumerator TestAttack()
        {
            do
            {
                yield return StartCoroutine(Attack());
                yield return new WaitForSeconds(1.5f);
            } while (_alive);
        }

        internal override IEnumerator Attack()
        {
            int attackChain = Random.Range(1, 5);
            //Debug.Log("Chain attack");
            for (int i = 0; i < attackChain; i++)
            {
                animator.SetTrigger(_comboAttack[i]);
                yield return StartCoroutine(DelayAttack());
                attackPos.Attack();
                yield return null;
            }
            yield return new WaitForSeconds(_delayBetweenAttack);
        }

        internal override IEnumerator Death()
        {
            StartCoroutine(base.Death());
            animator.SetBool(_deathID, true);
            yield return new WaitUntil(() => _readyToDie);
            SkinnedMeshRenderer.enabled = false;
            yield return StartCoroutine(base.DropLoot());
            Destroy(gameObject);
        }

        private IEnumerator SleepingUpdate()
        {
            do
            {
                yield return null;
            } while (_isSleeping&&!CanAttack());
            Sleeping();
        }

        public void Sleeping()
        {
            _isSleeping = !CanAttack()&&AtHome()&&target==null;
            animator.SetBool(_sleepID, _isSleeping);
            StartCoroutine(SleepingUpdate());
        }

        public void WalkingAnim()
        {
            //Debug.Log("Walking");
            animator.SetBool(_runID, target != null && !CanAttack());
        }
    }
}