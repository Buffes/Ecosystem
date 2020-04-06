using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Ecosystem.ECS.Grid;

namespace Ecosystem.ECS.Movement.Pathfinding
{
    /// <summary>
    /// Finds a path to the target position from a move command. Currently runs on a generated grid.
    /// Needs a reference to the grid world.
    /// </summary>
    public class PathfindingSystem : SystemBase
    {
        private const int MOVE_STRAIGHT_COST = 10;
        private const int MOVE_DIAGONAL_COST = 14; // Approximate sqrt(2) as an int

        EndSimulationEntityCommandBufferSystem m_EndSimulationEcbSystem;
        private WorldGridSystem worldGridSystem;

        protected override void OnCreate()
        {
            base.OnCreate();
            m_EndSimulationEcbSystem = World
                .GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
            worldGridSystem = World.GetOrCreateSystem<WorldGridSystem>();
        }

        protected override void OnUpdate()
        {
            var commandBuffer = m_EndSimulationEcbSystem.CreateCommandBuffer().ToConcurrent();
            var blockedCells = worldGridSystem.BlockedCells;
            var waterCells = worldGridSystem.WaterCells;
            var grid = worldGridSystem.Grid;
            double time = Time.ElapsedTime;
            Entities
                .WithReadOnly(blockedCells)
                .WithReadOnly(waterCells)
                .ForEach((Entity entity, int entityInQueryIndex,
                ref DynamicBuffer<PathElement> pathBuffer,
                ref DynamicBuffer<UnreachablePosition> unreachablePositionsBuffer,
                in MoveCommand moveCommand,
                in Translation translation) =>
            {
                bool canMoveOnLand = true;
                bool canMoveInWater = false;

                float3 target = moveCommand.Target;
                float reach = moveCommand.Reach;
                int maxTiles = moveCommand.MaxTiles;
                float3 position = translation.Value;    
                // Consume the command
                commandBuffer.RemoveComponent<MoveCommand>(entityInQueryIndex, entity);

                // Clear any existing path
                pathBuffer.Clear();
                // Offset the target by the reach
                float3 offsetTarget = target - reach * math.normalize(target - translation.Value);
                NativeList<int2> path  = FindPath(grid.GetGridPosition(position),
                    grid.GetGridPosition(offsetTarget), blockedCells, waterCells, grid,
                    canMoveOnLand, canMoveInWater, maxTiles);
                // Add path checkpoints

                for (int i = 0; i < path.Length - 1; i++)
                {
                    // Could be reduced to only put checkpoints at corners (ends of straight lines) instead of every grid cell.
                    pathBuffer.Add(new PathElement { Checkpoint = grid.GetWorldPosition(path[i]) });
                }
                pathBuffer.Add(new PathElement { Checkpoint = position }); // Start with the current position so that the path following can correctly stop the movement
                
                if (pathBuffer.Length <= 1)
                {
                    // Add to unreachable buffer
                    unreachablePositionsBuffer.Add(new UnreachablePosition 
                                                    { 
                                                        Position = grid.GetGridPosition(target),
                                                        Timestamp = time
                                                    });
                }
                
                path.Dispose();
            }).ScheduleParallel();

            m_EndSimulationEcbSystem.AddJobHandleForProducer(Dependency);
        }

        ///<summary>
        /// Path finding using a basic, unoptimized version of the A* algorithm. Uses only value types for Burst compatibility.
        ///</summary>
        private static NativeList<int2> FindPath(int2 startPosition, int2 targetPosition,
            NativeArray<bool> blockedCells, NativeArray<bool> waterCells, GridData grid,
            bool onLand, bool inWater, int maxTiles)
        {
            // Initialize neighbour offset array
            NativeArray<int2> neighbourOffsetArray = new NativeArray<int2>(8, Allocator.Temp);
            neighbourOffsetArray[0] = new int2(-1, 0); // Left
            neighbourOffsetArray[1] = new int2(+1, 0); // Right
            neighbourOffsetArray[2] = new int2(0, +1); // Up
            neighbourOffsetArray[3] = new int2(0, -1); // Down
            neighbourOffsetArray[4] = new int2(-1, -1); // Left Down
            neighbourOffsetArray[5] = new int2(-1, +1); // Left Up
            neighbourOffsetArray[6] = new int2(+1, -1); // Right Down
            neighbourOffsetArray[7] = new int2(+1, +1); // Right Up

            NativeList<PathNode> pathNodes = new NativeList<PathNode>(Allocator.Temp); // Add pathNodes to this lazily as needed
            int index = 0;
            NativeHashMap<int2, int> indexOfPathNode = new NativeHashMap<int2, int>(grid.Length, Allocator.Temp);
            
            // openList Could be optimized by implementing a NativePriorityQueue and using that instead.
            NativeList<int> openList = new NativeList<int>(Allocator.Temp);
            NativeList<int> closedList = new NativeList<int>(Allocator.Temp);

            pathNodes.Add(MakePathNode(startPosition, ref index, ref indexOfPathNode, Heuristic(startPosition, targetPosition), 0));
            pathNodes.Add(MakePathNode(targetPosition, ref index, ref indexOfPathNode, Heuristic(targetPosition, targetPosition), int.MaxValue));
            openList.Add(pathNodes[0].index);
            int targetNodeIndex = -1;
            indexOfPathNode.TryGetValue(new int2(targetPosition.x, targetPosition.y), out targetNodeIndex);

            while (openList.Length > 0)
            {
                int currentNodeIndex = GetLowestFCostNodeIndex(openList, pathNodes);
                PathNode currentNode = pathNodes[currentNodeIndex];

                if (currentNodeIndex == targetNodeIndex)
                {
                    // Finished.
                    break;
                }

                if (pathNodes.Length >= maxTiles)
                {
                    // Reached maximum search range
                    break;
                }

                // Remove current node from Open List
                for (int i = 0; i < openList.Length; i++) {
                    if (openList[i] == currentNodeIndex) {
                        openList.RemoveAtSwapBack(i);
                        break;
                    }
                }

                closedList.Add(currentNodeIndex);

                for (int i = 0; i < neighbourOffsetArray.Length; i++) 
                {
                    int2 neighbourOffset = neighbourOffsetArray[i];
                    int2 neighbourPosition = new int2(currentNode.x + neighbourOffset.x, currentNode.y + neighbourOffset.y);

                    if (!WorldGridSystem.IsWalkable(grid, blockedCells, waterCells, onLand,
                        inWater, neighbourPosition))
                    {
                        // Not walkable
                        continue;
                    }
                    
                    int neighbourNodeIndex = -1;
                    if (!indexOfPathNode.TryGetValue(neighbourPosition, out neighbourNodeIndex))
                    {
                        // Need to create the PathNode
                        pathNodes.Add(MakePathNode(
                                    neighbourPosition, 
                                    ref index, 
                                    ref indexOfPathNode, 
                                    Heuristic(neighbourPosition, targetPosition), 
                                    int.MaxValue));
                        neighbourNodeIndex = indexOfPathNode[neighbourPosition];
                    }

                    PathNode neighbourNode = pathNodes[neighbourNodeIndex];
                    if (closedList.Contains(neighbourNodeIndex)) 
                    {
                        // Already searched this node
                        continue;
                    }

                    int2 currentNodePosition = new int2(currentNode.x, currentNode.y);

                    int tentativeGCost = currentNode.gCost + Heuristic(currentNodePosition, neighbourPosition);
                    if (tentativeGCost < neighbourNode.gCost)
                    {
                        // This path to neighbor is better than any previous one.
                        neighbourNode.cameFromNodeIndex = currentNodeIndex;
                        neighbourNode.gCost = tentativeGCost;
		                neighbourNode.CalculateFCost();
                        pathNodes[neighbourNodeIndex] = neighbourNode;

                        if (!openList.Contains(neighbourNode.index)) {
			                openList.Add(neighbourNode.index);
		                }
                    }
                }
            }
            
            PathNode targetNode = pathNodes[targetNodeIndex];

            openList.Dispose();
            closedList.Dispose();
            indexOfPathNode.Dispose();
            neighbourOffsetArray.Dispose();

            if (targetNode.cameFromNodeIndex == -1) {
                // Didn't find a path!
                pathNodes.Dispose();
                return new NativeList<int2>(Allocator.Temp);
            } else {
                // Found a path
                NativeList<int2> path = ConstructPath(pathNodes, targetNode);
                pathNodes.Dispose();
                return path;
            }
        }

        /// <summary>
        /// Provides the octile heuristic cost from a to b, disregarding non-walkable positions.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>the heuristic cost.</returns>
        private static int Heuristic(int2 a, int2 b) {
            int dx = math.abs(a.x - b.x);
            int dy = math.abs(a.y - b.y);
            
            // Disregards walls to give an estimated cost.
            return MOVE_DIAGONAL_COST * math.min(dx, dy) + MOVE_STRAIGHT_COST * math.abs(dx - dy);
;
        }

        private static PathNode MakePathNode(int2 position, ref int index, ref NativeHashMap<int2, int> indexOfPathNode, int hCost, int gCost)
        {
            PathNode node = new PathNode();
            node.x = position.x;
            node.y = position.y;
            node.index = index;
            indexOfPathNode.TryAdd(new int2(node.x, node.y), index);
            index++;
            node.gCost = gCost;
            node.hCost = hCost;
            node.CalculateFCost();
            node.cameFromNodeIndex = -1;

            return node;
        }

        /// <summary>
        /// Returns the index of the PathNode with the lowest F value. Would be unnecessary if a NativePriorityQueue existed.
        /// </summary>
        private static int GetLowestFCostNodeIndex(NativeList<int> openList, NativeList<PathNode> pathNodes) 
        {
            PathNode lowestCostPathNode = pathNodes[openList[0]];
            for (int i = 1; i < openList.Length; i++) 
            {
                PathNode testPathNode = pathNodes[openList[i]];
                if (testPathNode.fCost < lowestCostPathNode.fCost) 
                {
                    lowestCostPathNode = testPathNode;
                }
            }
            return lowestCostPathNode.index;
        }

        private static NativeList<int2> ConstructPath(NativeList<PathNode> pathNodes, PathNode targetNode)
        {
            if (targetNode.cameFromNodeIndex == -1) 
            {
                // Couldn't find a path.
                return new NativeList<int2>(Allocator.Temp);
            } else 
            {
                NativeList<int2> path = new NativeList<int2>(Allocator.Temp);
                path.Add(new int2(targetNode.x, targetNode.y));

                PathNode currentNode = targetNode;
                while (currentNode.cameFromNodeIndex != -1) {
                    PathNode cameFromNode = pathNodes[currentNode.cameFromNodeIndex];
                    path.Add(new int2(cameFromNode.x, cameFromNode.y));
                    currentNode = cameFromNode;
                }

                return path;
            }
        }

        private struct PathNode 
        {
            public int x;
            public int y;
            public int index;

            public int gCost;
            public int hCost;
            public int fCost;
            public int cameFromNodeIndex;

            public void CalculateFCost() {
                fCost = gCost + hCost;
            }
        }
    }
}
