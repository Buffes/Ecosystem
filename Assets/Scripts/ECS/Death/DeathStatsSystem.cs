using Unity.Entities;
using UnityEngine;
using System.Threading;

namespace Ecosystem.ECS.Death {
    public class DeathStatsSystem : SystemBase {

        public int hunger;
        public int thirst;
        public int age;
        public int predator;
        public int other;

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
                switch(deathEvent.Cause) {
                    case DeathCause.Hunger: Interlocked.Increment(ref hunger); break;
                    case DeathCause.Thirst: Interlocked.Increment(ref thirst); break;
                    case DeathCause.Age: Interlocked.Increment(ref age); break;
                    case DeathCause.Predators: Interlocked.Increment(ref predator); break;
                    default: Interlocked.Increment(ref other); break;
                }
            }).Run();

        }
    }
}
