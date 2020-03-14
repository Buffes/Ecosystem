using Unity.Entities;
using UnityEngine;

namespace Ecosystem.ECS.Death
{
    /// <summary>
    /// Destroys an accompanying game object on death.
    /// </summary>
    [GenerateAuthoringComponent]
    public class DestroyOnDeath : IComponentData
    {
        public Object Destroy;
    }
}
