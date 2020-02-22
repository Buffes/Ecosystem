using System.Collections.Generic;
using UnityEngine;

namespace Ecosystem
{
    [CreateAssetMenu(menuName = "AnimalType")]
    public class AnimalType : ScriptableObject
    {
        [Tooltip("The type of meat that comes from this animal")]
        public FoodType Meat;

        [Tooltip("Animals that are prey to this animal")]
        public List<AnimalType> Prey = new List<AnimalType>();

        [Tooltip("Food that this animal can eat")]
        public List<FoodType> Food = new List<FoodType>();
    }
}
