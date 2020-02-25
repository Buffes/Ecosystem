using UnityEngine;
using Ecosystem.ECS.Hybrid.Movement;


namespace Ecosystem.Attributes {
    public interface IAnimal {
        float Hunger { get; set; }
        float Thirst { get; set; }
        float Mating { get; set; }
        string FoodSource { get; }
        Transform Trans { get; set; }
        float Speed { get; set; }
        float SprintSpeed { get; set; }
        void Move(Vector3 target);
        
    }
}