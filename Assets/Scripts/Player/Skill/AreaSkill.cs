using UnityEngine;

namespace Player.Skill
{
    using Effect;
    public class AreaSkill : ESkill, ISerializationCallbackReceiver
    {
        public AreaSkillData skillData;
        public AreaEffectSkill go;

        public override void ActionSkill()
        {
            base.ActionSkill();
            Instantiate(go, transform.position, Quaternion.identity).OnCreate(this,skillData);
        }

        public void OnAfterDeserialize()
        {
            base.skillEffectType = SkillEffectType.Area;
        }

        public void OnBeforeSerialize()
        {
            base.skillEffectType = SkillEffectType.Area;
        }
    }
}
