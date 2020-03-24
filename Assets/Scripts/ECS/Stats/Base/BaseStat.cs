namespace Ecosystem.ECS.Stats.Base
{
    /// <summary>
    /// Resets a stat to a base value.
    /// </summary>
    public interface BaseStat
    {
        /// <summary>
        /// The base value to reset the stat to.
        /// </summary>
        float BaseValue { get; }
    }
}
