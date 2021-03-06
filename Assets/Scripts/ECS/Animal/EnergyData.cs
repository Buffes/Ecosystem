﻿using System;
using Unity.Entities;

namespace Ecosystem.ECS.Animal {
    /// <summary>
    /// A float value ranging from 0 to infinity indicating the level of energy.
    /// </summary>
    [Serializable]
    [GenerateAuthoringComponent]
    public struct EnergyData : IComponentData {
        public float Energy;
    }
}
