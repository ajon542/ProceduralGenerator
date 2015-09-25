namespace Dungeon
{
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
}
