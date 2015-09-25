namespace Dungeon
{
    using UnityEngine;

    /// <summary>
    /// Control node class.
    /// </summary>
    public class ControlNode : MeshNode
    {
        /// <summary>
        /// Whether the control node is active.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// The node above.
        /// </summary>
        public MeshNode Above { get; set; }

        /// <summary>
        /// The node to the right.
        /// </summary>
        public MeshNode Right { get; set; }

        /// <summary>
        /// Creates a new instance of the <see cref="ControlNode"/> class.
        /// </summary>
        /// <param name="position">The position of the control node.</param>
        /// <param name="active">Whether the control node is active.</param>
        /// <param name="squareSize">The size of the square.</param>
        public ControlNode(Vector3 position, bool active, float squareSize)
            : base(position)
        {
            Active = active;
            Above = new MeshNode(position + Vector3.forward * squareSize / 2f);
            Right = new MeshNode(position + Vector3.right * squareSize / 2f);
        }
    }
}