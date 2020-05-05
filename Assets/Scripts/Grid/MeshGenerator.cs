using UnityEngine;

namespace Ecosystem.Grid
{
    public static class MeshGenerator
    {
        public static MeshData GenerateTerrainMesh(float[,] heightMap)
        {
            int width = heightMap.GetLength(0);
            int height = heightMap.GetLength(1);
            
            MeshData meshData = new MeshData(width, height);
            int vertexIndex = 0;
            float topLeftZ = (height - 1);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    meshData.vertices[vertexIndex] = new Vector3(x, 3f*heightMap[x,y], y);
                    meshData.uvs[vertexIndex] = new Vector2(x/(float)width, y/(float)height);
                    
                    if (x < width - 1 && y < height - 1)
                    {
                        // Triangles in clockwise order.
                        meshData.AddTriangle(vertexIndex, vertexIndex+width, vertexIndex+1);
                        meshData.AddTriangle(vertexIndex+width, vertexIndex+width+1, vertexIndex+1);
                    }

                    vertexIndex++;
                }
            }

            return meshData;
        }
    }

    public struct MeshData
    {
        public Vector3[] vertices;
        public int[] triangles;
        public Vector2[] uvs;
        
        private int triangleIndex;

        public MeshData(int width, int height)
        {
            vertices = new Vector3[width * height];
            uvs = new Vector2[width * height];
            triangles = new int[(width-1)*(height-1)*6];
            triangleIndex = 0;
        }

        public void AddTriangle(int a, int b, int c)
        {
            triangles[triangleIndex] = a;
            triangles[triangleIndex+1] = b;
            triangles[triangleIndex+2] = c;

            triangleIndex += 3;
        }

        public Mesh CreateMesh()
        {
            Mesh mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uvs;
            mesh.RecalculateNormals();
            return mesh;
        }
    }
}