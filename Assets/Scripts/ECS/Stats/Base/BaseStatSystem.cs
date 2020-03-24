using Unity.Entities;

namespace Ecosystem.ECS.Stats.Base
{
    /// <summary>
    /// Resets practical stats to their base values.
    /// <para/>
    /// The stats can then be modified by adding stat modifier components.
    /// </summary>
    [UpdateInGroup(typeof(BaseStatSystemGroup))]
    public abstract class BaseStatSystem : SystemBase
    {
        /// <summary>
        /// Resets a practical stat to a base value.
        /// </summary>
        protected static void ResetToBase<T>(ref float stat, in T baseStat)
            where T : struct, IComponentData, BaseStat
        {
            stat = baseStat.BaseValue;
        }
    }
}
