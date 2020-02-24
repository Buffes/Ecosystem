using System;
using Unity.Entities;
using Unity.Mathematics;

namespace Ecosystem.ECS.Targeting
{
    /// <summary>
    /// The id of the targets an animal is searching for
    /// </summary>
    [Serializable]
    public struct TargetCommand : IComponentData
    {
        public int targetID; 
    }
}