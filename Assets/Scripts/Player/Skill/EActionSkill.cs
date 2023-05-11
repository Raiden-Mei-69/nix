using UnityEngine;

namespace Player.Skill
{
    public class EActionSkill : ESkill, ISerializationCallbackReceiver
    {
        public override void ActionSkill()
        {
            base.ActionSkill();
        }

        public void OnAfterDeserialize()
        {
            base.skillEffectType = SkillEffectType.Action;
        }

        public void OnBeforeSerialize()
        {
            base.skillEffectType = SkillEffectType.Action;
        }
    }
}