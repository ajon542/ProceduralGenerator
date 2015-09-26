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
        public ControlSquare[,] grid;

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
            grid = new ControlSquare[nodeCountX - 1, nodeCountY - 1];

            for (int x = 0; x < nodeCountX - 1; ++x)
            {
                for (int y = 0; y < nodeCountY - 1; ++y)
                {
                    grid[x, y] = new ControlSquare(controlNodes[x, y + 1], controlNodes[x + 1, y + 1],
                        controlNodes[x + 1, y], controlNodes[x, y]);
                }
            }
        }

        /// <summary>
        /// Draw debug gizmos.
        /// </summary>
        public void Draw()
        {
            if (grid == null)
            {
                return;
            }

            for (int x = 0; x < grid.GetLength(0); ++x)
            {
                for (int y = 0; y < grid.GetLength(1); ++y)
                {
                    Gizmos.color = (grid[x, y].TopLeft.Active) ? Color.black : Color.white;
                    Gizmos.DrawCube(grid[x, y].TopLeft.Position, Vector3.one * 0.4f);

                    Gizmos.color = (grid[x, y].TopRight.Active) ? Color.black : Color.white;
                    Gizmos.DrawCube(grid[x, y].TopRight.Position, Vector3.one * 0.4f);

                    Gizmos.color = (grid[x, y].BottomLeft.Active) ? Color.black : Color.white;
                    Gizmos.DrawCube(grid[x, y].BottomLeft.Position, Vector3.one * 0.4f);

                    Gizmos.color = (grid[x, y].BottomRight.Active) ? Color.black : Color.white;
                    Gizmos.DrawCube(grid[x, y].BottomRight.Position, Vector3.one * 0.4f);

                    Gizmos.color = Color.grey;
                    Gizmos.DrawCube(grid[x, y].CenterTop.Position, Vector3.one * 0.15f);
                    Gizmos.DrawCube(grid[x, y].CenterBottom.Position, Vector3.one * 0.15f);
                    Gizmos.DrawCube(grid[x, y].CenterLeft.Position, Vector3.one * 0.15f);
                    Gizmos.DrawCube(grid[x, y].CenterRight.Position, Vector3.one * 0.15f);
                }
            }
        }
    }
}
