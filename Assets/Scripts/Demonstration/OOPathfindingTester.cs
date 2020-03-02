using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OOPathfindingTester : MonoBehaviour
{
    public GameObject pathfinder;
    void Start()
    {
        OOPathfinding pathfinding = new OOPathfinding(100, 100);
        for (int i = 0; i < 100; i++)
        {
            Instantiate(pathfinder);
        }
    }
}
