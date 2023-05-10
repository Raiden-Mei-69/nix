using Unity.Entities;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Collections;
using Unity.Transforms;
using Unity.Burst;
using System.Diagnostics;
using System.Linq.Expressions;

namespace ECS.Loot
{
    public partial class LootDropECS : SystemBase
    {
        private EntityQuery lootDropQuery;
        private BeginSimulationEntityCommandBufferSystem beginSimulationEntity;
        private EntityQuery GameSettingsQuery;

        public Entity prefab;

        public LootDropECS()
        {

        }

        protected override void OnCreate()
        {
            lootDropQuery = GetEntityQuery(ComponentType.ReadWrite<LootDropData>());
            beginSimulationEntity = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
            GameSettingsQuery = GetEntityQuery(ComponentType.ReadWrite<GameSettingsComponent>());
            RequireForUpdate(GameSettingsQuery);
        }

        protected override void OnUpdate()
        {
            if (prefab == Entity.Null)
            {
                prefab = GetSingleton<LootDropData>().prefab;
                return;
            }

            var settings = GetSingleton<GameSettingsComponent>();
            var commandBuffer=beginSimulationEntity.CreateCommandBuffer();
            var count = lootDropQuery.CalculateChunkCountWithoutFiltering();
            var asteroidPrefab = prefab;
            var rand=new Unity.Mathematics.Random((uint)Stopwatch.GetTimestamp());

            Job.WithCode(() =>
            {
                for (int i = 0; i < settings.numAsteroids; i++)
                {
                    var padding = .1f;
                    // we are going to have the asteroids start on the perimeter of the level
                    // choose the x, y, z coordinate of perimeter
                    // so the x value must be from negative levelWidth/2 to positive levelWidth/2 (within padding)
                    var xPosition = rand.NextFloat(-1f * ((settings.levelWidth) / 2 - padding), (settings.levelWidth) / 2 - padding);
                    // so the y value must be from negative levelHeight/2 to positive levelHeight/2 (within padding)
                    var yPosition = rand.NextFloat(-1f * ((settings.levelHeight) / 2 - padding), (settings.levelHeight) / 2 - padding);
                    // so the z value must be from negative levelDepth/2 to positive levelDepth/2 (within padding)
                    var zPosition = rand.NextFloat(-1f * ((settings.levelDepth) / 2 - padding), (settings.levelDepth) / 2 - padding);

                    var chooseFace = rand.NextFloat(0, 6);

                    //Based on what face was chosen, we x, y or z to a perimeter value
                    //(not important to learn ECS, just a way to make an interesting prespawned shape)
                    if (chooseFace < 1) { xPosition = -1 * ((settings.levelWidth) / 2 - padding); }
                    else if (chooseFace < 2) { xPosition = (settings.levelWidth) / 2 - padding; }
                    else if (chooseFace < 3) { yPosition = -1 * ((settings.levelHeight) / 2 - padding); }
                    else if (chooseFace < 4) { yPosition = (settings.levelHeight) / 2 - padding; }
                    else if (chooseFace < 5) { zPosition = -1 * ((settings.levelDepth) / 2 - padding); }
                    else if (chooseFace < 6) { zPosition = (settings.levelDepth) / 2 - padding; }

                    //we then create a new translation component with the randomly generated x, y, and z values                
                    var pos = new Translation { Value = new float3(xPosition, yPosition, zPosition) };

                    //on our command buffer we record creating an entity from our Asteroid prefab
                    var e = commandBuffer.Instantiate(asteroidPrefab);

                    //we then set the Translation component of the Asteroid prefab equal to our new translation component
                    commandBuffer.SetComponent(e, pos);
                }
            }).Schedule();

            beginSimulationEntity.AddJobHandleForProducer(Dependency);
        }
    }
}
