﻿using System;
using Unity.Entities;
using Unity.Mathematics;

namespace Ecosystem.ECS.Targeting.Targets
{
    [Serializable]
    [GenerateAuthoringComponent]
    public struct LookingForPrey : IComponentData
    {
        public bool HasFound;
        public Entity Entity;
        public float3 Position;
        public float3 PredictedPosition;
    }
}
