namespace Dungeon
{
    using UnityEngine;
    using System;

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
            if (map == null)
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
            }
        }

        /// <summary>
        /// Build the map.
        /// </summary>
        private void BuildMap()
        {
            map = new Map(width, height);

            RandomizeMap();
            SmoothMap();
        }

        /// <summary>
        /// Generate a randomly filled map with filled / non-filled tile status.
        /// </summary>
        private void RandomizeMap()
        {
            System.Random rand = new System.Random(Time.time.GetHashCode());

            // Generate the random data for each cell in the map.
            for (int x = 0; x < map.Width; ++x)
            {
                for (int y = 0; y < map.Height; ++y)
                {
                    Location location = new Location(x, y);

                    // Set the wall cells to filled.
                    if (x == 0 || x == map.Width - 1 || y == 0 || y == map.Height - 1)
                    {
                        map.SetValueAtLocation(location, 1);
                    }
                    // Center cells have random data.
                    else
                    {
                        map.SetValueAtLocation(location, (rand.Next(0, 100) < fillPercentage) ? 1 : 0);
                    }
                }
            }
        }

        /// <summary>
        /// Smooth the map based on simple cellular automata rules.
        /// </summary>
        private void SmoothMap()
        {
            
        }
    }
}