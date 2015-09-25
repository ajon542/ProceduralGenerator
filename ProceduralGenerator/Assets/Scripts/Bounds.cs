namespace Dungeon
{
    using UnityEngine;

    /// <summary>
    /// Class to define a bounding region.
    /// </summary>
    public class Bounds
    {
        /// <summary>
        /// The left bound of the region.
        /// </summary>
        public readonly float left;

        /// <summary>
        /// The right bound of the region.
        /// </summary>
        public readonly float right;

        /// <summary>
        /// The bottom bound of the region.
        /// </summary>
        public readonly float bottom;

        /// <summary>
        /// The top bound of the region.
        /// </summary>
        public readonly float top;

        /// <summary>
        /// The width of the region.
        /// </summary>
        public readonly float width;

        /// <summary>
        /// The height of the region.
        /// </summary>
        public readonly float height;

        /// <summary>
        /// Creates a new instance of the <see cref="Bounds"/> class.
        /// </summary>
        public Bounds()
        {
            left = 0;
            right = 0;
            top = 0;
            bottom = 0;

            width = right - left;
            height = top - bottom;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Bounds"/> class.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="bottom"></param>
        /// <param name="top"></param>
        public Bounds(float left, float right, float bottom, float top)
        {
            this.left = left;
            this.right = right;
            this.bottom = bottom;
            this.top = top;

            width = right - left;
            height = top - bottom;
        }

        /// <summary>
        /// Draw some debugging gizmos.
        /// </summary>
        public void Draw()
        {
            Vector3 bottomLeft = new Vector3(left, bottom);
            Vector3 bottomRight = new Vector3(right, bottom);
            Vector3 topRight = new Vector3(right, top);
            Vector3 topLeft = new Vector3(left, top);

            Color originalColor = Gizmos.color;

            Gizmos.color = Color.green;
            Gizmos.DrawLine(bottomLeft, bottomRight);
            Gizmos.DrawLine(bottomRight, topRight);
            Gizmos.DrawLine(topRight, topLeft);
            Gizmos.DrawLine(topLeft, bottomLeft);

            Gizmos.color = originalColor;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Format("[Bounds] l:{0} r:{1} t:{2} b:{3}", left, right, top, bottom);
        }
    }
}