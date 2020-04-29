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
            float[,] noiseMap = Ecosystem.Grid.Noise.GenerateNoiseMap(mapWidth, mapHeight, 0, mapScale, 1, .5f, 2);
        
            MapDisplay display = FindObjectOfType<MapDisplay>();
            display.DrawNoiseMap(noiseMap);
        }
    }
}