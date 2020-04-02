using Ecosystem.ECS.Animal.Needs;
using Ecosystem.ECS.Debugging.Selection;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using Unity.Collections;
using Unity.Mathematics;

namespace Ecosystem.ECS.Debugging
{
    public class StatusBarDebuggingSystem : SystemBase
    {
        public Camera mainCamera { get; set; }
        public bool Show { get; set; }
        public Material Material { get; set; }
        public Color HungerColor { get; set; }
        public Color ThirstColor { get; set; }
        public Color MateColor { get; set; }
        public float Height { get; set; }

        private Mesh mesh;

        protected override void OnCreate()
        {
            mesh = CreateMesh();
        }


        private Mesh CreateMesh()
        {
            Mesh mesh = new Mesh();

            Vector3[] vertices = new Vector3[4];
            Vector2[] uv = new Vector2[4];
            int[] triangles = new int[6];

            vertices[0] = new Vector3(0,0.25f);
            vertices[1] = new Vector3(1,0.25f);
            vertices[2] = new Vector3(0,0);
            vertices[3] = new Vector3(1,0);

            uv[0] = new Vector2(0,0.25f);
            uv[1] = new Vector2(1,0.25f);
            uv[2] = new Vector2(0,0);
            uv[3] = new Vector2(1,0);

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
        private void Draw(float cur, float max, Matrix4x4 matrix, Color color)
        {
            MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
            materialPropertyBlock.SetVector("_Color", color);
            materialPropertyBlock.SetFloat("_Fill", cur / max);
            UnityEngine.Graphics.DrawMesh(
                mesh,
                matrix,
                Material,
                1,
                mainCamera,
                0,
                materialPropertyBlock
            );
        }

        protected override void OnUpdate()
        {
            if (!Show) return;
            Entities.WithoutBurst().WithAll<Selected>().ForEach((Entity entity,
                in Translation position,
                in HungerData hungerData,
                in MaxHungerData maxHunger) =>
            {
                float3 pos = position.Value;
                pos.y += Height;
                Matrix4x4 m = Matrix4x4.TRS(pos, Quaternion.identity, Vector3.one);
                Draw(hungerData.Hunger, maxHunger.MaxHunger, m, HungerColor);
            }).Run();

            Entities.WithoutBurst().WithAll<Selected>().ForEach((Entity entity,
                in Translation position,
                in ThirstData thirstData,
                in MaxThirstData maxThirst) =>
            {
                float3 pos = position.Value;
                pos.y += Height + 0.5f;
                Matrix4x4 m = Matrix4x4.TRS(pos, Quaternion.identity, Vector3.one);
                Draw(thirstData.Thirst, maxThirst.MaxThirst, m, ThirstColor);
            }).Run();

            Entities.WithoutBurst().WithAll<Selected>().ForEach((Entity entity,
                in Translation position,
                in SexualUrgesData urgesData) =>
            {
                float3 pos = position.Value;
                pos.y += Height + 1.0f;
                Matrix4x4 m = Matrix4x4.TRS(pos, Quaternion.identity, Vector3.one);
                Draw(urgesData.Urge, 1.0f, m, MateColor);
            }).Run();

            
        }
    }
}
