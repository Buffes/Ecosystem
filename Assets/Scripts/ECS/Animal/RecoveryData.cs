using System;
using Unity.Entities;

namespace Ecosystem.ECS.Animal {

    [Serializable]
    [GenerateAuthoringComponent]
    public struct RecoveryData : IComponentData {
        public float RecoveryTime;
        public float RecoveryLimit;
    }
}
