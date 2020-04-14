using Ecosystem.ECS.Physics;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;

/// <summary>
/// Stops physics objects from spinning (in all dimensions).
/// </summary>
public class FreezeRotationSystem : SystemBase
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

        Entities
            .WithNone<FrozenRotation>()
            .ForEach((Entity entity, int entityInQueryIndex, ref PhysicsMass mass) =>
        {

            mass.InverseInertia = float3.zero;
            commandBuffer.AddComponent<FrozenRotation>(entityInQueryIndex, entity);

        }).ScheduleParallel();

        m_EndSimulationEcbSystem.AddJobHandleForProducer(Dependency);
    }
}
