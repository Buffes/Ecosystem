using System.Collections.Generic;

namespace Ecosystem.Graphs
{
    /// <summary>
    /// For collecting average animal stats over time. E.g., the average speed of all rabbits, all foxes, etc.
    /// </summary>
    public class AverageAnimalStats : AnimalStats<float>
    {
        private class SumEntry
        {
            public int Count;
            public float ValueSum;

            public float Average => Count > 0 ? ValueSum / Count : 0;

            public void Clear()
            {
                Count = 0;
                ValueSum = 0;
            }
        }

        private Dictionary<string, SumEntry> animalStatValueSums = new Dictionary<string, SumEntry>();

        /// <param name="path">The path to the file to write the data to</param>
        public AverageAnimalStats(string path) : base(path, "Average Value") { }

        private void Clear()
        {
            foreach (var sumEntry in animalStatValueSums.Values) sumEntry.Clear();
        }

        /// <summary>
        /// Adds the specified value to the sum of values for the specified animal.
        /// </summary>
        public void AddStatValue(string animal, float value)
        {
            if (!animalStatValueSums.ContainsKey(animal)) animalStatValueSums.Add(animal, new SumEntry());

            var sumEntry = animalStatValueSums[animal];
            sumEntry.Count++;
            sumEntry.ValueSum += value;
        }

        /// <summary>
        /// Adds a new data point with the average value for each animal since the last data point.
        /// </summary>
        public void AddDataPoint(double timestamp)
        {
            foreach (var pair in animalStatValueSums)
            {
                if (pair.Value.Count == 0) continue;
                AddDataPoint(timestamp, pair.Key, pair.Value.Average);
            }
            Clear();
        }
        public void AddDataPointCount(double timestamp)
        {
            foreach (var pair in animalStatValueSums)
            {
                AddDataPoint(timestamp, pair.Key, pair.Value.Count);
            }
            Clear();
        }
    }
}