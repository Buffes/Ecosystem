using UnityEngine;


namespace Ecosystem.Debugging.Noise
{
    public class MapGenerator : MonoBehaviour 
    {
        public int mapWidth;
        public int mapHeight;
        public float mapScale;
        public bool autoUpdate;

        public void GenerateMap()
        {
            float[,] noiseMap = Ecosystem.Grid.Noise.GenerateNoiseMap(mapWidth, mapHeight, mapScale, 1, 1, 1);
        
            MapDisplay display = FindObjectOfType<MapDisplay>();
            display.DrawNoiseMap(noiseMap);
        }
    }
}