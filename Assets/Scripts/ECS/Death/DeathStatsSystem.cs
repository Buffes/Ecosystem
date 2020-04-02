using Unity.Entities;
using UnityEngine;
using System.Threading;

namespace Ecosystem.ECS.Death {
    /// <summary>
    /// Kills desired entities
    /// </summary>
    public class DeathStatsSystem : SystemBase {

        private int hunger;
        private int thirst;
        private int age;
        private int predator;
        private int other;


        protected override void OnCreate() {
            base.OnCreate();
            hunger = 0;
            thirst = 0;
            age = 0;
            predator = 0;
            other = 0;
        }
        protected override void OnUpdate() {

            Entities.WithoutBurst().ForEach((Entity entity,int entityInQueryIndex,in DeathEvent deathEvent) => {
                switch(deathEvent.DeathCause) {
                    case 1: Interlocked.Increment(ref hunger); break;
                    case 2: Interlocked.Increment(ref thirst); break;
                    case 3: Interlocked.Increment(ref age); break;
                    case 4: Interlocked.Increment(ref predator); break;
                    default: Interlocked.Increment(ref other); break;
                }
            }).Run();

        }

        protected override void OnDestroy() {
            base.OnDestroy();
            Debug.Log("Hunger: "+hunger);
            Debug.Log("Thirst: " + thirst);
            Debug.Log("Age: " + age);
            Debug.Log("Predator: " + predator);
            Debug.Log("Other: " + other);
        }
    }
}
