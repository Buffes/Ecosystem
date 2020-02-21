using UnityEngine;

namespace Ecosystem.ECS.Hybrid
{
    public class Movement : MonoBehaviour
    {
        /// <summary>
        /// Sends a move command to move to the specified target position.
        /// Avoid calling this too often because for every frame that it is called in,
        /// a new path will be calculated.
        /// </summary>
        /// <param name="target">the target position to move to</param>
        public void Move(Vector3 target)
        {
        }
    }
}