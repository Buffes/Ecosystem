using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ecosystem
{
    [CreateAssetMenu(menuName = "Gameplay/SimulationSettings")]
    public class SimulationSettings : ScriptableObject
    {
        [SerializeField]
        private List<AnimalPopulation> initialPopulations = new List<AnimalPopulation>();

        public Dictionary<string, AnimalPopulation> InitialPopulations { get; } = new Dictionary<string, AnimalPopulation>();

        private void OnEnable()
        {
            InitialPopulations.Clear();
            foreach (var population in initialPopulations)
            {
                InitialPopulations.Add(population.Prefab.name.ToLower(), population);
            }
        }

        [Serializable]
        public class AnimalPopulation
        {
            public GameObject Prefab;
            public int Amount;
        }
    }
}
