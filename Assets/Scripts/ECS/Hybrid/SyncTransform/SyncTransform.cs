using System;
using Unity.Entities;
using UnityEngine;

namespace Ecosystem.ECS.Hybrid.SyncTransform
{
    /// <summary>
    /// A GameObject's Transform to sync the position of with this entity.
    /// </summary>
    [Serializable]
    [GenerateAuthoringComponent]
    public class SyncTransform : IComponentData
    {
        public Transform transform;
    }
}
