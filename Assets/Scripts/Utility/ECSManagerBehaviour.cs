using System.Collections;
using Unity.Entities;
using UnityEngine;

namespace Utility
{
    public class ECSManagerBehaviour : MonoBehaviour
    {
        public static ECSManagerBehaviour Instance;

        [Header("ECS")]
        public EntityManager entityManager;

        private void Awake()
        {
            Instance = this;
        }

        // Use this for initialization
        void Start()
        {
            
        }

        void CreateEntityManager()
        {
            entityManager=Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager;
        }
    }
}