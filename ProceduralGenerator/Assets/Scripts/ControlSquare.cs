namespace Dungeon
{
    /// <summary>
    /// Container for four <see cref="ControlNode"/> to assist in the
    /// marching square algorithm.
    /// </summary>
    /// <remarks>
    /// 
    /// [ ] - Control node
    ///  o  - Dividing mesh node
    ///  
    /// 
    /// [ ]   o   [ ]
    /// 
    ///  o         o
    /// 
    /// [ ]   o   [ ]
    /// 
    /// </remarks>
    public class ControlSquare
    {
        /// <summary>
        /// Gets the top left control node.
        /// </summary>
        public ControlNode TopLeft { get; private set; }

        /// <summary>
        /// Gets the top right control node.
        /// </summary>
        public ControlNode TopRight { get; private set; }

        /// <summary>
        /// Gets the bottom right control node.
        /// </summary>
        public ControlNode BottomRight { get; private set; }

        /// <summary>
        /// Gets the bottom left control node.
        /// </summary>
        public ControlNode BottomLeft { get; private set; }

        /// <summary>
        /// Gets the center top dividing node.
        /// </summary>
        public MeshNode CenterTop { get; private set; }

        /// <summary>
        /// Gets the center right dividing node.
        /// </summary>
        public MeshNode CenterRight { get; private set; }

        /// <summary>
        /// Gets the center bottom dividing node.
        /// </summary>
        public MeshNode CenterBottom { get; private set; }

        /// <summary>
        /// Gets the center left dividing node.
        /// </summary>
        public MeshNode CenterLeft { get; private set; }

        /// <summary>
        /// Gets the configuration of the control node.
        /// </summary>
        /// <remarks>
        /// As per the basic marching square algorithm, there are 16
        /// configurations of the control square.
        /// </remarks>
        public int Configuration { get; private set; }

        /// <summary>
        /// Creates a new instance of the <see cref="ControlSquare"/> class.
        /// </summary>
        /// <param name="topLeft">The top left control node.</param>
        /// <param name="topRight">The top right control node.</param>
        /// <param name="bottomRight">The bottom right control node.</param>
        /// <param name="bottomLeft">The bottom left control node.</param>
        public ControlSquare(ControlNode topLeft, ControlNode topRight, ControlNode bottomRight, ControlNode bottomLeft)
        {
            // Set the control nodes.
            TopLeft = topLeft;
            TopRight = topRight;
            BottomRight = bottomRight;
            BottomLeft = bottomLeft;

            // Set the dividing mesh nodes.
            CenterTop = topLeft.Right;
            CenterRight = bottomRight.Above;
            CenterBottom = bottomLeft.Right;
            CenterLeft = bottomLeft.Above;

            // Determine the configuration of this control square.
            if (topLeft.Active)
            {
                Configuration += 8;
            }
            if (topRight.Active)
            {
                Configuration += 4;
            }
            if (bottomRight.Active)
            {
                Configuration += 2;
            }
            if (bottomLeft.Active)
            {
                Configuration += 1;
            }
        }
    }
}