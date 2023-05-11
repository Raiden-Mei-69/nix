using UnityEngine;

namespace Loot
{
    [CreateAssetMenu(fileName = "Loot/Item")]
    public class LootItemSO : ScriptableObject
    {
        public string path = "";
        public string Name = "";
        public string Description = "";
        public int MaxStack = 10;
        public Sprite Icon;
        public string iconPath = "";
        public string pathGO = "";
    }
}
