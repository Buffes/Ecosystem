using Ecosystem.ECS.Debugging;
using Unity.Entities;
using UnityEngine;

namespace Ecosystem.Debugging
{
    /// <summary>
    /// Manages debugging systems: what to display and materials to use.
    /// </summary>
    public class VisualDebuggingManager : MonoBehaviour
    {
        [SerializeField]
        private Material hearingDebugMaterial = default;
        [SerializeField]
        private bool hearingDebugShow = default;

        private HearingDebuggingSystem hearingDebuggingSystem;

        private void Awake()
        {
            hearingDebuggingSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<HearingDebuggingSystem>();
        }

        private void Update()
        {
            hearingDebuggingSystem.Material = hearingDebugMaterial;
            hearingDebuggingSystem.Show = hearingDebugShow;
        }

        private void Update2()
        {
            Graphics.DrawMesh(
                    CreateMesh(2f, 2f),
                    new Vector3(),
                    Quaternion.identity,
                    hearingDebugMaterial,
                    0);
        }

        private static Mesh CreateMesh(float width, float height)
        {
            Vector3[] vertices = new Vector3[4];
            Vector2[] uv = new Vector2[4];
            int[] triangles = new int[6];

            float widthHalf = width / 2;
            float heightHalf = height / 2;

            vertices[0] = new Vector3(-widthHalf, heightHalf);
            vertices[1] = new Vector3(widthHalf, heightHalf);
            vertices[2] = new Vector3(-widthHalf, -heightHalf);
            vertices[3] = new Vector3(widthHalf, -heightHalf);

            uv[0] = new Vector2(0, 1);
            uv[1] = new Vector2(1, 1);
            uv[2] = new Vector2(0, 0);
            uv[3] = new Vector2(1, 0);

            triangles[0] = 0;
            triangles[1] = 1;
            triangles[2] = 2;
            triangles[3] = 2;
            triangles[4] = 1;
            triangles[5] = 3;

            Mesh mesh = new Mesh();

            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.triangles = triangles;

            return mesh;
        }
    }
}
