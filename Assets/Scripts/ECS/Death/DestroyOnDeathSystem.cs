using Unity.Entities;
using UnityEngine;

namespace Ecosystem.ECS.Death
{
    /// <summary>
    /// Destroys game objects on death.
    /// </summary>
    public class DestroyOnDeathSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities
                .WithoutBurst()
                .WithAll<DeathEvent>()
                .ForEach((DestroyOnDeath destroyOnDeath) =>
                {
                    var d = destroyOnDeath.Destroy;
                    if (d == null) return;
                    Object.Destroy(d);
                }).Run();
        }
    }
}
