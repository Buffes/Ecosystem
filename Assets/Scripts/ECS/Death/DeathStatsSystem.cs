using Unity.Entities;
using UnityEngine;
using System.Threading;
using System;
using System.IO;
using Ecosystem.Gameplay;
using Ecosystem.ECS.Animal;

namespace Ecosystem.ECS.Death {
    public class DeathStatsSystem : SystemBase {

        public int hunger;
        public int thirst;
        public int age;
        public int predator;
        public int other;

        public int rabbitHunger;
        public int rabbitThirst;
        public int rabbitAge;
        public int rabbitPredator;
        public int rabbitOther;

        public int foxHunger;
        public int foxThirst;
        public int foxAge;
        public int foxPredator;
        public int foxOther;

        protected override void OnCreate() {
            base.OnCreate();
            hunger = 0;
            thirst = 0;
            age = 0;
            predator = 0;
            other = 0;
            rabbitHunger = 0;
            rabbitThirst = 0;
            rabbitAge = 0;
            rabbitPredator = 0;
            rabbitOther = 0;
            foxHunger = 0;
            foxThirst = 0;
            foxAge = 0;
            foxPredator = 0;
            foxOther = 0;
        }

        protected override void OnUpdate() {

            Entities.WithoutBurst().ForEach((Entity entity,int entityInQueryIndex,in DeathEvent deathEvent,in AnimalTypeData type) => {

                switch (deathEvent.Cause) {
                    case DeathCause.Hunger:
                        Interlocked.Increment(ref hunger);
                        if (type.AnimalName == AnimalTypeNames.Fox) { Interlocked.Increment(ref foxHunger); }
                        else { Interlocked.Increment(ref rabbitHunger); }
                        break;
                    case DeathCause.Thirst:
                        if (type.AnimalName == AnimalTypeNames.Fox) { Interlocked.Increment(ref foxThirst); }
                        else { Interlocked.Increment(ref rabbitThirst); }
                        Interlocked.Increment(ref thirst);
                        break;
                    case DeathCause.Age:
                        if (type.AnimalName == AnimalTypeNames.Fox) { Interlocked.Increment(ref foxAge); }
                        else { Interlocked.Increment(ref rabbitAge); }
                        Interlocked.Increment(ref age);
                        break;
                    case DeathCause.Predators:
                        if (type.AnimalName == AnimalTypeNames.Fox) { Interlocked.Increment(ref foxPredator); }
                        else { Interlocked.Increment(ref rabbitPredator); }
                        Interlocked.Increment(ref predator);
                        break;
                    default:
                        if (type.AnimalName == AnimalTypeNames.Fox) { Interlocked.Increment(ref foxOther); }
                        else { Interlocked.Increment(ref rabbitOther); }
                        Interlocked.Increment(ref other);
                        break;
                }
            }).Run();

        }

        protected override void OnDestroy() {
            base.OnDestroy();
            using (StreamWriter sw = new StreamWriter("DeathCauses.csv")) {
                sw.WriteLine("Cause,Total,Rabbit,Fox");
                sw.WriteLine("Hunger," + hunger + "," + rabbitHunger + "," + foxHunger);
                sw.WriteLine("Thirst," + thirst + "," + rabbitThirst + "," + foxThirst);
                sw.WriteLine("Age," + age + "," + rabbitAge + "," + foxAge);
                sw.WriteLine("Predators," + predator + "," + rabbitPredator + "," + foxPredator);
                sw.WriteLine("Other," + other + "," + rabbitOther + "," + foxOther);
            }
        }
    }
}
