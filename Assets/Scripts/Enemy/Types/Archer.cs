using System.Collections;

namespace Enemy.Types
{
    public class Archer : RangeEnemyBase
    {
        internal override void Start()
        {
            base.Start();
            StartCoroutine(LoopActionRange());
        }

        internal override IEnumerator Death()
        {
            yield return StartCoroutine(base.Death());
            yield return StartCoroutine(base.DropLoot());
            yield return StartCoroutine(base.DestroySelf());
        }
    }
}
