namespace Dungeon
{
    using UnityEngine;

    /// <summary>
    /// A grid of <see cref="ControlSquare"/> objects.
    /// </summary>
    public class ControlSquareGrid
    {
        /// <summary>
        /// The control square grid.
        /// </summary>
        public ControlSquare[,] squares;

        /// <summary>
        /// Create a new instance of the <see cref="ControlSquareGrid"/> class.
        /// </summary>
        /// <param name="map"></param>
        /// <param name="squareSize"></param>
        public ControlSquareGrid(Map map, float squareSize)
        {
            int nodeCountX = map.Width;
            int nodeCountY = map.Height;

            float mapWidth = nodeCountX * squareSize;
            float mapHeight = nodeCountY * squareSize;

            // Setup the position of the control nodes.
            ControlNode[,] controlNodes = new ControlNode[nodeCountX, nodeCountY];

            for (int x = 0; x < nodeCountX; ++x)
            {
                for (int y = 0; y < nodeCountY; ++y)
                {
                    Vector3 position = new Vector3(-mapWidth / 2f + x * squareSize + squareSize / 2f, 0,
                        -mapHeight / 2f + y * squareSize + squareSize / 2);
                    controlNodes[x, y] = new ControlNode(position, map.GetValueAtLocation(x, y) == 1, squareSize);
                }
            }

            // Setup the control squares given each of the four control nodes.
            squares = new ControlSquare[nodeCountX - 1, nodeCountY - 1];

            for (int x = 0; x < nodeCountX - 1; ++x)
            {
                for (int y = 0; y < nodeCountY - 1; ++y)
                {
                    squares[x, y] = new ControlSquare(controlNodes[x, y + 1], controlNodes[x + 1, y + 1],
                        controlNodes[x + 1, y], controlNodes[x, y]);
                }
            }
        }
    }
}
