using UnityEngine;

namespace Ecosystem
{
    [CreateAssetMenu(menuName = "FoodType")]
    public class FoodType : ScriptableObject
    {
        [Range(1, 10)]
        public int FoodPoints = 1;
    }
}