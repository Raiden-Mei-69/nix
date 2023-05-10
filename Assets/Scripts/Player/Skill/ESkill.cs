namespace Player.Skill
{
    [System.Serializable]
    /// <summary>
    /// This class is used to define the action of a Skill
    /// what it does and how it does it
    /// </summary>
    public class ESkill : PlayerSkill
    {
        public SkillType skillType;
        public SkillEffectType skillEffectType;

        public override void UseSkill()
        {
            base.UseSkill();
            ActionSkill();
        }

        public virtual void ActionSkill()
        {

        }
    }

    public enum SkillType { Area, Self }
    public enum SkillEffectType { Buff, Area, Action }
    public enum SkillAreaOutput { Damage, Heal }

    public enum TargetAffected { Player, Enemy }

    [System.Serializable]
    public class AreaSkillData
    {
        public string skillName = "";
        public float duration = 5f;
        public float cooldownTime = 5f;
        public float pulseDelay = 1f;
        public SkillAreaOutput outputType;
        public int outputResult = 15;
        public TargetAffected targetAffected;
    }
}
