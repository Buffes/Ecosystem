using UnityEngine;

namespace Ecosystem
{
    [CreateAssetMenu(menuName = "FoodType")]
    public class FoodType : ScriptableObject
    {
        [Tooltip("How much hunger this food type restores")]
        [Range(1, 10)]
        public int FoodPoints = 1;
    }
}