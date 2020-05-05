using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;

namespace Ecosystem.ECS.Debugging.Selection
{
    /// <summary>
    /// Select entities by clicking on them.
    /// </summary>
    public class SelectionSystem : SystemBase
    {
        private static readonly int RAYCAST_DISTANCE = 1000;

        private Camera cam;

        protected override void OnCreate()
        {
            RequireForUpdate(GetEntityQuery(new EntityQueryDesc()));
        }

        protected override void OnStartRunning()
        {
            cam = Camera.main;
        }

        protected override void OnUpdate()
        {
            if (Input.GetMouseButtonDown(0))
            {
                DeselectAll();
                if (cam == null) cam = Camera.main;
                if (cam == null) return;
                Select(cam.ScreenPointToRay(Input.mousePosition));
            }
            else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.A))
            {
                SelectAll();
            }
        }

        private void Select(UnityEngine.Ray ray)
        {
            var from = ray.origin;
            var to = ray.GetPoint(RAYCAST_DISTANCE);

            // Select clicked entity
            if (Raycast(from, to, out Entity clickedEntity))
            {
                EntityManager.AddComponentData(clickedEntity, new Selected());
            }
        }

        private void DeselectAll()
        {
            Entities
                .WithoutBurst()
                .WithStructuralChanges()
                .WithAll<Selected>()
                .ForEach((Entity entity) =>
            {
                EntityManager.RemoveComponent<Selected>(entity);
            }).Run();
        }

        private void SelectAll()
        {
            Entities
                .WithoutBurst()
                .WithStructuralChanges()
                .WithNone<Selected>()
                .ForEach((Entity entity) =>
                {
                    EntityManager.AddComponentData(entity, new Selected());
                }).Run();
        }

        private bool Raycast(float3 from, float3 to, out Entity entity)
        {
            var physicsWorld = World.GetExistingSystem<Unity.Physics.Systems.BuildPhysicsWorld>().PhysicsWorld;
            var collisionWorld = physicsWorld.CollisionWorld;

            RaycastInput input = new RaycastInput()
            {
                Start = from,
                End = to,
                Filter = new CollisionFilter()
                {
                    BelongsTo = ~0u,
                    CollidesWith = ~0u, // all 1s, so all layers, collide with everything
                    GroupIndex = 0
                }
            };

            bool hasHit = collisionWorld.CastRay(input, out Unity.Physics.RaycastHit hit);
            entity = hasHit ? physicsWorld.Bodies[hit.RigidBodyIndex].Entity : Entity.Null;
            return hasHit;
        }
    }
}
