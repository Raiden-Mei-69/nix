using System.Collections.Generic;
using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Player.PlayerStat
{
    [CreateAssetMenu(menuName = "Player/CharacterStat", order = 0)]
    public class CharacterStat : ScriptableObject
    {
        public List<LevelStatIndicator> levelStats = new();
        public TextAsset file;

        public void Set(List<LevelStatIndicator> levelStats)
        {
            this.levelStats = levelStats;
        }
        //Now add a fact that it will read the file and add the value to the SO


    }

    [System.Serializable]
    public class CharacterStatHolder
    {
        public CharacterStat CharacterStat;
        public string pathStat;
    }

    [System.Serializable]
    public class LevelStatIndicator
    {
        public int Level = 1;
        public int MaxHealth = 100;
        public int BaseAttack = 100;
        public int BaseDef = 100;
        public float BaseCritRate = 5f;
        public float BaseCritDamage = 50f;
        public bool needAscension = false;
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(CharacterStat))]
    public class CharacterStatEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            CharacterStat stat = (CharacterStat)target;
            if (GUILayout.Button("Read"))
            {
                string data = stat.file.text;

                //=Newtonsoft.Json.JsonConvert.DeserializeObject<CharacterStat>(data);
                //var a=ScriptableObject.CreateInstance(typeof(CharacterStat));
                var a = Newtonsoft.Json.JsonConvert.DeserializeObject<CharacterStat>(data);
                stat.Set(a.levelStats);
            }
            EditorUtility.SetDirty(stat);
            base.OnInspectorGUI();
        }
    }
#endif
}
