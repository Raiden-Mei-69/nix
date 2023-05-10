using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Loot
{
    [Serializable]
    public class LootCell
    {
        public LootDrop loot;
        [Range(0.1f, 100f)] public float chanceSpawning = 5f;
        public int[] forkDrop = new int[2] { 1, 5 };
        public bool fixedDrop = false;

        public KeyValuePair<LootDrop, int> DropList()
        {
            int rnd = UnityEngine.Random.Range(0, 101);
            if (rnd <= chanceSpawning)
            {
                if (fixedDrop)
                {
                    return new(loot, forkDrop.First());
                }
                else
                {
                    return new(loot, UnityEngine.Random.Range(forkDrop.First(), forkDrop.Last() + 1));
                }
            }
            //so not in the chance so no drop
            return new(loot, 1);
        }
    }
}
