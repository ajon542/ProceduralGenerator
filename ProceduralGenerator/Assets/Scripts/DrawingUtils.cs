namespace Dungeon
{
    using UnityEngine;
    using System;
    using System.Collections.Generic;

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

    public class DrawingUtils
    {
        /// <summary>
        /// Get a list of locations representing a line between two locations.
        /// </summary>
        /// <param name="from">The start location of the line.</param>
        /// <param name="to">The end location of the line.</param>
        /// <returns>A list of locations representing a line between two locations.</returns>
        public static List<Location> GetLine(Location from, Location to)
        {
            List<Location> line = new List<Location>();
            int x = from.x;
            int y = from.y;

            int dx = to.x - from.x;
            int dy = to.y - from.y;

            bool inverted = false;
            int step = Math.Sign(dx);
            int gradientStep = Math.Sign(dy);

            int longest = Mathf.Abs(dx);
            int shortest = Mathf.Abs(dy);

            if (longest < shortest)
            {
                inverted = true;
                longest = Mathf.Abs(dy);
                shortest = Mathf.Abs(dx);
                step = Math.Sign(dy);
                gradientStep = Math.Sign(dx);
            }

            int gradientAccumulation = longest / 2;

            for (int i = 0; i < longest; ++i)
            {
                line.Add(new Location(x, y));

                if (inverted)
                {
                    y += step;
                }
                else
                {
                    x += step;
                }

                gradientAccumulation += shortest;
                if (gradientAccumulation >= longest)
                {
                    if (inverted)
                    {
                        x += gradientStep;
                    }
                    else
                    {
                        y += gradientStep;
                    }
                    gradientAccumulation -= longest;
                }
            }

            return line;
        }
    }
}