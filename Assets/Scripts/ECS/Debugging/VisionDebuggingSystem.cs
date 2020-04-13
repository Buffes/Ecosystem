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

        private Mesh CreateMesh(float angle, float range) => MeshShapeUtils.CreateCircleSection(angle, range); //Change this

        protected override void OnUpdate() {
            if (!Show) return;

            Entities
                .WithoutBurst()
                .WithAll<Selected>()
                .ForEach((Entity entity, in Vision vision, in Translation pos, in Rotation rot) => {
                    Mesh mesh = CreateMesh(vision.Angle, vision.Range);

                    var position = pos.Value;
                    position.y += 0.5f;

                    UnityEngine.Graphics.DrawMesh(
                        mesh, 
                        position, 
                        rot.Value, 
                        Material, 
                        0
                    );
                }).Run();
        }
    }
}
