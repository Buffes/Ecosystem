using Ecosystem.ECS.Hybrid;
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

        private float timeUntilNextPathfind = 0;

        private void Awake()
        {
            sensors.Converted += Init;
        }

        private void OnDestroy()
        {
            sensors.Converted -= Init;
        }

        private void Init()
        {
            sensors.LookForFood(true);
            sensors.LookForPredator(true);
            sensors.LookForPrey(true);
            sensors.LookForWater(true);
        }

        private void Update()
        {
            if (!sensors.HasConverted) return;

            timeUntilNextPathfind -= Time.deltaTime;
            if (timeUntilNextPathfind > 0f) return;
            timeUntilNextPathfind = pathfindInterval;

            Vector3 closestTarget = new Vector3();

            bool foundFood = sensors.FoundFood();
            bool foundPrey = sensors.FoundPrey();

            if (foundFood && foundPrey)
            {
                Vector3 foodLocation = sensors.GetFoundFoodInfo().Position;
                Vector3 preyLocation = sensors.GetFoundPreyInfo().Position;

                closestTarget = (Vector3.Distance(transform.position, foodLocation)
                    < Vector3.Distance(transform.position, preyLocation))
                    ? foodLocation
                    : preyLocation;
            }
            else if (foundFood)
            {
                closestTarget = sensors.GetFoundFoodInfo().Position;
            }
            else if (foundPrey)
            {
                closestTarget = sensors.GetFoundPreyInfo().Position;
            }

            if (foundFood || foundPrey)
            {
                movement.Move(closestTarget, reach, range);
            }
        }
    }
}
