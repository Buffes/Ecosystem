using UnityEngine;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

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
     
                // Consume the command
                //commandBuffer.RemoveComponent<MoveCommand>(entityInQueryIndex, entity);

                
                // Clear any existing path
                pathBuffer.Clear();
                // Offset the target by the reach
                target = target - reach * math.normalize(target - translation.Value);
                NativeList<int2> path  = FindPath(GetGridCoords(position), GetGridCoords(target));
                // Add path checkpoints

                for (int i = 0; i < path.Length; i++)
                {
                    // Could be reduced to only put checkpoints at corners (ends of straight lines) instead of every grid cell.
                    pathBuffer.Add(new PathElement { Checkpoint = GetWorldPosition(path[i]) });
                }
                path.Dispose();
            }).ScheduleParallel();

            m_EndSimulationEcbSystem.AddJobHandleForProducer(Dependency);
        }

        ///<summary>
        /// Path finding using a basic, unoptimized version of the A* algorithm. Uses only value types for Burst compatibility.
        ///</summary>
        private static NativeList<int2> FindPath(int2 startPosition, int2 targetPosition)
        {
            // Temporary representation of the grid, mapping coordinates to a boolean for walkability. 
            // false = not walkable, true = walkable. We assume equal weights for all positions in the grid for now.
            // TODO: Change this into reading from the grid when that is implemented. 
            //============================================================
            int2 gridSize = new int2(100, 100);
            NativeHashMap<int2, bool> pathFindingMap = new NativeHashMap<int2, bool>(gridSize.x * gridSize.y, Allocator.Temp);
            
            for (int y = 0; y < gridSize.y; y++)
            {
                for (int x = 0; x < gridSize.x; x++)
                {
                    pathFindingMap.TryAdd(new int2(x, y), true);
                }
            }
            //============================================================

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

            //NativeList<PathNode> pathNodes = new NativeList<PathNode>(Allocator.Temp); // Add pathNodes to this lazily as needed
            int index = 0;
            NativeHashMap<int2, PathNode> pathNodes = new NativeHashMap<int2, PathNode>(gridSize.x * gridSize.y, Allocator.Temp);
            
            // openList Could be optimized by implementing a NativePriorityQueue and using that instead.
            NativeList<int2> openList = new NativeList<int2>(Allocator.Temp);
            NativeList<int2> closedList = new NativeList<int2>(Allocator.Temp);

            pathNodes.Add(startPosition, MakePathNode(startPosition, ref index, Heuristic(startPosition, targetPosition), 0));
            pathNodes.Add(targetPosition, MakePathNode(targetPosition, ref index, Heuristic(targetPosition, targetPosition), int.MaxValue));
            openList.Add(pathNodes[0].index);
            // int targetNodeIndex = -1;
            // indexOfPathNode.TryGetValue(new int2(targetPosition.x, targetPosition.y), out targetNodeIndex);

            while (openList.Length > 0)
            {
                int2 currentPosition = GetLowestFCostNodeIndex(openList, pathNodes);
                PathNode currentNode = pathNodes[currentPosition];

                if (currentPosition.Equals(targetPosition))
                {
                    // Finished.
                    break;
                }

                // Remove current node from Open List
                for (int i = 0; i < openList.Length; i++) {
                    if (openList[i].Equals(currentPosition)) {
                        openList.RemoveAtSwapBack(i);
                        break;
                    }
                }

                closedList.Add(currentPosition);

                for (int i = 0; i < neighbourOffsetArray.Length; i++) 
                {
                    int2 neighbourOffset = neighbourOffsetArray[i];
                    int2 neighbourPosition = currentPosition + neighbourOffset;
                    //int2 neighbourPosition = new int2(currentNode.x + neighbourOffset.x, currentNode.y + neighbourOffset.y);
                    
                    if (!IsPositionInsideGrid(neighbourPosition, gridSize))
                    {
                        // Neighbour not a valid position
                        continue;
                    }
                    
                    if (!pathNodes.ContainsKey(neighbourPosition))
                    {
                        // Need to create the PathNode
                        pathNodes.Add(neighbourPosition, MakePathNode(
                                    neighbourPosition, 
                                    ref index, 
                                    Heuristic(neighbourPosition, targetPosition), 
                                    int.MaxValue));
                        //neighbourNodeIndex = indexOfPathNode[neighbourPosition];
                    }

                    PathNode neighbourNode = pathNodes[neighbourPosition];
                    if (closedList.Contains(neighbourPosition)) 
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
                        neighbourNode.cameFromPosition = currentPosition;
                        neighbourNode.gCost = tentativeGCost;
		                neighbourNode.CalculateFCost();
                        pathNodes[neighbourPosition] = neighbourNode;

                        if (!openList.Contains(neighbourPosition)) {
			                openList.Add(neighbourPosition);
		                }
                    }
                }
            }

            PathNode targetNode = pathNodes[targetPosition];

            openList.Dispose();
            closedList.Dispose();
            //indexOfPathNode.Dispose();
            neighbourOffsetArray.Dispose();
            pathFindingMap.Dispose(); // Dispose the temporary map

            if (targetNode.cameFromPosition.Equals(new int2(-1,-1))) {
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

        private static PathNode MakePathNode(int2 position, ref int index, int hCost, int gCost)
        {
            PathNode node = new PathNode();
            node.x = position.x;
            node.y = position.y;
            node.index = index;
            //indexOfPathNode.TryAdd(new int2(node.x, node.y), index);
            index++;
            node.gCost = gCost;
            node.hCost = hCost;
            node.CalculateFCost();
            node.cameFromPosition = new int2(-1, -1);

            return node;
        }

        /// <summary>
        /// Returns the index of the PathNode with the lowest F value. Would be unnecessary if a NativePriorityQueue existed.
        /// </summary>
        private static int2 GetLowestFCostNodeIndex(NativeList<int2> openList, NativeHashMap<int2, PathNode> pathNodes) 
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
            return new int2(lowestCostPathNode.x, lowestCostPathNode.y);
        }

        private static NativeList<int2> ConstructPath(NativeHashMap<int2, PathNode> pathNodes, PathNode targetNode)
        {
            if (targetNode.cameFromPosition.Equals(new int2(-1,-1))) 
            {
                // Couldn't find a path.
                return new NativeList<int2>(Allocator.Temp);
            } else 
            {
                NativeList<int2> path = new NativeList<int2>(Allocator.Temp);
                path.Add(new int2(targetNode.x, targetNode.y));

                PathNode currentNode = targetNode;
                while (!currentNode.cameFromPosition.Equals(new int2(-1,-1))) {
                    PathNode cameFromNode = pathNodes[currentNode.cameFromPosition];
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
            public int2 cameFromPosition;

            public void CalculateFCost() {
                fCost = gCost + hCost;
            }
        }
    }
}
