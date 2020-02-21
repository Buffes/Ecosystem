using UnityEngine;
using Ecosystem.StateMachines;

namespace Ecosystem.Attributes {
    public interface IAnimal {
        float Hunger { get; set; }
        float Thirst { get; set; }
        float Mating { get; set; }
        string FoodSource { get; }
        Transform Trans { get; set; }
    }
}