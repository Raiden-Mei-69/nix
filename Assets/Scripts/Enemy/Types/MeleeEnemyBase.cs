using System.Collections;

namespace Enemy.Types
{
    public class MeleeEnemyBase : EnemyBase
    {
        public virtual IEnumerator LoopAction()
        {
            do
            {
                runningMethod.Invoke();
                idleMethod.Invoke();
                if (target == null && AtHome())
                {
                    //Debug.Log("Mimimimi!!!");
                }
                else
                {
                    if (target == null && !AtHome())
                    {
                        //Debug.Log("Moving to home!");
                        yield return StartCoroutine(MoveToHome());
                    }
                    else if (target != null && !CanAttack())
                    {
                        yield return StartCoroutine(MoveTowardTarget());
                    }
                    else if (target != null && CanAttack())
                    {
                        yield return StartCoroutine(DecideToAttack());
                    }
                }
                yield return null;
            } while (_alive);
        }
    }
}
