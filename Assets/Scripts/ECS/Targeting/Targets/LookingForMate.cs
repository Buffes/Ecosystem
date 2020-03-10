﻿using System;
using Unity.Entities;
using Unity.Mathematics;
using Ecosystem.ECS.Animal.Sex;

namespace Ecosystem.ECS.Targeting.Targets
{
    [Serializable]
    [GenerateAuthoringComponent]
    public struct LookingForMate : IComponentData
    {
        public bool HasFound;
        /*public Sexes Sex; //Sex preference*/
        public Entity Entity;
        public float3 Position;

    }
}
