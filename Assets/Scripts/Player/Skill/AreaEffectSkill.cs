using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Skill.Effect
{
    public class AreaEffectSkill : MonoBehaviour
    {
        public AreaSkill areaSkill;
        private AreaSkillData skillData;
        public List<PlayerController> players = new();
        public List<Enemy.EnemyBase> enemies = new();

        public void OnCreate(AreaSkill areaSkill,AreaSkillData skillData)
        {
            this.areaSkill = areaSkill;
            this.skillData = skillData;
            StartCoroutine(Active(this.skillData.duration));
            StartCoroutine(Action());
        }

        private IEnumerator Active(float duration)
        {
            yield return new WaitForSeconds(duration);
            Destroy(gameObject);
        }

        private IEnumerator Action()
        {
            while (gameObject.activeSelf)
            {
                Debug.Log($"<color=yellow>Skill {skillData.skillName} Active!</color>");
                DoAction();
                yield return new WaitForSeconds(skillData.pulseDelay);
            }
        }

        private void DoAction()
        {
            switch (skillData.outputType)
            {
                case SkillAreaOutput.Damage:
                    {
                        enemies.ForEach((item) =>
                        {
                            item.TakeDamage(areaSkill.Output(),areaSkill.player);
                        });
                    }
                    break;
                case SkillAreaOutput.Heal:
                    {
                        players.ForEach((item) =>
                        {
                            item.TakeDamage(-areaSkill.Output());
                        });
                    }
                    break;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            switch (skillData.targetAffected)
            {
                case TargetAffected.Player:
                    {
                        if (other.gameObject.CompareTag(TagManager.Instance.PlayerTag))
                        {
                            //Debug.Log($"{other.gameObject.name}");
                            players.Add(other.gameObject.GetComponent<PlayerController>());
                        }
                    }
                    break;
                case TargetAffected.Enemy:
                    {
                        if (other.gameObject.CompareTag(TagManager.Instance.EnemyTag))
                        {
                            enemies.Add(other.gameObject.GetComponentInParent<Enemy.EnemyBase>());
                        }
                    }
                    break;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            switch (skillData.targetAffected)
            {
                case TargetAffected.Player:
                    {
                        if (other.gameObject.CompareTag(TagManager.Instance.PlayerTag))
                        {
                            players.Remove(other.gameObject.GetComponentInParent<PlayerController>());
                        }
                    }
                    break;
                case TargetAffected.Enemy:
                    {
                        if (other.gameObject.CompareTag(TagManager.Instance.EnemyTag))
                        {
                            enemies.Remove(other.gameObject.GetComponentInParent<Enemy.EnemyBase>());
                        }
                    }
                    break;
            }
        }
    }
}