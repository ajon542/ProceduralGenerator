
namespace Dungeon
{
    using UnityEngine;
    using System.Collections.Generic;

    /// <summary>
    /// Mesh generator class.
    /// </summary>
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class MeshGenerator : MonoBehaviour
    {
        /// <summary>
        /// The list of vertices for the mesh.
        /// </summary>
        private List<Vector3> vertices;

        /// <summary>
        /// The triangle indices for the mesh.
        /// </summary>
        private List<int> triangles;

        private void Start()
        {
            Mesh mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = mesh;

            vertices = new List<Vector3> { new Vector3(0, 0, 0), new Vector3(5, 5, 0), new Vector3(5, 0, 0) };
            triangles = new List<int> { 0, 1, 2 };

            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.RecalculateNormals();
        }
    }
}