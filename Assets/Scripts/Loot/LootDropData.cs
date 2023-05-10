using Unity.Entities;


namespace ECS.Loot
{
    [GenerateAuthoringComponent]
    public struct LootDropData : IComponentData
    {
        public Entity prefab;
    }

}
