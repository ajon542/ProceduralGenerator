
namespace Dungeon
{
    using System;

    /// <summary>
    /// Container for a location on the map.
    /// </summary>
    public class Location
    {
        /// <summary>
        /// The x-coordinate.
        /// </summary>
        public readonly int x;

        /// <summary>
        /// The y-coordinate.
        /// </summary>
        public readonly int y;

        /// <summary>
        /// Creates a new instance of the <see cref="Location"/> class.
        /// </summary>
        /// <param name="x">The x-coordinate.</param>
        /// <param name="y">The y-coordinate.</param>
        public Location(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    /// <summary>
    /// Container for the map representation.
    /// </summary>
    [Serializable]
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
    }
}