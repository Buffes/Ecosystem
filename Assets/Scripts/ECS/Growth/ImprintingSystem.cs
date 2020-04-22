using Ecosystem.ECS.Animal;
using Ecosystem.ECS.Grid.Buckets;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Ecosystem.ECS.Growth
{
    /// <summary>
    /// Newborn animals imprint on the closest adult they see. Assuming it to be their mother.
    /// </summary>
    public class ImprintingSystem : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem m_EndSimulationEcbSystem;

        protected override void OnCreate()
        {
            m_EndSimulationEcbSystem = World
                .GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var commandBuffer = m_EndSimulationEcbSystem.CreateCommandBuffer().ToConcurrent();

            float deltaTime = Time.DeltaTime/60;

            var adultEntities = GetComponentDataFromEntity<Adult>(true);
            
            Entities
                .WithNone<Adult, ParentData>()
                .WithReadOnly(adultEntities)
                .ForEach((Entity entity, int entityInQueryIndex,
                ref Translation translation,
                in DynamicBuffer<BucketAnimalData> sensedAnimals) =>
                {
                    int closestAdultIndex = -1;
                    float closestAdultDistance = 0f;

                    for (int i = 0; i < sensedAnimals.Length; i++)
                    {
                        var sensedAnimalInfo = sensedAnimals[i];

                        float3 targetPosition = sensedAnimalInfo.Position;
                        float targetDistance = math.distance(targetPosition, translation.Value);
                        
                        if (closestAdultIndex != -1 && targetDistance >= closestAdultDistance) continue; // Not the closest
                        if (!adultEntities.Exists(sensedAnimalInfo.Entity)) continue; // Not an adult

                        closestAdultIndex = i;
                        closestAdultDistance = targetDistance;
                    }

                    if (closestAdultIndex != -1)
                    {
                        commandBuffer.AddComponent<ParentData>(entityInQueryIndex, entity, 
                                new ParentData { Entity = sensedAnimals[closestAdultIndex].Entity});
                    }
                    
                }).ScheduleParallel();

            m_EndSimulationEcbSystem.AddJobHandleForProducer(Dependency);
        }
    }
}