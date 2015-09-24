namespace MapBuilder
{
    using UnityEngine;
    using System;

    [Serializable]
    public class Voxel
    {
        /// <summary>
        /// The state of the voxel.
        /// </summary>
        public bool state;

        /// <summary>
        /// The position of the voxel.
        /// </summary>
        public Vector2 position;

        /// <summary>
        /// The x-edge position.
        /// </summary>
        public Vector2 xEdgePosition;

        /// <summary>
        /// The y-edge position.
        /// </summary>
        public Vector2 yEdgePosition;

        /// <summary>
        /// Creates a new instance of the <see cref="Voxel"/> class.
        /// </summary>
        public Voxel()
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Voxel"/> class.
        /// </summary>
        /// <param name="x">The x-coordinate of the voxel.</param>
        /// <param name="y">The y-coordinate of the voxel.</param>
        /// <param name="size">The size of the voxel.</param>
        public Voxel(int x, int y, float size)
        {
            position.x = (x + 0.5f) * size;
            position.y = (y + 0.5f) * size;

            xEdgePosition = position;
            xEdgePosition.x += size * 0.5f;
            yEdgePosition = position;
            yEdgePosition.y += size * 0.5f;
        }

        public void BecomeXDummyOf(Voxel voxel, float offset)
        {
            state = voxel.state;
            position = voxel.position;
            xEdgePosition = voxel.xEdgePosition;
            yEdgePosition = voxel.yEdgePosition;
            position.x += offset;
            xEdgePosition.x += offset;
            yEdgePosition.x += offset;
        }

        public void BecomeYDummyOf(Voxel voxel, float offset)
        {
            state = voxel.state;
            position = voxel.position;
            xEdgePosition = voxel.xEdgePosition;
            yEdgePosition = voxel.yEdgePosition;
            position.y += offset;
            xEdgePosition.y += offset;
            yEdgePosition.y += offset;
        }

        public void BecomeXYDummyOf(Voxel voxel, float offset)
        {
            state = voxel.state;
            position = voxel.position;
            xEdgePosition = voxel.xEdgePosition;
            yEdgePosition = voxel.yEdgePosition;
            position.x += offset;
            position.y += offset;
            xEdgePosition.x += offset;
            xEdgePosition.y += offset;
            yEdgePosition.x += offset;
            yEdgePosition.y += offset;
        }
    }
}