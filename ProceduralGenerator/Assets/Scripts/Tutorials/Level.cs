
namespace MapBuilder.Tutorials
{
    using UnityEngine;
    using System.Collections.Generic;
    using Dungeon;

    public class Level : MonoBehaviour
    {
        private const int levelWidth = 200;
        private const int levelHeight = 200;
        private const int minSplit = 30;
        private const int maxSplit = 70;
        private const int treeRoot = 1;

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
        /// <remarks>
        /// The general algorithm is as follows:
        /// 1. Specify the width and height of the level, this becomes the overall level area.
        /// 2. Split the area into two parts either horizontally or vertically.
        /// 3. Store the two new areas as children of the parent area.
        /// 4. Repeat the process until we have the desired number of areas.
        /// </remarks>
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
            // The way this works is areas[1] contains the overall area of the level.
            // As we add the children to the areas array, the sum of the two children
            // areas will equal the area of the parent.
            areas[treeRoot] = new Area(levelWidth, levelHeight);

            int index = treeRoot;
            Queue<int> queue = new Queue<int>();
            queue.Enqueue(index);

            // Set the area of each of the child rooms.
            while (queue.Count > 0)
            {
                // Get the parent index.
                index = queue.Dequeue();

                // Calculate the index of the two children.
                int leftChild = Left(index);
                int rightChild = Right(index);

                // Generate the areas.
                Area areaA;
                Area areaB;

                // Split the room based on the width and height parameters.
                if (areas[index].Bounds.width > areas[index].Bounds.height)
                {
                    int splitPoint = rand.Next(minSplit, maxSplit);
                    areas[index].SplitAlongVerticalAxis(splitPoint, out areaA, out areaB);
                }
                else
                {
                    int splitPoint = rand.Next(minSplit, maxSplit);
                    areas[index].SplitAlongHorizontalAxis(splitPoint, out areaA, out areaB);
                }

                // Set the new area bounds.
                if (leftChild < areas.Length)
                {
                    queue.Enqueue(leftChild);
                    areas[leftChild] = areaA;
                }
                if (rightChild < areas.Length)
                {
                    queue.Enqueue(rightChild);
                    areas[rightChild] = areaB;
                }
            }
        }

        /// <summary>
        /// Generate the initial level areas.
        /// </summary>
        private void Start()
        {
            GenerateLevelAreas(areaCount, levelWidth, levelHeight);
        }

        /// <summary>
        /// Generate a new level area when the space key is pressed.
        /// </summary>
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GenerateLevelAreas(areaCount, levelWidth, levelHeight);
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