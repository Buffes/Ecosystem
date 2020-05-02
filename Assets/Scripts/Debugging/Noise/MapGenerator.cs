using UnityEngine;


namespace Ecosystem.Debugging.Noise
{
    public class MapGenerator : MonoBehaviour 
    {
        public int MapWidth;
        public int MapHeight;
        public float MapScale;
        public int Octaves;
        public float Persistence;
        public float Lacunarity;
        public int Seed;
        public bool AutoUpdate;

        public void GenerateMap()
        {
            float[,] noiseMap = Ecosystem.Grid.Noise.GenerateNoiseMap(MapWidth, MapHeight, Seed, MapScale, Octaves, Persistence, Lacunarity);
        
            MapDisplay display = FindObjectOfType<MapDisplay>();
            display.DrawNoiseMap(noiseMap);
        }
    }
}