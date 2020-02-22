using System.Collections.Generic;
using UnityEngine;

namespace Ecosystem
{
    [CreateAssetMenu(menuName = "AnimalType")]
    public class AnimalType : ScriptableObject
    {
        [Tooltip("Animals that are prey to this animal type")]
        public List<AnimalType> Prey = new List<AnimalType>();

        [Tooltip("Food that is edible for this animal type")]
        public List<FoodType> Food = new List<FoodType>();
    }
}
