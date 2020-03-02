using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class OOPpathfind : MonoBehaviour
{
    OOPathfinding pathfinding;
    List<PathNode> path;
    int currentIndex;
    Vector3 currentNode;

    void Start()
    {
        if (OOPathfinding.Instance == null) 
        {
            pathfinding = new OOPathfinding(100, 100);
        }
        else
        {
        pathfinding = OOPathfinding.Instance;
        }
        RandomizeNewTarget();
    }

    void Update()
    {
        if (currentIndex >= path.Count - 1) 
        {
            RandomizeNewTarget();
        }


        if (Vector3.Distance(transform.position, currentNode) < 0.1)
        {
            currentNode = GetWorldPosition(new int2(path[currentIndex].x, path[currentIndex].y));
            currentIndex++;
        }

        // Move
        Vector3 movementDirection = Vector3.Normalize(currentNode - transform.position);
        transform.Translate(movementDirection * 6f * Time.deltaTime);
        if (movementDirection != Vector3.zero)
        {
            transform.rotation = quaternion.LookRotation(movementDirection, math.up());
        }
    }

    private void RandomizeNewTarget()
    {
        int2 logicTarget = new int2(UnityEngine.Random.Range(1, 100), UnityEngine.Random.Range(1, 100));
        int2 logicStart = GetGridCoords(transform.position);
        path = pathfinding.FindPath(logicStart.x, logicStart.y, logicTarget.x, logicTarget.y);
        // Debug.Log("Start: " + logicStart);
        // Debug.Log("Target: " + logicTarget);
        // Debug.Log(path == null);
        currentNode = GetWorldPosition(new int2(path[0].x, path[0].y));
        currentIndex = 0;

        // for (int i = 0; i < path.Count - 2; i++)
        // {
        //     PathNode node = path[i];
        //     PathNode next = path[i + 1];
            
        //     Debug.DrawLine(new Vector3(node.x, 0, node.y), new Vector3(next.x, 0, next.y), Color.red, 20f);
        // }
        // Debug.DrawLine(new Vector3(path[0].x, 0, path[0].y), new Vector3(path[1].x, 0, path[1].y), Color.blue, 20f);

        // Debug.Log("new target is: (" + path[path.Count - 1].x + ", " + path[path.Count - 1].y + ")");
    }

    private static Vector3 GetWorldPosition(int2 gridCoords)
    {
        float x = gridCoords.x + 0.5f;
        float z = gridCoords.y + 0.5f;
        return new Vector3(x, 0f, z);
    }
   
    private static int2 GetGridCoords(Vector3 worldPosition)
    {
        int x = (int)worldPosition.x - (worldPosition.x < 0 ? 1 : 0);
        int z = (int)worldPosition.z - (worldPosition.z < 0 ? 1 : 0);
        return new int2(x, z);
    }
}
