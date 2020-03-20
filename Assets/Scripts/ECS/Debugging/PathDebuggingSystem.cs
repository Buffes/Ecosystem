using Unity.Entities;
using UnityEngine;
using Ecosystem.ECS.Movement.Pathfinding;
using Ecosystem.ECS.Debugging.Selection;
using Unity.Mathematics;
using Unity.Transforms;

namespace Ecosystem.ECS.Debugging
{
    public class PathDebuggingSystem : SystemBase
    {
        private const float PATH_LINE_Y = 1;

        public bool Show { get; set; }
        public Material Material { get; set; }

        protected override void OnUpdate()
        {
            if (!Show) return;

            Entities
                .WithoutBurst()
                .WithAll<Selected>()
                .ForEach((Entity entity, in DynamicBuffer<PathElement> path, in Translation position) =>
                {
                    DrawPath(path, position.Value);
                }).Run();
        }

        private void DrawPath(DynamicBuffer<PathElement> path, float3 start)
        {
            Vector3[] points = new Vector3[path.Length + 1];

            for (int i = 0; i < path.Length; i++)
            {
                points[i] = path[i].Checkpoint;
            }
            points[points.Length - 1] = start;

            UnityEngine.Graphics.DrawMesh(
                    CreateLineMesh(points, PATH_LINE_Y),
                    new Vector3(),
                    Quaternion.identity,
                    Material,
                    0);
        }

        private static Mesh CreateLineMesh(Vector3[] points, float y)
        {
            int[] indices = new int[points.Length];

            for (int i = 0; i < indices.Length; i++)
            {
                indices[i] = i;
            }

            for (int i = 0; i < points.Length; i++)
            {
                points[i].y = y;
            }

            Mesh mesh = new Mesh();
            mesh.vertices = points;
            mesh.SetIndices(indices, MeshTopology.LineStrip, 0);
            return mesh;
        }
    }
}
