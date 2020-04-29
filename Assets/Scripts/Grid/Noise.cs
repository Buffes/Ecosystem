using UnityEngine;

namespace Ecosystem.Grid
{
    public static class Noise 
    {
        /// <summary>
        /// Generates a noise map based on Perlin noise and the specified parameters.
        /// </summary>
        /// <param name="width">The width of the map.</param>
        /// <param name="height">The height of the map.</param>
        /// <param name="scale">The scale of the map. At 1 this will return a map with the same value everywhere.</param>
        /// <param name="octaves"></param>
        /// <param name="persistence"></param>
        /// <param name="lacunarity"></param>
        /// <returns></returns>
        public static float[,] GenerateNoiseMap(int width, int height, float scale, int octaves, float persistence, float lacunarity)
        {
            if (scale <= 0f)
            {
                scale = 0.0001f; // Avoid division by 0.
            }
            float[,] noiseMap = new float[width, height]; 
        
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float amplitude = 1;
                    float frequency = 1;
                    float noiseHeight = 0;

                    for (int i = 0; i < octaves; i++)
                    {        
                        float yInput = y / scale * frequency;
                        float xInput = x / scale * frequency;

                        float perlinValue = Mathf.PerlinNoise(xInput, yInput);
                        noiseHeight += perlinValue * amplitude;

                        amplitude *= persistence;
                        frequency *= lacunarity;
                    }
                    noiseMap[x,y] = noiseHeight;
                }            
            }

            return noiseMap;
        }
    }
}