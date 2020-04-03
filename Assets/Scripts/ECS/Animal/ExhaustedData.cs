using System;
using Unity.Entities;

namespace Ecosystem.ECS.Animal {
    [Serializable]
    [GenerateAuthoringComponent]
    public struct ExhaustedData : IComponentData {
        public float TimeUntilSprintPossible;
        public ExhaustedData(float time) {
            TimeUntilSprintPossible = time;
        }
    }
}
