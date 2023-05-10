using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Loot
{
    public class Chest : MonoBehaviour
    {
        public string ChestName = "";
        [Space(10)]
        [Header("Components")]
        [SerializeField] private Animator _anim;
        [SerializeField] private Collider _coll;
        private int _triggerOpen;

        [Header("Stuff")]
        [SerializeField] private float _maxRangeDrop = 2f;
        [SerializeField] private int _etherAmount;
        public LootCell[] lootCells;
        [SerializeField] internal bool _canBeOpen = true;
        [SerializeField] internal bool _canBeLoot = true;
        [SerializeField] private AnimationClip openingClip;
        private float _delayDisapear = 5f;

        private void Awake()
        {
            _triggerOpen = Animator.StringToHash("Open");
        }

        // Use this for initialization
        private void Start()
        {

        }

        private IEnumerator OpeningAnimation()
        {
            //yield return StartCoroutine(Disapear());
            yield return null;

            _anim.SetTrigger(_triggerOpen);
            yield return new WaitForSeconds(openingClip.averageDuration);
            StartCoroutine(Disapear());
        }

        public void GetLoots()
        {
            StartCoroutine(OpeningAnimation());
            CreateLoot();
            //Destroy(gameObject);
        }

        private void CreateLoot()
        {
            //Create Ether Loot
            CreateEtherLoot();

            //Create the loot item
            List<string> stuffs = PopulateDrops();
            //string content = "";
            //foreach (var stuff in stuffs)
            //{
            //    content += $"{stuff}\n";
            //}
            //Debug.Log(content);
            int i = 0;
            foreach (string stuff in stuffs)
            {
                float angle = i * Mathf.PI * 2;
                float x = Mathf.Cos(angle);
                float z = Mathf.Sin(angle);
                Vector3 pos = transform.position + new Vector3(x, 0f, z);
                float angleDegree = -angle * Mathf.Rad2Deg;
                Quaternion rot = Quaternion.Euler(0, angleDegree, 0);
                Addressables.InstantiateAsync(stuff, new(pos.x, pos.y + 1f, pos.z), rot, GameManager.Instance.lootHolder).Completed += SpawnedItem;
                i++;
            }
        }

        private List<string> PopulateDrops()
        {
            List<string> paths = new();
            foreach (var cell in lootCells)
            {
                //Debug.Log($"<color=blue>{cell.DropList().Key.GetItems().Key.Name}</color>");
                paths.Add(cell.DropList().Key.GetItems().Key.pathGO);
            }
            return paths;
        }

        private void CreateEtherLoot()
        {
            float angle = Mathf.PI * 2;
            float x = Mathf.Cos(angle);
            float z = Mathf.Sin(angle);
            Vector3 pos = transform.position + new Vector3(x, 0f+1f, z);
            float angleDegree = -angle * Mathf.Rad2Deg;
            Quaternion rot = Quaternion.Euler(0, angleDegree, 0);
            Addressables.InstantiateAsync(AddressablesPath.EtherPath, pos, rot, GameManager.Instance.lootHolder).Completed += SpawnedEther;
        }

        private void SpawnAllItemsAround(IEnumerable<string> stuffs)
        {
            int i = 0;
            foreach (var stuff in stuffs)
            {
                float angle = i * Mathf.PI * 2;
                float x = Mathf.Cos(angle);
                float z = Mathf.Sin(angle);
                Vector3 pos = transform.position + new Vector3(x, 0f, z);
                float angleDegree = -angle * Mathf.Rad2Deg;
                Quaternion rot = Quaternion.Euler(0, angleDegree, 0);
                Addressables.InstantiateAsync(stuff, pos, rot, GameManager.Instance.lootHolder).Completed += SpawnedItem; ;
            }
        }

        private void SpawnedItem(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<GameObject> obj)
        {
            obj.WaitForCompletion().GetComponent<LootDrop>().Number = 2;
        }

        private void SpawnedEther(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<GameObject> obj)
        {
            obj.WaitForCompletion().GetComponent<Ether>()._number = _etherAmount;
        }

        private IEnumerator Disapear()
        {
            yield return new WaitForSeconds(_delayDisapear);
            Renderer[] rends = GetComponentsInChildren<Renderer>();
            do
            {
                foreach (Renderer rend in rends)
                {
                    rend.material.color = new Color(rend.material.color.r, rend.material.color.g, rend.material.color.b, rend.material.color.a - .1f);
                }
                yield return null;
            } while (rends.First().material.color.a > 0f);
            Destroy(gameObject);
        }

        private void OnDrawGizmosSelected()
        {
            //a little ray to show the face
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, transform.forward * 3f);

            //draw the sphere of the range of drop
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _maxRangeDrop);
        }
    }
}