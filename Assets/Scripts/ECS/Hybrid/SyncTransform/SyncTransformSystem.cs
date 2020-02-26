using Unity.Entities;
using Unity.Transforms;

namespace Ecosystem.ECS.Hybrid.SyncTransform
{
    /// <summary>
    /// Syncs the position of a game object's transform with an entity's translation.
    /// </summary>
    public class SyncTransformSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities
                .WithoutBurst()
                .ForEach((
                    SyncTransform syncTransform,
                    in Translation position,
                    in Rotation rotation) =>
            {

                syncTransform.transform.position = position.Value;
                syncTransform.transform.rotation = rotation.Value;

            }).Run();
        }
    }
}
