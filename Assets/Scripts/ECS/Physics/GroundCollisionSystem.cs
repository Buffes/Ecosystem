using Ecosystem.ECS.Grid;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

/// <summary>
/// Stops everything from falling through the ground.
/// 
/// This system can use a height map to represent the ground level in the world or just use a
/// flat number for a flat world. Even a flat world could, for example, want to support lower
/// ground levels in water to allow fish to swim underneath the surface.
/// 
/// The system could also be replaced by adding a regular ground plane
/// in the editor.
/// </summary>
[UpdateInGroup(typeof(PhysicsSystemGroup))]
public class GroundCollisionSystem : SystemBase
{
    WorldGridSystem worldGridSystem;
    protected override void OnCreate()
    {
        worldGridSystem = World.GetOrCreateSystem<WorldGridSystem>();
    }

    protected override void OnUpdate()
    {
        var heightMap = worldGridSystem.HeightMap;
        var grid = worldGridSystem.Grid;

        Entities
            .WithReadOnly(heightMap)
            .WithAll<PhysicsMass>()
            .ForEach((ref Translation translation, ref PhysicsVelocity velocity) =>
            {
                float groundLevel = GetGroundLevel(translation.Value, heightMap, grid);

                if (translation.Value.y < groundLevel)
                {
                    translation.Value.y = groundLevel;
                    velocity.Linear.y = 0;
                }
            }).ScheduleParallel();
    }

    /// <summary>
    /// Returns the linearly interpolated height for a given position in the grid. Safe to use in systems.
    /// </summary>
    /// <returns></returns>
    public static float GetGroundLevel(float3 position, NativeArray<float> heightMap, GridData grid)
    {
        
        // Use linear interpolation to avoid staircase-like movement
        int2 position2D = grid.GetGridPosition(position);
        if(!grid.IsInBounds(position2D))
        {
            return 0;
        }

        float xU = position.x - position2D.x;
        float zU = (position.z - position2D.y);

        xU = xU / grid.CellSize;
        zU = zU / grid.CellSize;
        
        int2 bottomRight = new int2(position2D.x + 1, position2D.y);
        int2 topLeft = new int2(position2D.x, position2D.y + 1);
        int2 topRight = new int2(position2D.x + 1, position2D.y + 1);
        
        int2 origin = position2D;
        if (xU + zU > 1f && grid.IsInBounds(topRight))
        {
            // On second triangle in this cell. Invert and switch xU and zU
            origin = topRight;
            xU = 1f - zU;
            zU = 1f - xU; 
        }
        
        float xHeight;
        float zHeight;

        if (grid.IsInBounds(bottomRight))
        {
            xHeight = math.lerp(heightMap[grid.GetCellIndex(origin)], 
                                heightMap[grid.GetCellIndex(bottomRight)], xU);
        }
        else
        {
            xHeight = heightMap[grid.GetCellIndex(position)];
        }

        if (grid.IsInBounds(topLeft))
        {
            zHeight = math.lerp(heightMap[grid.GetCellIndex(origin)], 
                                heightMap[grid.GetCellIndex(topLeft)], zU);
        }
        else
        {
            zHeight = heightMap[grid.GetCellIndex(position)];
        }


        return (xHeight + zHeight) / 2f;
    }
}
