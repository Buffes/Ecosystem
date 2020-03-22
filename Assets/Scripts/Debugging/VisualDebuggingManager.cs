using Ecosystem.ECS.Debugging;
using Unity.Entities;
using UnityEngine;

namespace Ecosystem.Debugging
{
    /// <summary>
    /// Manages debugging systems: what to display and materials to use.
    /// </summary>
    public class VisualDebuggingManager : MonoBehaviour
    {
        [Header("Materials")]
        [SerializeField]
        private Material hearingDebugMaterial = default;
        [SerializeField]
        private Material pathDebugMaterial = default;

        [Header("Enable/Disable")]
        [SerializeField]
        private bool hearingDebugShow = default;
        [SerializeField]
        private bool pathDebugShow = default;

        private HearingDebuggingSystem hearingDebuggingSystem;
        private PathDebuggingSystem pathDebuggingSystem;

        private void Awake()
        {
            World world = World.DefaultGameObjectInjectionWorld;
            hearingDebuggingSystem = world.GetOrCreateSystem<HearingDebuggingSystem>();
            pathDebuggingSystem = world.GetOrCreateSystem<PathDebuggingSystem>();
        }

        private void Update()
        {
            hearingDebuggingSystem.Material = hearingDebugMaterial;
            hearingDebuggingSystem.Show = hearingDebugShow;
            pathDebuggingSystem.Material = pathDebugMaterial;
            pathDebuggingSystem.Show = pathDebugShow;
        }
    }
}
