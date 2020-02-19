using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace Ecosystem.ECS.Movement.Pathfinding
{
    /// <summary>
    /// Finds a path to the target position from a move command. Currently just sets the path straight
    /// toward the target. Needs a reference to the grid world to actually pathfind.
    /// </summary>
    public class PathfindingSystem : JobComponentSystem
    {
        private const int MOVE_STRAIGHT_COST = 10;
        private const int MOVE_DIAGONAL_COST = 14; // Approximate sqrt(2) as an int

        EndSimulationEntityCommandBufferSystem m_EndSimulationEcbSystem;

        protected override void OnCreate()
        {
            base.OnCreate();
            m_EndSimulationEcbSystem = World
                .GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override JobHandle OnUpdate(JobHandle inputDependencies)
        {
            var commandBuffer = m_EndSimulationEcbSystem.CreateCommandBuffer().ToConcurrent();

            var jobHandle = Entities.ForEach((Entity entity, int entityInQueryIndex,
                ref DynamicBuffer<PathElement> pathBuffer,
                in MoveCommand moveCommand,
                in Translation translation) =>
            {

                float3 target = moveCommand.Target;
                float reach = moveCommand.Reach;

                // Consume the command
                commandBuffer.RemoveComponent<MoveCommand>(entityInQueryIndex, entity);
                // Clear any existing path
                pathBuffer.Clear();

                // Offset the target by the reach
                target = target - reach * math.normalize(target - translation.Value);

                // Add path checkpoint
                pathBuffer.Add(new PathElement { Checkpoint = target });

            }).Schedule(inputDependencies);

            m_EndSimulationEcbSystem.AddJobHandleForProducer(jobHandle);
            return jobHandle;
        }

        ///<summary>
        /// Path finding using a basic, unoptimized version of the A* algorithm. Uses solely value types for Burst compatibility.
        ///<summary>
        private void FindPath(int2 startPosition, int2 targetPosition)
        {
            // Temporary representation of the grid, mapping coordinates to an int for walkability. 
            // 0 = not walkable, 1 = walkable. We assume equal weights for all positions in the grid for now.
            // TODO: Change this into reading from the grid when that is implemented. 
            int2 gridSize = new int2(100, 100);
            NativeHashMap<int2, int> pathFindingMap = new NativeHashMap<int2, int>(gridSize.x * gridSize.y, Allocator.Temp);
            
            NativeList<PathNode> pathNodes = new NativeList<PathNode>(Allocator.Temp); // Add pathNodes to this lazily as needed
            int index = 0;

            NativeList<int> openList = new NativeList<int>(Allocator.Temp);
            NativeHashMap<int, int> closedList = new NativeHashMap<int, int>(Allocator.Temp);

            pathNodes.Add(MakePathNode(startPosition, ref index, Heuristic(startPosition, targetPosition), 0));

            openList.Add(pathNodes[0].index);

            while (openList.Length > 0)
            {
                int currentNode = GetLowestFCostNodeIndex(openList, pathNodes);

                if ()
            }

            openList.Dispose();
            closedList.Dispose();
            pathNodes.Dispose();
            pathFindingMap.Dispose(); // Dispose the temporary map
        }

        private int Heuristic(int2 a, int2 b) {
            int dx = math.abs(a.x - b.x);
            int dy = math.abs(a.y - b.y);
            
            // Disregards walls to give an estimated cost.
            return MOVE_DIAGONAL_COST * math.min(dx, dy) + MOVE_STRAIGHT_COST * math.abs(dx - dy);
;
        }

        private PathNode MakePathNode(int2 position, ref int index, int hCost, int gCost)
        {
            PathNode node = new PathNode();
            node.x = position.x;
            node.y = position.y;
            node.index = index;
            index++;
            node.gCost = gCost;
            node.hCost = hCost;
            node.CalculateFCost();
            node.cameFromIndex = -1;

            return node;
        }

        // adasd
        private int GetLowestFCostNodeIndex(NativeList<int> openList, NativeList<PathNode> pathNodes) 
        {
            PathNode lowestCostPathNode = pathNodes[openList[0]];
            for (int i = 1; i < openList.Length; i++) {
                PathNode testPathNode = pathNodes[openList[i]];
                if (testPathNode.fCost < lowestCostPathNode.fCost) {
                    lowestCostPathNode = testPathNode;
                }
            }
            return lowestCostPathNode.index;
        }

        private static int2 GetGridCoords(float3 worldPosition)
        {
            int x = (int)worldPosition.x - (worldPosition.x < 0 ? 1 : 0);
            int z = (int)worldPosition.z - (worldPosition.z < 0 ? 1 : 0);
            return new int2(x, z);
        }

        private static float3 GetWorldPosition(int2 gridCoords)
        {
            float x = gridCoords.x + 0.5f;
            float z = gridCoords.y + 0.5f;
            return new float3(x, 0f, z);
        }

        private struct PathNode 
        {
            public int x;
            public int y;
            public int index;

            public int gCost;
            public int hCost;
            public int fCost;
            public int cameFromIndex;

            public void CalculateFCost() {
                fCost = gCost + hCost;
            }
        }
    }
}
