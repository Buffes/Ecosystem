using Unity.Entities;

namespace Ecosystem.ECS.Stats.Modifier
{
    /// <summary>
    /// Applies percent modifiers on stats. The modifiers can not reduce the stats below 0.
    /// <para/>
    /// For example, if an animal is sprinting, its movement speed stat could get a speed percent
    /// modifier with the value 1 (100%) to double its speed. Or if an animal is crippled after
    /// hurting its leg, it could get a speed percent modifier of -0.8 (-80%) to effectively
    /// make it five times slower.
    /// </summary>
    [UpdateInGroup(typeof(PercentStatModifierSystemGroup))]
    public abstract class PercentModifierSystem : SystemBase
    {
        /// <summary>
        /// Applies percent modifiers on a stat.
        /// </summary>
        protected static void ApplyModifiers<T>(ref float stat, DynamicBuffer<T> modifierBuffer)
            where T : struct, IBufferElementData
        {
            DynamicBuffer<float> modifiers = modifierBuffer.Reinterpret<float>();
            for (int i = modifiers.Length - 1; i >= 0; i--)
            {
                float modifier = modifiers[i];
                modifiers.RemoveAt(i);

                if (stat <= 0) continue; // Don't multiply negative values
                stat *= 1 + modifier; // Multiply stat
                if (stat < 0) stat = 0; // Don't reduce stat below 0
            }
        }
    }
}
