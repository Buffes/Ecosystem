using UnityEngine;
using Ecosystem.ECS.Hybrid;

namespace Ecosystem.Attributes {
    public interface IAnimal {
        string FoodSource { get; }
        float Speed { get; set; }
        float SprintSpeed { get; set; }
        void Move(Vector3 target,float reach,float range);
        Sensors GetSensors();
        void Die();
        float GetHunger();
        void SetHunger(float newHunger);
        float GetThirst();
        void SetThirst(float newThirst);
        Transform GetTransform();
    }
}