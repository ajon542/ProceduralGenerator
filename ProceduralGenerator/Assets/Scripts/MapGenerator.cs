
namespace Dungeon
{
    using UnityEngine;

    /// <summary>
    /// Map generator for a single level of the dungeon.
    /// </summary>
    /// <remarks>
    /// This class will take care of the map representation. It will
    /// hand off responsibility to another class to handle the mesh
    /// generation.
    /// </remarks>
    public class MapGenerator : MonoBehaviour
    {
        /// <summary>
        /// Width of the map.
        /// </summary>
        public int width = 180;

        /// <summary>
        /// Height of the map.
        /// </summary>
        public int height = 70;

        /// <summary>
        /// The fill percentage for the map.
        /// </summary>
        public int fillPercentage = 45;

        /// <summary>
        /// The map data structure.
        /// </summary>
        private Map map;

        private ControlSquareGrid grid;

        /// <summary>
        /// Start building the map.
        /// </summary>
        private void Start()
        {
            BuildMap();
        }

        /// <summary>
        /// Generate a new map when the space key is pressed.
        /// </summary>
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                BuildMap();
            }
        }

        /// <summary>
        /// Draw a debug representation of the map.
        /// </summary>
        private void OnDrawGizmos()
        {
            /*if (map == null)
            {
                return;
            }

            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    Location location = new Location(x, y);

                    Gizmos.color = (map.GetValueAtLocation(location) == 1) ? Color.black : Color.white;
                    Vector3 pos = new Vector3(-width / 2 + x + 0.5f, -height / 2 + y + 0.5f);
                    Gizmos.DrawCube(pos, Vector3.one);
                }
            }*/

            if (grid == null)
            {
                return;
            }

            grid.Draw();
        }

        /// <summary>
        /// Build the map.
        /// </summary>
        private void BuildMap()
        {
            // Create the map.
            map = new Map(width, height);

            map.Randomize(fillPercentage);

            for (int i = 0; i < 5; ++i)
            {
                map.Smooth();                
            }

            map.AddBorder();

            // Create the control grid from the map.
            grid = new ControlSquareGrid(map, 1);
        }
    }
}