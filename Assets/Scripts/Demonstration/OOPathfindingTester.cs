using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OOPathfindingTester : MonoBehaviour
{
    public GameObject pathfinder;
    void Start()
    {
        for (int i = 0; i < 100; i++)
        {
            Instantiate(pathfinder);
        }
    }
}
