using System;
using Unity.Entities;
using UnityEngine;

namespace Ecosystem.ECS.Movement
{
    /// <summary>
    /// Defines how fast an entity can move and sprint.
    /// </summary>
    [Serializable]
    [GenerateAuthoringComponent]
    public struct MovementStats : IComponentData
    {
        public float Speed;
        public float SprintSpeed;
    }
}
