namespace MapBuilder.Tutorials
{
    using UnityEngine;
    using System.Collections.Generic;

    public class Level : MonoBehaviour
    {
        public int width = 100;
        public int height = 100;

        private LevelArea root;

        private void Start()
        {
            // left, right, bottom, top.
            root = new LevelArea(new Bounds(0, width, 0, height));

            root.SplitAlongHorizontalAxis(50);
            root.area1.SplitAlongVerticalAxis(60);
            root.area2.SplitAlongVerticalAxis(30);
        }

        private void OnDrawGizmos()
        {
            if (root == null)
            {
                return;
            }

            root.Draw();
        }
    }

    public class Bounds
    {
        public readonly float left;
        public readonly float right;
        public readonly float bottom;
        public readonly float top;

        public Bounds(float left, float right, float bottom, float top)
        {
            this.left = left;
            this.right = right;
            this.bottom = bottom;
            this.top = top;
        }

        public void Draw()
        {
            Vector3 bottomLeft = new Vector3(left, bottom);
            Vector3 bottomRight = new Vector3(right, bottom);
            Vector3 topRight = new Vector3(right, top);
            Vector3 topLeft = new Vector3(left, top);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(bottomLeft, bottomRight);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(bottomRight, topRight);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(topRight, topLeft);
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(topLeft, bottomLeft);
        }
    }

    /// <summary>
    /// LevelArea by default starts out as a leaf node.
    /// When we call split, it will divide itself into two regions which then become the leaf nodes.
    /// </summary>
    public class LevelArea
    {
        public LevelArea area1;
        public LevelArea area2;

        public Bounds bounds;

        public LevelArea(Bounds bounds)
        {
            this.bounds = bounds;
        }

        public void SplitAlongHorizontalAxis(float percentage)
        {
            // Split the current level along the horizontal axis.
            float splitPoint = (bounds.top + bounds.bottom) * percentage / 100.0f;

            // Create the sub areas with the new bounds.
            area1 = new LevelArea(new Bounds(bounds.left, bounds.right, splitPoint, bounds.top));
            area2 = new LevelArea(new Bounds(bounds.left, bounds.right, bounds.bottom, splitPoint));
        }

        public void SplitAlongVerticalAxis(float percentage)
        {
            // Split the current level along the horizontal axis.
            float splitPoint = (bounds.left + bounds.right) * percentage / 100.0f;

            // Create the sub areas with the new bounds.
            area1 = new LevelArea(new Bounds(bounds.left, splitPoint, bounds.bottom, bounds.top));
            area2 = new LevelArea(new Bounds(splitPoint, bounds.right, bounds.bottom, bounds.top));
        }

        public void Draw()
        {
            bounds.Draw();
            if (area1 != null)
            {
                area1.Draw();
            }
            if (area2 != null)
            {
                area2.Draw();
            }
        }
    }
}