using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Ecosystem.ECS.Targeting
{
    /// <summary>
    /// The id of a sensed entity
    /// </summary>
    
    [Serializable]
    [InternalBufferCapacity(20)]
    [GenerateAuthoringComponent]
    public struct Target : IBufferElementData
    {
        //public Entity e; 
        public int targetID;
    }
}

