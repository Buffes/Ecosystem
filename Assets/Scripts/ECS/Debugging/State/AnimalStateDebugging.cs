using Ecosystem.ECS.Debugging.Selection;
using Ecosystem.StateMachines;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Ecosystem.ECS.Debugging
{
    /// <summary>
    /// Draws spheres above animals heads indicating what states they are in by changing the color.
    /// </summary>
    public class AnimalStateDebugging : SystemBase
    {
        public bool Show { get; set; }
        public Material Material { get; set; }
        public float Radius { get; set; }
        public float Height { get; set; }

        public Color DefaultColor { get; set; }
        public Color CasualColor { get; set; }
        public Color HungerColor { get; set; }
        public Color ThirstColor { get; set; }
        public Color MateColor { get; set; }
        public Color FleeColor { get; set; }
        public Color HuntColor { get; set; }

        private EntityQuery query;

        private Mesh mesh;
        private float prevRadius;

        protected override void OnCreate()
        {
            query = GetEntityQuery(
                ComponentType.ReadOnly<Selected>(),
                ComponentType.ReadOnly<StateColor>(),
                ComponentType.ReadOnly<Translation>());

            CreateMesh();
        }

        private void CreateMesh() => mesh = MeshShapeUtils.CreateSphere(Radius);

        protected override void OnUpdate()
        {
            if (!Show) return;

            if (prevRadius != Radius) CreateMesh();
            prevRadius = Radius;

            Entities
                .WithoutBurst()
                .WithAll<Selected>()
                .ForEach((Entity entity, StateMachineRef stateMachine, ref StateColor stateColor) =>
                {
                    Color color = DefaultColor;
                    IState state = stateMachine.StateMachine.getCurrentState();

                    if      (state is CasualState) color = CasualColor;
                    else if (state is HungerState) color = HungerColor;
                    else if (state is ThirstState) color = ThirstColor;
                    else if (state is MateState) color = MateColor;
                    else if (state is FleeState) color = FleeColor;
                    else if (state is HuntState) color = HuntColor;

                    stateColor.Value = color;
                }).Run();

            Draw();
        }

        private void Draw()
        {
            var translations = query.ToComponentDataArray<Translation>(Allocator.TempJob);
            var stateColors = query.ToComponentDataArray<StateColor>(Allocator.TempJob);

            MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
            List<Matrix4x4> matrices = new List<Matrix4x4>();
            List<Vector4> colors = new List<Vector4>();

            for (int i = 0; i < translations.Length; i++)
            {
                float3 position = translations[i].Value;
                position.y += Height;
                matrices.Add(Matrix4x4.TRS(position, Quaternion.identity, Vector3.one));
                colors.Add(stateColors[i].Value);
            }

            materialPropertyBlock.SetVectorArray("_Colors", colors);

            UnityEngine.Graphics.DrawMeshInstanced(
                mesh,
                0,
                Material,
                matrices,
                materialPropertyBlock
            );

            translations.Dispose(Dependency);
            stateColors.Dispose(Dependency);
        }
    }
}
