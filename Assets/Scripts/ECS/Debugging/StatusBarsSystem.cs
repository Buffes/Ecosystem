using Ecosystem.ECS.Debugging.Graphics;
using Ecosystem.ECS.Debugging.Selection;
using Ecosystem.ECS.Targeting.Sensors;
using Ecosystem.ECS.Animal.Needs;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using Unity.Collections;
using System.Collections.Generic;
using Unity.Mathematics;

namespace Ecosystem.ECS.Debugging
{
    public class StatusBarsSystem : SystemBase
    {
        Camera mainCamera;
        public bool Show { get; set; }
        public Material Material { get; set; }
        public Color HungerColor { get; set; }
        public Color ThirstColor { get; set; }
        public Color MateColor { get; set; }
        public float Height { get; set; }

        private EndSimulationEntityCommandBufferSystem m_EndSimulationEcbSystem;

        private EntityQuery query;

        //private Mesh mesh;
        private TextMesh bar;

        private Mesh mesh;

        protected override void OnCreate()
        {
            mainCamera = Camera.main;

            m_EndSimulationEcbSystem = World
                .GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();

            query = GetEntityQuery(
                ComponentType.ReadOnly<Selected>(),
                ComponentType.ReadOnly<Translation>(),
                ComponentType.ReadOnly<HungerData>(),
                ComponentType.ReadOnly<ThirstData>(),
                ComponentType.ReadOnly<SexualUrgesData>());

            mesh = CreateMesh();
        }


        private Mesh CreateMesh()
        {
            Mesh mesh = new Mesh();

            Vector3[] vertices = new Vector3[4];
            Vector2[] uv = new Vector2[4];
            int[] triangles = new int[6];

            vertices[0] = new Vector3(0,1);
            vertices[1] = new Vector3(4,1);
            vertices[2] = new Vector3(0,0);
            vertices[3] = new Vector3(4,0);

            uv[0] = new Vector2(0,1);
            uv[1] = new Vector2(4,1);
            uv[2] = new Vector2(0,0);
            uv[3] = new Vector2(4,0);

            triangles[0] = 0;
            triangles[1] = 1;
            triangles[2] = 2;
            triangles[3] = 2;
            triangles[4] = 1;
            triangles[5] = 3;

            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.triangles = triangles;

            return mesh;

        }
        protected override void OnUpdate()
        {
            if (!Show) return;

            Entities
                .WithoutBurst()
                .WithAll<Selected>()
                .ForEach((Entity entity,
                    in Translation translation,
                    in HungerData hungerData,
                    in ThirstData thirstData,
                    in SexualUrgesData sexualUrgesData) =>
                {


                }).Run();
        }

        private void otherDraw()
        {

        }

        private void Draw()
        {
            var translations = query.ToComponentDataArray<Translation>(Allocator.TempJob);
            var hunger = query.ToComponentDataArray<HungerData>(Allocator.TempJob);
            var thirst = query.ToComponentDataArray<ThirstData>(Allocator.TempJob);
            var urge = query.ToComponentDataArray<SexualUrgesData>(Allocator.TempJob);

            MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
            List<Matrix4x4> matrices = new List<Matrix4x4>();

            for (int i = 0; i < translations.Length; i++)
            {
                float3 position = translations[i].Value;
                position.y += Height;
                materialPropertyBlock.SetFloat("_Fill", hunger[i].Hunger / 1.0f);
                matrices.Add(Matrix4x4.TRS(position, Quaternion.identity, Vector3.one));
            }

            materialPropertyBlock.SetFloat("_Fill", 2.0f / 1.0f);

            UnityEngine.Graphics.DrawMeshInstanced(
                mesh,
                0,
                Material,
                matrices,
                materialPropertyBlock
            );

            translations.Dispose(Dependency);
            hunger.Dispose(Dependency);
            thirst.Dispose(Dependency);
            urge.Dispose(Dependency);
        }

    }
}
