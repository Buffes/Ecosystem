using Unity.Entities;
using Unity.Collections;
using Unity.Jobs.LowLevel.Unsafe;

namespace Ecosystem.ECS.Random
{
    /// <summary>
    /// Setup for thread-safe pseudo-randomness for ECS/Jobs using Unity.Mathematics.Random.
    /// Runs during initialization.
    /// </summary>
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    class RandomSystem : SystemBase
    {
        public NativeArray<Unity.Mathematics.Random> RandomArray {get; private set;}

        protected override void OnCreate()
        {
            var randomArray = new Unity.Mathematics.Random[JobsUtility.MaxJobThreadCount];
            var seed = new System.Random(); // Not the only option for getting a seed

            for (int i = 0; i < JobsUtility.MaxJobThreadCount; i++)
            {
                randomArray[i] = new Unity.Mathematics.Random((uint)seed.Next());
            }

            RandomArray = new NativeArray<Unity.Mathematics.Random>(randomArray, Allocator.Persistent);
        }

        protected override void OnDestroy()
        {
            RandomArray.Dispose();
        }

        protected override void OnUpdate() { }
    }
}
