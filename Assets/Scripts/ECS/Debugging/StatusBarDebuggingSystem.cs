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
        protected override void OnUpdate()
        {
            if (!Show) return;
            Entities
                .WithoutBurst()
                .WithAll<Selected>()
                .ForEach((Entity entity, 
                    in Translation position,
                    in HungerData hungerData,
                    in ThirstData thirstData,
                    in SexualUrgesData urgesData,
                    in MaxHungerData maxHungerData,
                    in MaxThirstData maxThirstData) =>
                {
                    MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
                    
                    float3 pos = position.Value;
                    pos.y += Height;
                    pos.x -= 0.5f;
                    Matrix4x4 m = Matrix4x4.TRS(pos, Quaternion.identity, Vector3.one);

                    materialPropertyBlock.SetVector("_Color", HungerColor);
                    materialPropertyBlock.SetFloat("_Fill", hungerData.Hunger / maxHungerData.MaxHunger);
                    UnityEngine.Graphics.DrawMesh(
                        mesh,
                        m,
                        Material,
                        1,
                        mainCamera,
                        0,
                        materialPropertyBlock
                    );
                    

                    pos.y += 0.5f;
                    m = Matrix4x4.TRS(pos, Quaternion.identity, Vector3.one);

                    materialPropertyBlock.SetVector("_Color", ThirstColor);
                    materialPropertyBlock.SetFloat("_Fill", thirstData.Thirst / maxThirstData.MaxThirst);
                    UnityEngine.Graphics.DrawMesh(
                        mesh,
                        m,
                        Material,
                        1,
                        mainCamera,
                        0,
                        materialPropertyBlock
                    );

                    pos.y += 0.5f;
                    m = Matrix4x4.TRS(pos, Quaternion.identity, Vector3.one);

                    materialPropertyBlock.SetVector("_Color", MateColor);
                    materialPropertyBlock.SetFloat("_Fill", urgesData.Urge/1.0f);
                    UnityEngine.Graphics.DrawMesh(
                        mesh,
                        m,
                        Material,
                        1,
                        mainCamera,
                        0,
                        materialPropertyBlock
                    );

                }).Run();
        }
    }
}
