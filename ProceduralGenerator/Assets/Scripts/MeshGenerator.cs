
namespace Dungeon
{
    using UnityEngine;
    using System;
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

        /// <summary>
        /// List of nodes in the mesh.
        /// </summary>
        private List<MeshNode> meshNodesList;

        private void Start()
        {
            Mesh mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = mesh;

            vertices = new List<Vector3>();
            triangles = new List<int>();

            meshNodesList = new List<MeshNode>();

            meshNodesList.Add(new MeshNode(new Vector3(0, 0, 0)));
            meshNodesList.Add(new MeshNode(new Vector3(0, 5, 0)));
            meshNodesList.Add(new MeshNode(new Vector3(5, 5, 0)));
            meshNodesList.Add(new MeshNode(new Vector3(5, 0, 0)));

            CreateMeshFromNodes(meshNodesList);

            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.RecalculateNormals();
        }

        /// <summary>
        /// Create a triangle mesh.
        /// </summary>
        /// <param name="a">First meshnode in the triangle.</param>
        /// <param name="b">Second meshnode in the triangle.</param>
        /// <param name="c">Third meshnode in the triangle.</param>
        private void CreateTriangle(MeshNode a, MeshNode b, MeshNode c)
        {
            if (vertices == null || triangles == null)
            {
                throw new Exception("Attempting to create a mesh before initialization");
            }

            // Add the triangle indices.
            triangles.Add(a.VertexIndex);
            triangles.Add(b.VertexIndex);
            triangles.Add(c.VertexIndex);
        }

        /// <summary>
        /// Create a mesh from the list of mesh nodes.
        /// </summary>
        /// <param name="meshNodes">The list of mesh nodes.</param>
        private void CreateMeshFromNodes(List<MeshNode> meshNodes)
        {
            AssignVertices(meshNodes);

            if (meshNodes.Count >= 3)
            {
                CreateTriangle(meshNodes[0], meshNodes[1], meshNodes[2]);
            }
            if (meshNodes.Count >= 4)
            {
                CreateTriangle(meshNodes[0], meshNodes[2], meshNodes[3]);
            }
            if (meshNodes.Count >= 5)
            {
                CreateTriangle(meshNodes[0], meshNodes[3], meshNodes[4]);
            }
            if (meshNodes.Count >= 6)
            {
                CreateTriangle(meshNodes[0], meshNodes[4], meshNodes[5]);
            }
        }

        /// <summary>
        /// Assign vertices to each node with an unassigned vertex index.
        /// </summary>
        /// <param name="meshNodes">The list of mesh nodes.</param>
        private void AssignVertices(List<MeshNode> meshNodes)
        {
            foreach(MeshNode node in meshNodes)
            {
                if (node.VertexIndex == -1)
                {
                    node.VertexIndex = vertices.Count;
                    vertices.Add(node.Position);
                }
            }
        }
    }
}