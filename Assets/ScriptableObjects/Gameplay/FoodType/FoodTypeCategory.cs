using System.Collections.Generic;
using UnityEngine;

namespace Ecosystem.Gameplay
{
    [CreateAssetMenu(menuName = "Gameplay/FoodCategory")]
    public class FoodTypeCategory : FoodTypeComponent
    {
        public List<FoodTypeComponent> Children = new List<FoodTypeComponent>();

        public override void AddLeaves(List<FoodType> list)
        {
            foreach (FoodTypeComponent component in Children)
            {
                component.AddLeaves(list);
            }
        }
    }
}
