using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;

/// <summary>
/// Stops physics objects from spinning (in all dimensions).
/// </summary>
public class FreezeRotationSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref PhysicsMass mass) =>
        {

            mass.InverseInertia = float3.zero;

        }).ScheduleParallel();
    }
}