using Ecosystem.Attributes;
using System.Collections.Generic;
using UnityEngine;

namespace Ecosystem.Gameplay
{
    [CreateAssetMenu(menuName = "Gameplay/AnimalType")]
    public class AnimalType : AnimalTypeComponent
    {
        [Tooltip("The name of this animal (e.g., Fox, Rabbit, Eagle)")]
        public string Name;

        [Header("Stats")]
        public float MovementSpeed = 3;
        public float HearingRange = 6;
        public float VisionRange = 12;
        public float VisionAngle = 2;
        public float MaxHunger = 3;
        public float MaxThirst = 3;
        public float MaxSexualUrge = 3;
        public float Bravery = 0.5f;
        public float Lifespan = 20;
        public float GestationPeriod = 0.2f;

        [Header("Terrain")]

        public bool Land = true;
        public bool Water = false;

        [Header("Prefabs")]

        [Tooltip("The meat that comes from this animal")]
        public GameObject Meat;

        [Tooltip("The prefab that this animal spawns babies from")]
        public GameObject Baby;

        [Header("Lists")]

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
