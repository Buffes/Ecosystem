using Ecosystem.ECS.Movement.Pathfinding;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

/// <summary>
/// Removes UnreachablePosition elements from an entity's buffer based on its timestamp.
/// </summary>
public class RemoveOldUnreachableSystem : SystemBase
{
    private const double REMOVE_TIME = 60.0; // seconds

    protected override void OnUpdate()
    {
        double now = Time.ElapsedTime;

        Entities
        .WithoutBurst()
        .ForEach((Entity entity,
            ref DynamicBuffer<UnreachablePosition> unreachablePositions) =>
        {
            for (int i = 0; i < unreachablePositions.Length; i++)
            {
                double difference = now - unreachablePositions[i].Timestamp;
                if (difference > REMOVE_TIME)
                {
                    unreachablePositions.RemoveAt(i);
                }
            }
        }).Run();
    }
}
