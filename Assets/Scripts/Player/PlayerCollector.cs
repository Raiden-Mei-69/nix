using Animal.Aquatic;
using Loot;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Player.Loot
{
    public class PlayerCollector : MonoBehaviour
    {
        [SerializeField] private PlayerController playerController;
        [SerializeField] internal List<LootDrop> lootItems = new();
        [SerializeField] internal List<Chest> chests = new();
        [SerializeField] internal List<Fish> fishes = new();
        [SerializeField] internal List<LootDrop> lootWeapons = new();

        private void Awake()
        {
            playerController = GetComponentInParent<PlayerController>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        public void CollectableChange()
        {
            Clean();
            if (chests.Count > 0)
            {
                playerController.ShowInteract(true, chests.First().ChestName);
            }
            else if (fishes.Count > 0)
            {
                playerController.ShowInteract(true, "<color=red>Fish</color>");
            }
            else if (lootItems.Count > 0)
            {
                playerController.ShowInteract(true, lootItems.First().GetItems().Key.Name);
            }
            else if (lootWeapons.Count > 0)
            {
                playerController.ShowInteract(true, lootWeapons.First().GetWeapon().WeaponName);
            }
            else
            {
                playerController.ShowInteract(false);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(TagManager.Instance.etherTag))
            {
                other.gameObject.GetComponent<Ether>().Collect(playerController);
            }
            else if (other.gameObject.CompareTag(TagManager.Instance.lootTag))
            {
                var drop = other.gameObject.GetComponent<LootDrop>();
                //If this is an Item / Material
                if (drop._type == LootType.Item)
                {
                    lootItems.Add(drop);
                }
                //Else this is a Weapon
                else
                {
                    lootWeapons.Add(drop);
                }
            }
            else if (other.gameObject.CompareTag(TagManager.Instance.ChestTag))
            {
                chests.Add(other.gameObject.GetComponent<Chest>());
            }
            else if (other.gameObject.CompareTag(TagManager.Instance.fishTag))
            {
                fishes.Add(other.gameObject.GetComponent<Fish>());
            }
            CollectableChange();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag(TagManager.Instance.lootTag))
            {
                var drop = other.gameObject.GetComponent<LootDrop>();
                //If this is an Item / Material
                if (drop._type == LootType.Item)
                {
                    lootItems.Remove(drop);
                }
                //Else this is a Weapon
                else
                {
                    lootWeapons.Remove(drop);
                }
            }
            else if (other.gameObject.CompareTag(TagManager.Instance.ChestTag))
            {
                chests.Remove(other.gameObject.GetComponent<Chest>());
            }
            else if (other.gameObject.CompareTag(TagManager.Instance.fishTag))
            {
                fishes.Remove(other.gameObject.GetComponent<Fish>());
            }
            CollectableChange();
        }

        public void Clean()
        {
            lootItems.RemoveAll((item) => item == null);
            chests.RemoveAll((item) => item == null);
            fishes.RemoveAll((item) => item == null);
        }

        public void RemoveLoot(Chest chest = null, Fish fish = null, LootDrop item = null, LootDrop weapon = null)
        {
            if (chest != null)
                chests.Remove(chest);
            else if (fish != null)
                fishes.Remove(fish);
            else if (item != null)
                lootItems.Remove(item);
            else if (weapon != null)
                lootWeapons.Remove(weapon);
            CollectableChange();
        }
    }
}