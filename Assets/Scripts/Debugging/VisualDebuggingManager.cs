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

        [Header("Enable/Disable")]
        [SerializeField] private bool hearingDebugShow = default;
        [SerializeField] private bool pathDebugShow = default;
        [SerializeField] private bool stateDebugShow = default;
        [SerializeField] private bool statusDebugShow = default;

        [Header("Hearing")]
        [SerializeField] private Material hearingMaterial = default;

        [Header("Path")]
        [SerializeField] private Material pathMaterial = default;

        [Header("State")]
        [SerializeField] private Material stateMaterial = default;
        [SerializeField] private float stateRadius = 0.15f;
        [SerializeField] private float stateHeight = 3.5f;
        [SerializeField] private Color defaultColor = default;
        [SerializeField] private Color casualColor = default;
        [SerializeField] private Color hungerColor = default;
        [SerializeField] private Color thirstColor = default;
        [SerializeField] private Color mateColor = default;
        [SerializeField] private Color fleeColor = default;

        [Header("Status")]
        [SerializeField] private Material statusMaterial = default;
        [SerializeField] private float statusHeight = 2.5f;
        [SerializeField] private new Camera camera = default;


        private HearingDebuggingSystem hearingDebuggingSystem;
        private PathDebuggingSystem pathDebuggingSystem;
        private AnimalStateDebugging animalStateDebuggingSystem;
        private StatusBarDebuggingSystem statusBarDebuggingSystem;

        private void Awake()
        {
            World world = World.DefaultGameObjectInjectionWorld;
            hearingDebuggingSystem = world.GetOrCreateSystem<HearingDebuggingSystem>();
            pathDebuggingSystem = world.GetOrCreateSystem<PathDebuggingSystem>();
            animalStateDebuggingSystem = world.GetOrCreateSystem<AnimalStateDebugging>();
            statusBarDebuggingSystem = world.GetOrCreateSystem<StatusBarDebuggingSystem>();
        }

        private void Update()
        {
            hearingDebuggingSystem.Material = hearingMaterial;
            hearingDebuggingSystem.Show = hearingDebugShow;

            pathDebuggingSystem.Material = pathMaterial;
            pathDebuggingSystem.Show = pathDebugShow;

            animalStateDebuggingSystem.Material = stateMaterial;
            animalStateDebuggingSystem.Show = stateDebugShow;
            animalStateDebuggingSystem.Radius = stateRadius;
            animalStateDebuggingSystem.Height = stateHeight;
            animalStateDebuggingSystem.DefaultColor = defaultColor;
            animalStateDebuggingSystem.CasualColor = casualColor;
            animalStateDebuggingSystem.HungerColor = hungerColor;
            animalStateDebuggingSystem.ThirstColor = thirstColor;
            animalStateDebuggingSystem.MateColor = mateColor;
            animalStateDebuggingSystem.FleeColor = fleeColor;

            statusBarDebuggingSystem.mainCamera = camera;
            statusBarDebuggingSystem.Material = statusMaterial;
            statusBarDebuggingSystem.Show = statusDebugShow;
            statusBarDebuggingSystem.Height = statusHeight;
            statusBarDebuggingSystem.HungerColor = hungerColor;
            statusBarDebuggingSystem.ThirstColor = thirstColor;
            statusBarDebuggingSystem.MateColor = mateColor;
        }
    }
}
