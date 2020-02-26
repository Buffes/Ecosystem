using UnityEngine;
using Ecosystem.ECS.Hybrid;


namespace Ecosystem.Attributes {
    public interface IAnimal {
        string FoodSource { get; }
        Transform Trans { get; set; }
        float Speed { get; set; }
        float SprintSpeed { get; set; }
        void Move(Vector3 target);
        Sensors Sensors { get; }
    }
}