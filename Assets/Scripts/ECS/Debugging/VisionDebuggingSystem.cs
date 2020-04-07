using Ecosystem.ECS.Debugging.Selection;
using Ecosystem.ECS.Targeting.Sensors;
using Ecosystem.StateMachines;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Ecosystem.ECS.Debugging {
    /// <summary>
    /// Draws spheres above animals heads indicating what states they are in by changing the color.
    /// </summary>
    public class VisionDebuggingSystem : SystemBase {
        public bool Show { get; set; }
        public Material Material { get; set; }

        protected override void OnCreate() {
            UpdateMesh();
        }

        private Mesh CreateMesh(float angle, float range) => MeshShapeUtils.CreateCircleSection(angle, range); //Change this

        protected override void OnUpdate() {
            if (!Show) return;

            UpdateMesh();
        }

        private void UpdateMesh() {
            Entities
                .WithoutBurst()
                .WithAll<Selected>()
                .ForEach((Entity entity, Vision vision, Translation pos, Rotation rot) => {
                    Mesh mesh = CreateMesh(vision.Angle, vision.Range);

                    UnityEngine.Graphics.DrawMesh(
                        mesh, 
                        pos.Value, 
                        rot.Value, 
                        Material, 
                        0
                    );
                }).Run();
        }
    }
}
