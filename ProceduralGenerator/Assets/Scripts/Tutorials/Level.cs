
namespace MapBuilder.Tutorials
{
    using UnityEngine;
    using System.Collections.Generic;
    using Dungeon;

    public class Level : MonoBehaviour
    {
        /// <summary>
        /// The number of areas in the level.
        /// </summary>
        public int areaCount = 5;
        
        /// <summary>
        /// Array of areas in the level.
        /// </summary>
        /// <remarks>
        /// This is used as a balanced, binary tree.
        /// </remarks>
        private Area[] areas;

        /// <summary>
        /// Random number generator for the area bounds.
        /// </summary>
        private System.Random rand = new System.Random();

        /// <summary>
        /// Obtain the parent index of a child.
        /// </summary>
        /// <param name="i">The child index.</param>
        /// <returns>The parent index of a child.</returns>
        private int Parent(int i)
        {
            return i >> 1;
        }

        /// <summary>
        /// Obtain the left child index of a parent.
        /// </summary>
        /// <param name="i">The parent index.</param>
        /// <returns>The left child index of a parent.</returns>
        private int Left(int i)
        {
            return i << 1;
        }

        /// <summary>
        /// Obtain the right child index of a parent.
        /// </summary>
        /// <param name="i">The parent index.</param>
        /// <returns>The right child index of a parent.</returns>
        private int Right(int i)
        {
            return (i << 1) + 1;
        }
        
        /// <summary>
        /// Generate the different areas in the level.
        /// </summary>
        /// <param name="numberOfAreas">The number of areas in the level.</param>
        /// <param name="levelWidth">The width of the level.</param>
        /// <param name="levelHeight">The height of the level.</param>
        private void GenerateLevelAreas(int numberOfAreas, int levelWidth, int levelHeight)
        {
            // Tree node formulas
            // n = mi + 1
            // n = i + l
            //
            // l = numberOfAreas
            // m = 2 (binary tree)
            //
            // i = n - l
            // n = 2(n - l) + 1
            // n = 2n - 2l + 1
            // n = 2l - 1
            areas = new Area[2 * numberOfAreas - 1];
            areas[0] = null;
            areas[1] = new Area(levelWidth, levelHeight);

            int index = 1;
            Queue<int> queue = new Queue<int>();
            queue.Enqueue(index);

            // Set the area of each of the child rooms.
            while (queue.Count > 0)
            {
                index = queue.Dequeue();

                int left = Left(index);
                int right = Right(index);

                Area areaA;
                Area areaB;

                // Split the room based on the width and height parameters.
                if (areas[index].Bounds.width > areas[index].Bounds.height)
                {
                    areas[index].SplitAlongVerticalAxis(rand.Next(30, 70), out areaA, out areaB);
                }
                else
                {
                    areas[index].SplitAlongHorizontalAxis(rand.Next(30, 70), out areaA, out areaB);
                }

                // Set the new area bounds.
                if (left < areas.Length)
                {
                    queue.Enqueue(left);
                    areas[left] = areaA;
                }
                if (right < areas.Length)
                {
                    queue.Enqueue(right);
                    areas[right] = areaB;
                }
            }
        }

        /// <summary>
        /// Generate the initial level areas.
        /// </summary>
        private void Start()
        {
            GenerateLevelAreas(areaCount, 200, 200);
        }

        /// <summary>
        /// Generate a new level area when the space key is pressed.
        /// </summary>
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GenerateLevelAreas(areaCount, 200, 200);
            }
        }

        /// <summary>
        /// Draw some debugging gizmos.
        /// </summary>
        private void OnDrawGizmos()
        {
            if (areas == null)
            {
                return;
            }

            for (int i = 1; i < areas.Length; ++i)
            {
                areas[i].Draw();
            }
        }
    }
}