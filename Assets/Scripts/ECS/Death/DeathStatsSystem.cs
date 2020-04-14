using Unity.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using Ecosystem.ECS.Animal;
using Unity.Collections;
using System.Text;

namespace Ecosystem.ECS.Death
{
    public class DeathStatsSystem : SystemBase
    {
        public class DeathStats
        {
            private Dictionary<DeathCause, int> deathCauseCount = new Dictionary<DeathCause, int>();

            public void Increment(DeathCause deathCause)
            {
                deathCauseCount.TryGetValue(deathCause, out var count);
                deathCauseCount[deathCause] = count + 1;
            }

            public int GetCount(DeathCause deathCause)
            {
                deathCauseCount.TryGetValue(deathCause, out var count);
                return count;
            }
        }

        public Dictionary<NativeString64, DeathStats> deathStats = new Dictionary<NativeString64, DeathStats>();
        public DeathStats totalDeathStats = new DeathStats();

        protected override void OnUpdate()
        {
            Entities
                .WithoutBurst()
                .ForEach((Entity entity, int entityInQueryIndex,
                in DeathEvent deathEvent,
                in AnimalTypeData type) =>
                {
                    if (!deathStats.ContainsKey(type.AnimalName))
                    {
                        deathStats.Add(type.AnimalName, new DeathStats());
                    }

                    deathStats[type.AnimalName].Increment(deathEvent.Cause);
                    totalDeathStats.Increment(deathEvent.Cause);
                }).Run();
        }

        protected override void OnDestroy()
        {
            deathStats.Add("Total", totalDeathStats);

            using (StreamWriter sw = new StreamWriter("DeathCauses.csv"))
            {
                StringBuilder sb = new StringBuilder();
                WriteLine(sw, sb, "Animal", (DeathCause deathCause) => deathCause.ToString());

                foreach (KeyValuePair<NativeString64, DeathStats> pair in deathStats)
                {
                    WriteLine(sw, sb, pair.Key.ToString(), (DeathCause cause) => pair.Value.GetCount(cause).ToString());
                }
            }
        }

        private void WriteLine(StreamWriter sw, StringBuilder sb, string firstValue, Func<DeathCause, string> forEachCause)
        {
            sb.Clear();
            sb.Append(firstValue);

            foreach (DeathCause deathCause in Enum.GetValues(typeof(DeathCause)))
            {
                sb.Append(",").Append(forEachCause(deathCause));
            }

            sw.WriteLine(sb.ToString());
        }
    }
}
