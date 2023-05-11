using System;

namespace Player.Talents
{
    [Serializable]
    public class TalentsTable
    {
        public string TalentName;
        public TalentType talentType;
        public int Level;
        public StatScaling stat;
        public float scalingRatio;
    }

    public enum StatScaling { HP, DEF, ATK }
    public enum TalentType { Attack, Skill, Ultimate }
}
