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
        [SerializeField]
        private Material hearingDebugMaterial = default;
        [SerializeField]
        private bool hearingDebugShow = default;

        private HearingDebuggingSystem hearingDebuggingSystem;

        private void Awake()
        {
            hearingDebuggingSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<HearingDebuggingSystem>();
        }

        private void Update()
        {
            hearingDebuggingSystem.Material = hearingDebugMaterial;
            hearingDebuggingSystem.Show = hearingDebugShow;
        }
    }
}
