using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Ecosystem.Graphs
{
    /// <summary>
    /// For collecting animal data over time.
    /// </summary>
    /// <typeparam name="T">The type of the stat value</typeparam>
    public class AnimalStats<T>
    {
        private struct AnimalStatsEntry
        {
            private readonly int timestamp;
            private readonly string animal;
            private readonly T value;

            public AnimalStatsEntry(double timestamp, string animal, T value)
            {
                this.timestamp = (int)timestamp;
                this.animal = animal;
                this.value = value;
            }

            public override string ToString()
            {
                return string.Format("{0},{1},{2}", timestamp.ToString(), animal, value.ToString());
            }
        }

        private static readonly string STATS_DIRECTORY
            = Path.Combine(Directory.GetParent(Application.dataPath).FullName, "Stats");
        private string path;
        private string valueTitle;
        private List<AnimalStatsEntry> entries = new List<AnimalStatsEntry>();

        /// <param name="path">The path to the file to write the data to</param>
        /// <param name="valueTitle">The title of the value column.
        /// If multiple values, you can separate the titles with commas</param>
        public AnimalStats(string path, string valueTitle)
        {
            this.path = Path.Combine(STATS_DIRECTORY, path);
            this.valueTitle = valueTitle;
        }

        /// <summary>
        /// Adds a new data point with the specified values.
        /// </summary>
        /// <param name="timestamp">The timestamp of this entry</param>
        /// <param name="animal">The name of the animal type</param>
        /// <param name="value">The data value</param>
        public void AddDataPoint(double timestamp, string animal, T value)
        {
            entries.Add(new AnimalStatsEntry(timestamp, animal, value));
        }

        /// <summary>
        /// Writes the data to the file at the given path in CSV format.
        /// </summary>
        public void WriteToFile()
        {
            if (path == null) return;

            Directory.CreateDirectory(Path.GetDirectoryName(path));
            using (StreamWriter sw = new StreamWriter(File.Create(path)))
            {
                sw.WriteLine("Time,Animal," + valueTitle);
                foreach (var entry in entries) sw.WriteLine(entry.ToString());
            }
        }
    }
}
