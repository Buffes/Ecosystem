using UnityEngine;
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
    public class PathfindingSystem : SystemBase
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

        protected override void OnUpdate()
        {
            var commandBuffer = m_EndSimulationEcbSystem.CreateCommandBuffer().ToConcurrent();

            Entities.ForEach((Entity entity, int entityInQueryIndex,
                ref DynamicBuffer<PathElement> pathBuffer,
                in MoveCommand moveCommand,
                in Translation translation) =>
            {

                float3 target = moveCommand.target;
                float reach = moveCommand.reach;
                float3 position = translation.Value;    

                
                FindPath(new int2(0,0), new int2(6,7));
                // Consume the command
                commandBuffer.RemoveComponent<MoveCommand>(entityInQueryIndex, entity);
                // Clear any existing path
                pathBuffer.Clear();

                // Offset the target by the reach
                target = target - reach * math.normalize(target - translation.Value);

                // Add path checkpoint
                pathBuffer.Add(new PathElement { Checkpoint = target });

            }).ScheduleParallel();

            m_EndSimulationEcbSystem.AddJobHandleForProducer(Dependency);
        }

        ///<summary>
        /// Path finding using a basic, unoptimized version of the A* algorithm. Uses only value types for Burst compatibility.
        ///</summary>
        private static void FindPath(int2 startPosition, int2 targetPosition)
        {
            // Temporary representation of the grid, mapping coordinates to a boolean for walkability. 
            // false = not walkable, true = walkable. We assume equal weights for all positions in the grid for now.
            // TODO: Change this into reading from the grid when that is implemented. 
            int2 gridSize = new int2(100, 100);
            NativeHashMap<int2, bool> pathFindingMap = new NativeHashMap<int2, bool>(gridSize.x * gridSize.y, Allocator.Temp);
            
            for (int y = 0; y < gridSize.y; y++)
            {
                for (int x = 0; x < gridSize.x; x++)
                {
                    pathFindingMap.TryAdd(new int2(x, y), true);
                }
            }

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
            NativeHashMap<int2, int> indexOfPathNode = new NativeHashMap<int2, int>(gridSize.x * gridSize.y, Allocator.Temp);
            
            // Could be optimized by implementing a NativePriorityQueue and using that instead.
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
                    
                    if (!IsPositionInsideGrid(neighbourPosition, gridSize))
                    {
                        // Neighbour not a valid position
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

                    if (!pathFindingMap[neighbourPosition])
                    {
                        // Not walkable
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

            if (targetNode.cameFromNodeIndex == -1) {
                // Didn't find a path!
                Debug.Log("Didn't find a path!");
            } else {
                // Found a path
                NativeList<int2> path = ConstructPath(pathNodes, targetNode);
                
                for (int i = 0; i < path.Length; i++)
                {
                    Debug.Log(path[i]);   
                }
                
                path.Dispose(); // temporary
            }



            openList.Dispose();
            closedList.Dispose();
            indexOfPathNode.Dispose();
            pathNodes.Dispose();
            neighbourOffsetArray.Dispose();
            pathFindingMap.Dispose(); // Dispose the temporary map
        }

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

        private static bool IsPositionInsideGrid(int2 gridPosition, int2 gridSize) {
            return
                gridPosition.x >= 0 && 
                gridPosition.y >= 0 &&
                gridPosition.x < gridSize.x &&
                gridPosition.y < gridSize.y;
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
            public int cameFromNodeIndex;

            public void CalculateFCost() {
                fCost = gCost + hCost;
            }
        }
    }
}
