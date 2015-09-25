namespace Dungeon
{
    /// <summary>
    /// Class defining the bounds for an area.
    /// </summary>
    public class Area
    {
        /// <summary>
        /// Gets or sets the bounds of the area.
        /// </summary>
        public Bounds Bounds { get; set; }

        /// <summary>
        /// Creates a new instance of the <see cref="Area"/> class.
        /// </summary>
        public Area()
        {
            Bounds = new Bounds();
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Area"/> class.
        /// </summary>
        /// <param name="width">The width of the area.</param>
        /// <param name="height">The height of the area.</param>
        public Area(int width, int height)
        {
            Bounds = new Bounds(0, width, 0, height);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Area"/> class.
        /// </summary>
        /// <param name="bounds">The bounds of the area.</param>
        public Area(Bounds bounds)
        {
            Bounds = bounds;
        }

        /// <summary>
        /// Draw some debugging gizmos.
        /// </summary>
        public void Draw()
        {
            Bounds.Draw();
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
            float splitPoint = ((Bounds.top - Bounds.bottom) * percentage / 100.0f) + Bounds.bottom;

            // Create the sub areas with the new bounds.
            areaA = new Area(new Bounds(Bounds.left, Bounds.right, splitPoint, Bounds.top));
            areaB = new Area(new Bounds(Bounds.left, Bounds.right, Bounds.bottom, splitPoint));
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
            float splitPoint = ((Bounds.right - Bounds.left) * percentage / 100.0f) + Bounds.left;

            // Create the sub areas with the new bounds.
            areaA = new Area(new Bounds(Bounds.left, splitPoint, Bounds.bottom, Bounds.top));
            areaB = new Area(new Bounds(splitPoint, Bounds.right, Bounds.bottom, Bounds.top));
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Format("[Area] {0}", Bounds);
        }
    }
}