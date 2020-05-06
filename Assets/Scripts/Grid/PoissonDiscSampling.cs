using System.Collections.Generic;
using UnityEngine;

namespace Ecosystem.Grid
{
    public static class PoissonDiscSampling
    {
        public static List<Vector2> GeneratePoisson(float[,] noiseMap, Vector2 sampleRegionSize, int numSamplesBeforeRejection, float min_radius, float max_radius, int numToSpawn, float waterTreshold)
        {
            float grey;
            if(min_radius > max_radius)
            {
                var tmp = max_radius;
                max_radius = min_radius;
                min_radius = tmp;
            }
            float cellSize = max_radius / Mathf.Sqrt(2);

            int[,] grid = new int[Mathf.CeilToInt(sampleRegionSize.x / cellSize), Mathf.CeilToInt(sampleRegionSize.y / cellSize)];
            List<Vector2> points = new List<Vector2>();
            List<Vector2> spawnPoints = new List<Vector2>();
            int toSpawnRemaining = numToSpawn;

            spawnPoints.Add(sampleRegionSize / 2);
            while (spawnPoints.Count > 0)
            {
                if(toSpawnRemaining <= 0)
                {
                    break;
                }
                int spawnIndex = Random.Range(0, spawnPoints.Count);
                Vector2 spawnCentre = spawnPoints[spawnIndex];
                int x = (int)spawnCentre.x;
                int y = (int)spawnCentre.y;
                grey = noiseMap[x, y];
                /*if (grey <= waterTreshold)
                {
                    spawnPoints.RemoveAt(spawnIndex);
                    continue;
                }*/
                grey = Mathf.Clamp(grey, 0.0f, 1.0f);
                float min_dist = min_radius + grey * (max_radius - min_radius);
                min_dist = Mathf.Clamp(min_dist, min_radius, max_radius);
                float radius = min_dist * (Random.value + 1.0f);
                bool candidateAccepted = false;
                for (int i = 0; i < numSamplesBeforeRejection; i++)
                {
                    float angle = Random.value * Mathf.PI * 2;
                    Vector2 dir = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
                    Vector2 candidate = spawnCentre + dir * Random.Range(radius, 2 * radius);
                    if (isValid(candidate, sampleRegionSize, cellSize, radius, points, grid))
                    {
                        points.Add(candidate);
                        spawnPoints.Add(candidate);
                        grid[(int)(candidate.x / cellSize), (int)(candidate.y / cellSize)] = points.Count;
                        candidateAccepted = true;
                        toSpawnRemaining--;
                        break;
                    }
                }
                if (!candidateAccepted)
                {
                    spawnPoints.RemoveAt(spawnIndex);
                }
            }
            return points;
        }

        static bool isValid(Vector2 candidate, Vector2 sampleRegionSize, float cellSize, float radius, List<Vector2> points, int[,] grid)
        {
            if(candidate.x >= 0 && candidate.x < sampleRegionSize.x && candidate.y >= 0 && candidate.y < sampleRegionSize.y)
            {
                int cellX = (int)(candidate.x / cellSize);
                int cellY = (int)(candidate.y / cellSize);
                int searchStartX = Mathf.Max(0, cellX - 2);
                int searchEndX = Mathf.Min(cellX + 2, grid.GetLength(0) - 1);
                int searchStartY = Mathf.Max(0, cellY - 2);
                int searchEndY = Mathf.Min(cellY + 2, grid.GetLength(1) - 1);

                for(int x = searchStartX; x <= searchEndX; x++)
                {
                    for(int y = searchStartY; y <= searchEndY; y++)
                    {
                        int pointIndex = grid[x, y] - 1;
                        if(pointIndex != -1)
                        {
                            float sqrDst = (candidate - points[pointIndex]).sqrMagnitude;
                            if(sqrDst < radius*radius)
                            {
                                return false;
                            }
                        }
                    }
                }
                return true;
            }
            return false;
        }
    }
}
