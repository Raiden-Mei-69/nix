using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

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
