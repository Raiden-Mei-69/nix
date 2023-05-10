using System.Collections;
using UnityEngine;

namespace Player.Skill
{
    public class PlayerSkill : MonoBehaviour
    {
        public PlayerController player;
        public float cooldownTime = 5f;
        public float duration = 5f;
        private bool skillReady = true;

        private void Awake()
        {
            
        }

        private IEnumerator CooldownSkill()
        {
            skillReady = false;
            yield return new WaitForSeconds(cooldownTime);
            skillReady = true;
        }

        public virtual void UseSkill()
        {
            if (!skillReady)
                return;
            Debug.Log("Skill activated");
            //var go=Instantiate(skill, player.transform.position, Quaternion.identity);
            //Destroy(go, 5f);
            StartCoroutine(CooldownSkill());
        }

        public int Output() =>
            player.playerData.TalentOutput();
    }
}
