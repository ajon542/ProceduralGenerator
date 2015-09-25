namespace Dungeon
{
    using UnityEngine;

    /// <summary>
    /// Container for a vertex position and triangle index.
    /// </summary>
    public class MeshNode
    {
        /// <summary>
        /// Gets or sets the vertex position.
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// Gets or sets the vertex index for the triangles.
        /// </summary>
        public int VertexIndex { get; set; }

        /// <summary>
        /// Creates a new instance of the <see cref="MeshNode"/> class.
        /// </summary>
        public MeshNode()
        {
            Position = new Vector3();
            VertexIndex = -1;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="MeshNode"/> class.
        /// </summary>
        /// <param name="position">The vertex position.</param>
        public MeshNode(Vector3 position)
        {
            Position = position;
            VertexIndex = -1;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="MeshNode"/> class.
        /// </summary>
        /// <param name="position">The vertex position.</param>
        /// <param name="vertexIndex">The vertex index.</param>
        public MeshNode(Vector3 position, int vertexIndex)
        {
            Position = position;
            VertexIndex = vertexIndex;
        }
    }
}