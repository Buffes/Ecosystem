using UnityEngine;
using Ecosystem.StateMachines;

namespace Ecosystem.Attributes {
    public interface IAnimal {
        public float Hunger { get; set; }
        public float Thirst { get; set; }
        public float Mating { get; set; }
    }
}