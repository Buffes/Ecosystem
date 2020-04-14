using Ecosystem.Attributes;
using System.Collections.Generic;
using UnityEngine;

namespace Ecosystem.Gameplay
{
    [CreateAssetMenu(menuName = "Gameplay/AnimalType")]
    public class AnimalType : AnimalTypeComponent
    {
        [Tooltip("The meat that comes from this animal")]
        public GameObject Meat;

        [Tooltip("The prefab that this animal spawns babies from")]
        public Animal Baby;

        [Tooltip("The name of this animal (Fox,Rabbit,Fish,Eagle)")]
        public string Name;

        [Tooltip("Animals that are prey to this animal")]
        [SerializeField]
        private List<AnimalTypeComponent> prey = new List<AnimalTypeComponent>();

        [Tooltip("Food that this animal can eat")]
        [SerializeField]
        private List<FoodTypeComponent> food = new List<FoodTypeComponent>();

        public List<AnimalType> Prey { get; private set; }
        public List<FoodType> Food { get; private set; }

        public override void AddLeaves(List<AnimalType> list)
        {
            list.Add(this);
        }

        private void OnEnable()
        {
            Prey = AnimalType.GetLeaves(prey);
            Food = FoodType.GetLeaves(food);
        }
    }
}
