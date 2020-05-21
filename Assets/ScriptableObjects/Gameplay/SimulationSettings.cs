using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ecosystem.Gameplay
{
    [CreateAssetMenu(menuName = "Gameplay/SimulationSettings")]
    public class SimulationSettings : ScriptableObject
    {
        [SerializeField]
        private List<AnimalPopulation> initialPopulations = new List<AnimalPopulation>();

        public List<AnimalPopulation> InitialPopulations => initialPopulations;

        [Serializable]
        public class AnimalPopulation
        {
            public GameObject Prefab;
            public int Amount;
        }

        public void Clear() => initialPopulations.Clear();

        public void AddAnimalPopulation(GameObject prefab, int amount)
        {
            initialPopulations.Add(new AnimalPopulation
            {
                Prefab = prefab,
                Amount = amount
            });
        }
    }
}
