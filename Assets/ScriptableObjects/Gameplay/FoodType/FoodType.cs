using System.Collections.Generic;
using UnityEngine;

namespace Ecosystem.Gameplay
{
    [CreateAssetMenu(menuName = "Gameplay/FoodType")]
    public class FoodType : FoodTypeComponent
    {
        [Tooltip("How much hunger this food type restores")]
        [Range(1, 10)]
        public int FoodPoints = 1;

        public override void AddLeaves(List<FoodType> list)
        {
            list.Add(this);
        }
    }
}
