﻿using Ecosystem.ECS.Hybrid;
using UnityEngine;

namespace Ecosystem.Samples
{
    public class BasicFollowing : MonoBehaviour
    {
        [SerializeField]
        private Movement movement = default;
        [SerializeField]
        private Sensors sensors = default;

        [SerializeField]
        private float reach = 1f;
        [SerializeField]
        private float range = 100f;
        [SerializeField]
        [Tooltip("How often this unit calculates a new path")]
        private float pathfindInterval = 1f;

        private float timeSinceLastFrame = 0;

        private void Update()
        {
            timeSinceLastFrame += Time.deltaTime;
            if (timeSinceLastFrame < pathfindInterval) return;
            timeSinceLastFrame = 0f;

            sensors.LookForFood(true);
            sensors.LookForPredator(true);
            sensors.LookForPrey(true);
            sensors.LookForWater(true);

            Vector3 closestTarget = new Vector3();

            bool foundFood = sensors.FoundFood();
            bool foundPrey = sensors.FoundPrey();

            if (foundFood && foundPrey)
            {
                Vector3 foodLocation = sensors.GetFoodLocation();
                Vector3 preyLocation = sensors.GetPreyLocation();

                closestTarget = (Vector3.Distance(transform.position, foodLocation)
                    < Vector3.Distance(transform.position, preyLocation))
                    ? foodLocation
                    : preyLocation;
            }
            else if (foundFood)
            {
                closestTarget = sensors.GetFoodLocation();
            }
            else if (foundPrey)
            {
                closestTarget = sensors.GetPreyLocation();
            }

            if (foundFood || foundPrey)
            {
                movement.Move(closestTarget, reach, range);
            }
        }
    }
}
