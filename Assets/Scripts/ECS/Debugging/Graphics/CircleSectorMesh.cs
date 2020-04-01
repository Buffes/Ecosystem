using Unity.Entities;

namespace Ecosystem.ECS.Debugging.Graphics {
    public struct CircleSectorMesh : IComponentData {
        public float Range;
        public float Angle;
    }
}
