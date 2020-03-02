using UnityEngine;
using Ecosystem.ECS.Hybrid;


namespace Ecosystem.Attributes {
    public interface IAnimal {
        //float Hunger { get; set; }
        //float Thirst { get; set; }
        //float Mating { get; set; }
        string FoodSource { get; }
        Transform Trans { get; set; }
        float Speed { get; set; }
        float SprintSpeed { get; set; }
        void Move(Vector3 target,float reach,float range);
        Sensors GetSensors();
        void Die();
        float GetHunger();
        void SetHunger(float newHunger);
        float GetThirst();
        void SetThirst(float newThirst);
    }
}