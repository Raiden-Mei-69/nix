using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;


namespace Utility.Damage.UI
{
    public class DamagePopup : MonoBehaviour
    {
        public TextMeshPro textPro;
        public float time = 1f;

        public void OnCreate(string content,bool isCrit)
        {
            if (isCrit)
                content += "!";
            textPro.text = content;
            StartCoroutine(Rotate());
            Destroy(gameObject,1f);
        }

        private IEnumerator Rotate()
        {
            Camera target = Camera.main;
            do
            {
                transform.LookAt(target.transform.position);
                yield return null;
            } while (gameObject!=null);
        }

        public static DamagePopup Create(Vector3 position,Transform transform,string content,bool isCrit=false)
        {
            var op=Addressables.InstantiateAsync(AddressablesPath.damagePopup, position, Quaternion.identity);
            var sc=op.WaitForCompletion().GetComponent<DamagePopup>();
            //AddressablesPath.ClearOps(op);
            //sc.transform.position = position;
            //sc.rectTransform.position = position;
            sc.OnCreate(content,isCrit);
            return sc;
        }
    }
}
