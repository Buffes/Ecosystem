using Ecosystem.ECS.Debugging.Selection;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Ecosystem.CameraControl
{
    public class FollowedEntitySystem : SystemBase
    {
        public bool FollowingEntity { get; private set; }
        public Vector3 Position { get; private set; }

        public void ToggleFollow()
        {
            var entities = GetEntityQuery(ComponentType.ReadOnly<Selected>(), ComponentType.ReadOnly<Translation>()).ToEntityArray(Allocator.TempJob);

            if (entities.Length == 1)
            {
                Entity entity = entities[0];
                if (EntityManager.HasComponent<Followed>(entity)) Unfollow();
                else
                {
                    Unfollow();
                    EntityManager.AddComponent<Followed>(entity);
                }
            }
            else Unfollow();

            entities.Dispose();
        }

        private void Unfollow()
        {
            FollowingEntity = false;
            Entities
                .WithoutBurst()
                .WithStructuralChanges()
                .WithAll<Followed>()
                .ForEach((Entity entity) =>
                {
                    EntityManager.RemoveComponent<Followed>(entity);
                }).Run();
        }

        protected override void OnUpdate()
        {
            FollowingEntity = false;
            Entities
                .WithoutBurst()
                .WithAll<Followed>()
                .ForEach((Translation position) =>
                {
                    Position = position.Value;
                    FollowingEntity = true;
                }).Run();
        }

        private struct Followed : IComponentData { }
    }
}
