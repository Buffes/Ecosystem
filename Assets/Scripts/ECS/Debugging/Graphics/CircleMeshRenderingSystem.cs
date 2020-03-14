using System;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

namespace Ecosystem.ECS.Debugging.Graphics
{
    /// <summary>
    /// Uses <see cref="CircleMesh"/> + <see cref="ShapeStyle"/> to render circles.
    /// </summary>
    public class CircleMeshRenderingSystem : SystemBase
    {
        // How many edges the shape has (the circle is not perfectly round)
        private static readonly int CIRCLE_MESH_RESOLUTION = 40;

        private Mesh circleMesh;

        protected override void OnCreate()
        {
            circleMesh = CreateCircleMesh(CIRCLE_MESH_RESOLUTION);
        }

        protected override void OnUpdate()
        {
            // Update circle radius
            Entities
                .WithChangeFilter<CircleMesh>()
                .ForEach((Entity entity,
                ref NonUniformScale scale,
                in CircleMesh circleRenderMesh) =>
                {
                    float radius = circleRenderMesh.Radius;

                    scale.Value.x = radius;
                    scale.Value.z = radius;
                }).Run();

            // Convert CircleMesh + ShapeStyle into RenderMesh + NonUniformScale
            Entities
                .WithStructuralChanges()
                .WithoutBurst()
                .WithNone<RenderMesh>()
                .ForEach((Entity entity,
                in CircleMesh circleMesh,
                in ShapeStyle shapeStyle) =>
                {
                    EntityManager.AddSharedComponentData(entity, new RenderMesh
                    {
                        mesh = this.circleMesh,
                        material = shapeStyle.Material,
                        castShadows = UnityEngine.Rendering.ShadowCastingMode.Off,
                        receiveShadows = false
                    });

                    float r = circleMesh.Radius;
                    EntityManager.AddComponentData(entity, new NonUniformScale { Value = new float3(r, 1, r) });
                }).Run();
        }

        private static Mesh CreateCircleMesh(int resolution)
        {
            if (resolution < 3) throw new ArgumentException("Resolution cannot be lower than 3");

            float radius = 1;
            int n = resolution;

            Vector3[] vertices = new Vector3[n];
            for (int i = 0; i < n; i++)
            {
                float x = radius * Mathf.Sin(2 * Mathf.PI * i / n);
                float z = radius * Mathf.Cos(2 * Mathf.PI * i / n);
                vertices[i] = new Vector3(x, 0.5f, z);
            }

            int[] triangles = new int[3 * (n - 2)];
            for (int i = 0; i < 3 * (n - 2); i += 3)
            {
                triangles[i] = 0;
                triangles[i + 1] = i / 3 + 1;
                triangles[i + 2] = i / 3 + 2;
            }

            Vector3[] normals = new Vector3[vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                normals[i] = -Vector3.forward;
            }

            Mesh mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.normals = normals;

            return mesh;
        }
    }
}
