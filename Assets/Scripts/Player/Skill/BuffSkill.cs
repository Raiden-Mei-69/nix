using System.Collections;
using UnityEngine;

namespace Player.Skill
{
    public class BuffSkill : ESkill, ISerializationCallbackReceiver
    {
        public override void ActionSkill()
        {
            base.ActionSkill();
            StartCoroutine(CreateBuff());
        }

        public void OnAfterDeserialize()
        {
            base.skillEffectType = SkillEffectType.Buff;
        }

        public void OnBeforeSerialize()
        {
            base.skillEffectType = SkillEffectType.Buff;
        }

        private IEnumerator CreateBuff()
        {
            Debug.Log("Co");
            yield return null;
            Debug.Log("Co");
        }
    }
}
