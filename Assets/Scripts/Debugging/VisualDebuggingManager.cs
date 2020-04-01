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
        [SerializeField]
        private Material visionDebugMaterial = default;

        [Header("Enable/Disable")]
        [SerializeField]
        private bool hearingDebugShow = default;
        [SerializeField]
        private bool pathDebugShow = default;
        [SerializeField]
        private bool visionDebugShow = default;

        private HearingDebuggingSystem hearingDebuggingSystem;
        private PathDebuggingSystem pathDebuggingSystem;
        private VisionDebuggingSystem visionDebuggingSystem;

        private void Awake()
        {
            World world = World.DefaultGameObjectInjectionWorld;
            hearingDebuggingSystem = world.GetOrCreateSystem<HearingDebuggingSystem>();
            pathDebuggingSystem = world.GetOrCreateSystem<PathDebuggingSystem>();
            visionDebuggingSystem = world.GetOrCreateSystem<VisionDebuggingSystem>();

        }

        private void Update()
        {
            hearingDebuggingSystem.Material = hearingDebugMaterial;
            hearingDebuggingSystem.Show = hearingDebugShow;
            pathDebuggingSystem.Material = pathDebugMaterial;
            pathDebuggingSystem.Show = pathDebugShow;
            visionDebuggingSystem.Material = visionDebugMaterial;
            visionDebuggingSystem.Show = visionDebugShow;

        }
    }
}
