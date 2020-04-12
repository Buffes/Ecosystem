using Unity.Entities;
using System.Collections.Generic;
using System.IO;
using Ecosystem.ECS.Animal;
using Unity.Collections;

namespace Ecosystem.ECS.Death {
    public class DeathStatsSystem : SystemBase {

        public struct DeathStats {
            public NativeString64 name;
            public int hunger;
            public int thirst;
            public int age;
            public int predator;
            public int other;

            public DeathStats(NativeString64 name,int hunger, int thirst, int age, int predator, int other) {
                this.name = name;
                this.hunger = hunger;
                this.thirst = thirst;
                this.age = age;
                this.predator = predator;
                this.other = other;
            }
        }

        public Dictionary<NativeString64,DeathStats> deathStats;

        protected override void OnCreate() {
            base.OnCreate();
            deathStats = new Dictionary<NativeString64,DeathStats>();
        }

        protected override void OnUpdate() {

            Entities.WithoutBurst().ForEach((Entity entity,int entityInQueryIndex,in DeathEvent deathEvent,in AnimalTypeData type) => {
                if (!deathStats.ContainsKey(type.AnimalName)) {
                    deathStats.Add(type.AnimalName,new DeathStats { name = type.AnimalName });
                }
                DeathStats ds = deathStats[type.AnimalName];
                switch (deathEvent.Cause) {
                    case DeathCause.Hunger:
                        deathStats[type.AnimalName] = new DeathStats(type.AnimalName,ds.hunger+1,ds.thirst,ds.age,ds.predator,ds.other);
                        break;
                    case DeathCause.Thirst:
                        deathStats[type.AnimalName] = new DeathStats(type.AnimalName,ds.hunger,ds.thirst+1,ds.age,ds.predator,ds.other);
                        break;
                    case DeathCause.Age:
                        deathStats[type.AnimalName] = new DeathStats(type.AnimalName,ds.hunger,ds.thirst,ds.age+1,ds.predator,ds.other);
                        break;
                    case DeathCause.Predators:
                        deathStats[type.AnimalName] = new DeathStats(type.AnimalName,ds.hunger,ds.thirst,ds.age,ds.predator+1,ds.other);
                        break;
                    default:
                        deathStats[type.AnimalName] = new DeathStats(type.AnimalName,ds.hunger,ds.thirst,ds.age,ds.predator,ds.other+1);
                        break;
                }
            }).Run();

        }

        protected override void OnDestroy() {
            base.OnDestroy();

            DeathStats total = new DeathStats { name = "Total" };
            foreach (DeathStats stats in deathStats.Values) {
                total.hunger += stats.hunger;
                total.thirst += stats.thirst;
                total.age += stats.age;
                total.predator += stats.predator;
                total.other += stats.other;
            }
            
            deathStats.Add("Total",total);
            using (StreamWriter sw = new StreamWriter("DeathCauses.csv")) {
                sw.WriteLine("Animal,Hunger,Thirst,Age,Predators,Other");
                foreach (DeathStats stats in deathStats.Values) {
                    sw.WriteLine(stats.name + "," + stats.hunger + "," + stats.thirst + "," + stats.age + "," + stats.predator + "," + stats.other);
                }
            }
        }
    }
}
