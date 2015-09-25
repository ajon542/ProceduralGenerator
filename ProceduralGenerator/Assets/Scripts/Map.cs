
namespace Dungeon
{
    using UnityEngine;
    using System.Collections.Generic;

    /// <summary>
    /// Container for the map representation.
    /// </summary>
    public class Map
    {
        /// <summary>
        /// Gets the width of the map.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// Gets the height of the map.
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// The map representation.
        /// </summary>
        private int[,] map;

        /// <summary>
        /// Creates a new instance of the <see cref="Map"/> class.
        /// </summary>
        /// <param name="width">The width of the map.</param>
        /// <param name="height">The height of the map.</param>
        public Map(int width, int height)
        {
            Width = width;
            Height = height;

            map = new int[Width, Height];
        }

        /// <summary>
        /// Get the value at the given map location.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <returns>The value at the given location; -1 if the location is not within map bounds.</returns>
        public int GetValueAtLocation(Location location)
        {
            // Check if the location is within map bounds.
            if (!IsValidLocation(location))
            {
                return -1;
            }

            return map[location.x, location.y];
        }

        /// <summary>
        /// Get the value at the given map location.
        /// </summary>
        /// <param name="x">The x-location.</param>
        /// <param name="y">The y-location.</param>
        /// <returns>The value at the given location; -1 if the location is not within map bounds.</returns>
        public int GetValueAtLocation(int x, int y)
        {
            // Check if the location is within map bounds.
            if (!IsValidLocation(new Location(x, y)))
            {
                return -1;
            }

            return map[x, y];
        }

        /// <summary>
        /// Set the value at the given map location.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="value">The value to set.</param>
        public void SetValueAtLocation(Location location, int value)
        {
            // Check if the location is within map bounds.
            if (!IsValidLocation(location))
            {
                return;
            }

            map[location.x, location.y] = value;
        }

        /// <summary>
        /// Determines whether the given location is in the map bounds.
        /// </summary>
        /// <param name="location">The location to test.</param>
        /// <returns>true if the locaiton is within map bounds; false otherwise.</returns>
        public bool IsValidLocation(Location location)
        {
            return location.x >= 0 && location.x < Width && location.y >= 0 && location.y < Height;
        }

        /// <summary>
        /// Determines whether the given location is in the map bounds.
        /// </summary>
        /// <param name="x">The x-location.</param>
        /// <param name="y">The y-location.</param>
        /// <returns>true if the locaiton is within map bounds; false otherwise.</returns>
        public bool IsValidLocation(int x, int y)
        {
            return x >= 0 && x < Width && y >= 0 && y < Height;
        }

        /// <summary>
        /// Add a border around the edge of the map.
        /// </summary>
        public void AddBorder()
        {
            // Add wall along the left side.
            List<Location> line = DrawingUtils.GetLine(new Location(0, 0), new Location(0, Height));
            foreach (Location location in line)
            {
                SetValueAtLocation(location, 1);
            }

            // Add wall along the right side.
            line = DrawingUtils.GetLine(new Location(Width - 1, 0), new Location(Width - 1, Height));
            foreach (Location location in line)
            {
                SetValueAtLocation(location, 1);
            }

            // Add wall along the bottom side.
            line = DrawingUtils.GetLine(new Location(0, 0), new Location(Width, 0));
            foreach (Location location in line)
            {
                SetValueAtLocation(location, 1);
            }

            // Add wall along the top side.
            line = DrawingUtils.GetLine(new Location(0, Height - 1), new Location(Width, Height - 1));
            foreach (Location location in line)
            {
                SetValueAtLocation(location, 1);
            }
        }

        /// <summary>
        /// Generate a randomly filled map with filled / non-filled tile status.
        /// </summary>
        public void Randomize(int fillPercentage)
        {
            System.Random rand = new System.Random(Time.time.GetHashCode());

            // Generate the random data for each cell in the map.
            for (int x = 0; x < Width; ++x)
            {
                for (int y = 0; y < Height; ++y)
                {
                    Location location = new Location(x, y);

                    // Set the outer wall cells to filled.
                    if (x == 0 || x == Width - 1 || y == 0 || y == Height - 1)
                    {
                        SetValueAtLocation(location, 1);
                    }
                    // Center cells have random data.
                    else
                    {
                        SetValueAtLocation(location, (rand.Next(0, 100) < fillPercentage) ? 1 : 0);
                    }
                }
            }
        }

        /// <summary>
        /// Smooth the map based on simple cellular automata rules.
        /// </summary>
        /// <returns>A new map resulting from the smoothing rules.</returns>
        public void Smooth()
        {
            int[,] newMap = new int[Width, Height];

            for (int x = 0; x < Width; ++x)
            {
                for (int y = 0; y < Height; ++y)
                {
                    // Get neighbouring cell count.
                    int neighbourWallTiles = GetNeighbourCount(x, y);

                    // Set the new map fill based on the previous map data.
                    if (neighbourWallTiles > 4)
                    {
                        newMap[x, y] = 1;
                    }
                    else if (neighbourWallTiles < 4)
                    {
                        newMap[x, y] = 0;
                    }
                    else
                    {
                        newMap[x, y] = map[x, y];
                    }
                }
            }

            map = newMap;
        }

        /// <summary>
        /// Get the count of neighbouring cells.
        /// </summary>
        /// <param name="gridX">The map x-location.</param>
        /// <param name="gridY">The map y-location.</param>
        /// <returns>The count of neighbouring cells.</returns>
        public int GetNeighbourCount(int gridX, int gridY)
        {
            int wallCount = 0;
            for (int x = gridX - 1; x <= gridX + 1; ++x)
            {
                for (int y = gridY - 1; y <= gridY + 1; ++y)
                {
                    if (IsValidLocation(new Location(x, y)))
                    {
                        if (x != gridX || y != gridY)
                        {
                            wallCount += map[x, y];
                        }
                    }
                    else
                    {
                        wallCount++;
                    }
                }
            }

            return wallCount;
        }
    }
}
