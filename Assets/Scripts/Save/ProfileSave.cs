using UnityEngine;

namespace Save.Profile
{
    [CreateAssetMenu(fileName = "Settings\\Profile")]
    public class ProfileSave : ScriptableObject
    {
        public Vector2 camSensitivityMouse;
        public Vector2 camSensitivityController;

        public string Stringify() =>
            JsonUtility.ToJson(this, true);
    }
}
