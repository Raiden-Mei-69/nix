using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Player.Data
{
    using Newtonsoft.Json;
    using PlayerStat;

    [Serializable]
    public class PlayerListCharacter
    {
        public List<CharacterDetail> characters = new(); 

        public string Stringify()
        {
            foreach (var item in characters)
            {
                item.SetData();
            }
            return JsonConvert.SerializeObject(characters,Formatting.Indented); 
            //return UnityEngine.JsonUtility.ToJson(characters,true);
        }
    }

    [Serializable]
    public class CharacterDetail
    {
        [JsonIgnore] public CharacterStat characterStat;
        public string CharacterName;
        public int Level = 1;
        public int MaxHealth = 100;
        public int Attack = 100;
        public int Def = 100;
        public float CritRate = 5f;
        public float CritDamage = 50f;
        public bool needAscension = false;

        public void SetData()
        {
            CharacterName = "Hello";
            Level = 1;
            LevelStatIndicator statLevel = characterStat.levelStats[Level - 1];
            MaxHealth = statLevel.MaxHealth;
            Attack = statLevel.BaseAttack;
            Def = statLevel.BaseDef;
            CritRate = statLevel.BaseCritRate;
            CritDamage = statLevel.BaseCritDamage;
            needAscension = statLevel.needAscension;
        }
    }

    [Serializable]
    public class ActiveCharacterDetail : CharacterDetail
    {

    }

    [Serializable]
    public class SelectedCharacterDetail : CharacterDetail
    {

    }
}
