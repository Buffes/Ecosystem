using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ecosystem.Grid
{
    public class MapDisplay : MonoBehaviour
    {
        public Renderer TextureRenderer;
        public MeshFilter meshFilter;
        public MeshRenderer meshRenderer;

        public void DrawTexture(Texture2D texture)
        {
            TextureRenderer.material.mainTexture = texture;
            TextureRenderer.transform.localScale = new Vector3(texture.width, 1, texture.height);
        }

        public void DrawMesh(MeshData meshData, Texture2D texture)
        {
            Mesh mesh = meshData.CreateMesh();
            meshFilter.mesh = mesh;
            meshRenderer.material.mainTexture = texture;
        }
    }

}