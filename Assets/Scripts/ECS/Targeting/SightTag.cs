using System;

using Unity.Entities;

namespace Ecosystem.ECS.Targeting
{
    /// <summary>
    /// Marks the entity as something with senses
    /// </summary>
    [Serializable]
    [GenerateAuthoringComponent]
    public struct SightTag : IComponentData 
    {
        
    }
}
