using System.Collections;
using System.Collections.Generic;

namespace Enemy.Types
{
    using Projectile;
    using UnityEngine;

    public class RangeEnemyBase : EnemyBase
    {
        public Bullet projectile;
        public Transform launchPoint;
        public float projectileSpeed = .25f;
        internal override IEnumerator Attack()
        {
            yield return null;
            Bullet bullet = Instantiate(projectile, launchPoint.position, transform.rotation);
            bullet.OnSpawn(projectileSpeed, this);
            yield return new WaitForSeconds(Random.Range(_minDelay, _maxDelay));
        }

        public virtual IEnumerator LoopActionRange()
        {
            do
            {
                if (target == null)
                {
                    //idle
                }
                else if (target != null)
                {
                    //Attack the target
                    yield return StartCoroutine(Attack());
                }
                yield return null;
            } while (_alive);
        }
    }
}
