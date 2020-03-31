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

        private Mesh mesh;
        private TextMesh bar;

        protected override void OnCreate()
        {
            mainCamera = Camera.main;

            m_EndSimulationEcbSystem = World
                .GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();

            query = GetEntityQuery(
                ComponentType.ReadOnly<Selected>(),
                ComponentType.ReadOnly<Translation>());
        } 

        protected override void OnUpdate()
        {
            if (!Show) return;

            Entities
                .WithoutBurst()
                .WithAll<Selected>()
                .ForEach((Entity entity,
                    in HungerData hungerData,
                    in ThirstData thirstData,
                    in SexualUrgesData sexualUrgesData) =>
                {

                }).Run();
        }

    }
}
