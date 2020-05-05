using UnityEngine;

namespace Ecosystem.Grid
{
    
    public class MeshCollision : MonoBehaviour
    {
        public MeshCollider meshCollider;

        public void SetMeshCollider(MeshData meshData)
        {
            Mesh mesh = meshData.CreateMesh();
            meshCollider.sharedMesh = mesh;
        }
    }
}