using Unity.Entities;
using UnityEngine;
using System.Threading;
using System;
using System.IO;

namespace Ecosystem.ECS.Death {
    public class DeathStatsSystem : SystemBase {

        public int hunger;
        public int thirst;
        public int age;
        public int predator;
        public int other;

        protected override void OnCreate() {
            Debug.Log("create");
            base.OnCreate();
            hunger = 0;
            thirst = 0;
            age = 0;
            predator = 0;
            other = 0;
        }
        protected override void OnUpdate() {
            Debug.Log("update");

            Entities.WithoutBurst().ForEach((Entity entity,int entityInQueryIndex,in DeathEvent deathEvent) => {
                switch(deathEvent.Cause) {
                    case DeathCause.Hunger: Debug.Log("hunger"); Interlocked.Increment(ref hunger); break;
                    case DeathCause.Thirst: Debug.Log("thirst"); Interlocked.Increment(ref thirst); break;
                    case DeathCause.Age: Debug.Log("age"); Interlocked.Increment(ref age); break;
                    case DeathCause.Predators: Debug.Log("predators"); Interlocked.Increment(ref predator); break;
                    default: Debug.Log("other"); Interlocked.Increment(ref other); break;
                }
            }).Run();

        }

        protected override void OnDestroy()
		{
            base.OnDestroy();
            Debug.Log("destroy");
            using (StreamWriter sw = new StreamWriter("DeathCauses.csv"))
            {
                sw.WriteLine("Cause,Number");
                sw.WriteLine("Hunger," + hunger);
                sw.WriteLine("Thirst," + thirst);
                sw.WriteLine("Age," + age);
                sw.WriteLine("Predators," + predator);
                sw.WriteLine("Other," + other);
            }
        }
    }
}
