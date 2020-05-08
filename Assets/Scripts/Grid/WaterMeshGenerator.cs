using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ecosystem.Grid
{
    public class WaterMeshGenerator : MonoBehaviour
    {
        public float size = 1.0f;
        public int gridSize = 100;
        public float WaterLevel = 0.45f;

        public MeshFilter Filter;
        public MeshRenderer Renderer;

        void Start()
        {
            gridSize = GameZone.NoiseMap.GetLength(0);
        }

        public void DrawWaterMesh(Mesh mesh)
        {
            Filter.mesh = mesh;
        }

        public MeshData GenerateMeshData()
        {
            int width = gridSize;
            int height = gridSize;

            MeshData meshData = new MeshData(width, height);
            int vertexIndex = 0;
            float topLeftZ = (height - 1);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    meshData.vertices[vertexIndex] = new Vector3(x, WaterLevel, y);
                    meshData.uvs[vertexIndex] = new Vector2(x / (float)width, y / (float)height);

                    if (x < width - 1 && y < height - 1)
                    {
                        // Triangles in clockwise order.
                        meshData.AddTriangle(vertexIndex, vertexIndex + width, vertexIndex + 1);
                        meshData.AddTriangle(vertexIndex + width, vertexIndex + width + 1, vertexIndex + 1);
                    }

                    vertexIndex++;
                }
            }

            return meshData;
        
        }

    }
}

