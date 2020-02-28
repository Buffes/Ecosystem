using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demonstration
{
    public class Pathfinder : MonoBehaviour
    {
        OOPathfinding pathfinding;
        List<PathNode> path;

        void Start()
        {
            pathfinding = new OOPathfinding(100, 100);
        }

        // Update is called once per frame
        void Update()
        {
            path = pathfinding.FindPath(0,0,50,50);
        }
    }
}