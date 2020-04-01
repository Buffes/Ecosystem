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

        private EntityQuery query;

        private Mesh mesh;

        protected override void OnCreate()
        {
            //mainCamera = Camera.main;

            query = GetEntityQuery(
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

            vertices[0] = new Vector3(0,0.5f);
            vertices[1] = new Vector3(4,0.5f);
            vertices[2] = new Vector3(0,0);
            vertices[3] = new Vector3(4,0);

            uv[0] = new Vector2(0,0.5f);
            uv[1] = new Vector2(4,0.5f);
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
                    in Translation position,
                    in HungerData hungerData,
                    in ThirstData thirstData,
                    in SexualUrgesData urgesData) =>
                {
                    MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
                    
                    float3 pos = position.Value;
                    pos.y += Height;
                    pos.x -= 2.0f;
                    var cam = mainCamera.transform;
                    var forward = (Vector3)pos - cam.position;
                    forward.Normalize();
                    var up = Vector3.Cross(forward, cam.right);
                    //Matrix4x4 m = Matrix4x4.LookAt(cam.position, pos, up);
                    Matrix4x4 m = Matrix4x4.TRS(pos, Quaternion.identity, Vector3.one);


                    materialPropertyBlock.SetFloat("_Fill", 0.5f);
                    UnityEngine.Graphics.DrawMesh(
                        mesh,
                        m,
                        Material,
                        1,
                        mainCamera,
                        0,
                        materialPropertyBlock
                    );
                    pos.y += 0.9f;
                    m = Matrix4x4.TRS(pos, Quaternion.identity, Vector3.one);
                    materialPropertyBlock.SetFloat("_Fill", 0.75f);
                    UnityEngine.Graphics.DrawMesh(
                        mesh,
                        m,
                        Material,
                        1,
                        mainCamera,
                        0,
                        materialPropertyBlock
                    );

                    pos.y += 0.9f;
                    m = Matrix4x4.TRS(pos, Quaternion.identity, Vector3.one);
                    materialPropertyBlock.SetFloat("_Fill", 1.0f);
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
            //Draw();
        }

        private void Draw()
        {
            var translations = query.ToComponentDataArray<Translation>(Allocator.TempJob);
            var hunger = query.ToComponentDataArray<HungerData>(Allocator.TempJob);
            var thirst = query.ToComponentDataArray<ThirstData>(Allocator.TempJob);
            var urge = query.ToComponentDataArray<SexualUrgesData>(Allocator.TempJob);

            MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();

            for (int i = 0; i < translations.Length; i++)
            {
                float3 position = translations[i].Value;
                position.y += Height;             
                Matrix4x4 m = Matrix4x4.TRS(position, Quaternion.identity, Vector3.one);

                materialPropertyBlock.SetFloat("_Fill", hunger[i].Hunger / 1.0f);
                UnityEngine.Graphics.DrawMesh(
                    mesh,
                    m,
                    Material,
                    1,
                    mainCamera,
                    0,
                    materialPropertyBlock
                );

                position.y += 0.5f;
                m = Matrix4x4.TRS(position, Quaternion.identity, Vector3.one);
                materialPropertyBlock.SetFloat("_Fill", thirst[i].Thirst / 1.0f);
                UnityEngine.Graphics.DrawMesh(
                    mesh,
                    m,
                    Material,
                    1,
                    mainCamera,
                    0,
                    materialPropertyBlock
                );

                position.y += 0.5f;
                m = Matrix4x4.TRS(position, Quaternion.identity, Vector3.one);
                materialPropertyBlock.SetFloat("_Fill", urge[i].Urge / 1.0f);
                UnityEngine.Graphics.DrawMesh(
                    mesh,
                    m,
                    Material,
                    1,
                    mainCamera,
                    0,
                    materialPropertyBlock
                );


            }

            translations.Dispose(Dependency);
            hunger.Dispose(Dependency);
            thirst.Dispose(Dependency);
            urge.Dispose(Dependency);
        }

    }
}
