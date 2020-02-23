using UnityEngine;

namespace Ecosystem.ECS.Hybrid
{
    /// <summary>
    /// Functionality for awareness of surroundings (e.g., vision and hearing).
    /// </summary>
    public class Sensors : MonoBehaviour
    {
        /// <summary>
        /// Start/stop actively looking for water.
        /// </summary>
        public void LookForWater(bool enabled)
        {
        }

        public void LookForFood(bool enabled)
        {
        }

        public void LookForPrey(bool enabled)
        {
        }

        public void LookForPredator(bool enabled)
        {
        }


        /// <summary>
        /// Returns if water has been found. Make sure to have enabled
        /// <see cref="LookForWater(bool)"/> first.
        /// </summary>
        public bool FoundWater()
        {
            return true;
        }

        public bool FoundFood()
        {
            return true;
        }

        public bool FoundPrey()
        {
            return true;
        }

        public bool FoundPredator()
        {
            return true;
        }


        /// <summary>
        /// Returns the location where water has been found.
        /// Make sure that water has been found first by checking <see cref="FoundWater()"/>.
        /// </summary>
        public Vector3 GetWaterLocation()
        {
            return new Vector3();
        }

        public Vector3 GetFoodLocation()
        {
            return new Vector3();
        }

        public Vector3 GetPreyLocation()
        {
            return new Vector3();
        }

        public Vector3 GetPredatorLocation()
        {
            return new Vector3();
        }
    }
}
