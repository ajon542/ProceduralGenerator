namespace Dungeon
{
    /// <summary>
    /// Class defining the bounds for an area.
    /// </summary>
    public class Area
    {
        public Bounds bounds;

        /// <summary>
        /// Creates a new instance of the <see cref="Area"/> class.
        /// </summary>
        public Area()
        {
            bounds = new Bounds();
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Area"/> class.
        /// </summary>
        /// <param name="width">The width of the area.</param>
        /// <param name="height">The height of the area.</param>
        public Area(int width, int height)
        {
            bounds = new Bounds(0, width, 0, height);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Area"/> class.
        /// </summary>
        /// <param name="bounds">The bounds of the area.</param>
        public Area(Bounds bounds)
        {
            this.bounds = bounds;
        }

        /// <summary>
        /// Draw some debugging gizmos.
        /// </summary>
        public void Draw()
        {
            bounds.Draw();
        }

        /// <summary>
        /// Split the area along the horizontal axis.
        /// </summary>
        /// <param name="percentage">The split percentage.</param>
        /// <param name="areaA">New area created.</param>
        /// <param name="areaB">New area created.</param>
        public void SplitAlongHorizontalAxis(float percentage, out Area areaA, out Area areaB)
        {
            // Split the current level along the horizontal axis.
            float splitPoint = ((bounds.top - bounds.bottom) * percentage / 100.0f) + bounds.bottom;

            // Create the sub areas with the new bounds.
            areaA = new Area(new Bounds(bounds.left, bounds.right, splitPoint, bounds.top));
            areaB = new Area(new Bounds(bounds.left, bounds.right, bounds.bottom, splitPoint));
        }

        /// <summary>
        /// Split the area along the vertical axis.
        /// </summary>
        /// <param name="percentage">The split percentage.</param>
        /// <param name="areaA">New area created.</param>
        /// <param name="areaB">New area created.</param>
        public void SplitAlongVerticalAxis(float percentage, out Area areaA, out Area areaB)
        {
            // Split the current level along the horizontal axis.
            float splitPoint = ((bounds.right - bounds.left) * percentage / 100.0f) + bounds.left;

            // Create the sub areas with the new bounds.
            areaA = new Area(new Bounds(bounds.left, splitPoint, bounds.bottom, bounds.top));
            areaB = new Area(new Bounds(splitPoint, bounds.right, bounds.bottom, bounds.top));
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Format("[Area] {0}", bounds);
        }
    }
}