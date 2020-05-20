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
        public float MovementSpeed = 4;
        public float Lifespan = 10;
        [Range(0, 1)] public float InfertilityAge = 0.9f;
        public float GestationPeriod = 1f;
        public float Bravery = 0.5f;

        [Header("Senses")]
        public float HearingRange = 8;
        public float VisionRange = 16;
        public float VisionAngle = 2.4f;

        [Header("Needs")]
        public float MaxHunger = 4;
        [Range(0, 1)] public float HungerLimit = 0.3f;
        public float MaxThirst = 4;
        [Range(0, 1)] public float ThirstLimit = 0.3f;
        public float MaxSexualUrge = 4;
        [Range(0, 1)] public float MatingLimit = 0.3f;


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
