using UnityEngine;
using Ecosystem.ECS.Hybrid;

namespace Ecosystem.Attributes {
    public interface IAnimal {
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