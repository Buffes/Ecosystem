using Unity.Entities;
using UnityEngine;
using System.Threading;
using System.Collections.Generic;
using System.IO;
using Ecosystem.Gameplay;
using Ecosystem.ECS.Animal;
using Unity.Collections;

namespace Ecosystem.ECS.Death {
    public class DeathStatsSystem : SystemBase {

        public struct DeathStats {
            public string name;
            public int hunger;
            public int thirst;
            public int age;
            public int predator;
            public int other;
        }

        public DeathStats fox;
        public DeathStats rabbit;
        public DeathStats fish;
        public DeathStats eagle;

        protected override void OnCreate() {
            base.OnCreate();
            fox = new DeathStats { name = "Fox" };
            rabbit = new DeathStats { name = "Rabbit" };
            fish = new DeathStats { name = "Fish" };
            eagle = new DeathStats { name = "Eagle" };
        }

        protected override void OnUpdate() {

            Entities.WithoutBurst().ForEach((Entity entity,int entityInQueryIndex,in DeathEvent deathEvent,in AnimalTypeData type) => {

                switch (deathEvent.Cause) {
                    case DeathCause.Hunger:
                        switch (type.AnimalName) {
                            case AnimalTypeNames.Fox: Interlocked.Increment(ref fox.hunger); break;
                            case AnimalTypeNames.Rabbit: Interlocked.Increment(ref rabbit.hunger); break;
                            case AnimalTypeNames.Fish: Interlocked.Increment(ref fish.hunger); break;
                            case AnimalTypeNames.Eagle: Interlocked.Increment(ref eagle.hunger); break;
                            default: break;
                        }
                        break;
                    case DeathCause.Thirst:
                        switch (type.AnimalName) {
                            case AnimalTypeNames.Fox: Interlocked.Increment(ref fox.thirst); break;
                            case AnimalTypeNames.Rabbit: Interlocked.Increment(ref rabbit.thirst); break;
                            case AnimalTypeNames.Fish: Interlocked.Increment(ref fish.thirst); break;
                            case AnimalTypeNames.Eagle: Interlocked.Increment(ref eagle.thirst); break;
                            default: break;
                        }
                        break;
                    case DeathCause.Age:
                        switch (type.AnimalName) {
                            case AnimalTypeNames.Fox: Interlocked.Increment(ref fox.age); break;
                            case AnimalTypeNames.Rabbit: Interlocked.Increment(ref rabbit.age); break;
                            case AnimalTypeNames.Fish: Interlocked.Increment(ref fish.age); break;
                            case AnimalTypeNames.Eagle: Interlocked.Increment(ref eagle.age); break;
                            default: break;
                        }
                        break;
                    case DeathCause.Predators:
                        switch (type.AnimalName) {
                            case AnimalTypeNames.Fox: Interlocked.Increment(ref fox.predator); break;
                            case AnimalTypeNames.Rabbit: Interlocked.Increment(ref rabbit.predator); break;
                            case AnimalTypeNames.Fish: Interlocked.Increment(ref fish.predator); break;
                            case AnimalTypeNames.Eagle: Interlocked.Increment(ref eagle.predator); break;
                            default: break;
                        }
                        break;
                    default:
                        switch (type.AnimalName) {
                            case AnimalTypeNames.Fox: Interlocked.Increment(ref fox.other); break;
                            case AnimalTypeNames.Rabbit: Interlocked.Increment(ref rabbit.other); break;
                            case AnimalTypeNames.Fish: Interlocked.Increment(ref fish.other); break;
                            case AnimalTypeNames.Eagle: Interlocked.Increment(ref eagle.other); break;
                            default: break;
                        }
                        break;
                }
            }).Run();

        }

        protected override void OnDestroy() {
            base.OnDestroy();
            List<DeathStats> deathStats = new List<DeathStats> {
                fox, rabbit, fish, eagle
            };
            DeathStats total = new DeathStats { name = "Total"};
            foreach (DeathStats stats in deathStats) {
                total.hunger += stats.hunger;
                total.thirst += stats.thirst;
                total.age += stats.age;
                total.predator += stats.predator;
                total.other += stats.other;
            }
            deathStats.Add(total);
            using (StreamWriter sw = new StreamWriter("DeathCauses.csv")) {
                sw.WriteLine("Animal,Hunger,Thirst,Age,Predators,Other");
                foreach (DeathStats stats in deathStats) {
                    sw.WriteLine(stats.name + "," + stats.hunger + "," + stats.thirst + "," + stats.age + "," + stats.predator + "," + stats.other);
                }
            }
        }
    }
}
