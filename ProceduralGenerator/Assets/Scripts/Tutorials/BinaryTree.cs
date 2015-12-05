using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MapBuilder.Tutorials
{
    /// <summary>
    /// Implementation of a balanced binary tree.
    /// </summary>
    public class BalancedBinaryTree
    {
        private const int treeRoot = 1;
        private List<int> tree = new List<int>();

        public BalancedBinaryTree()
        {
            tree.Add(0);
        }

        public int Insert(int element)
        {
            // insert the element into the tree in no particular order
            // it will be added to the end of the list
            // returns the index of the element
            return 0;
        }

        public int Remove()
        {
            // remove the last element from the tree
            // returns the index of the element
            return 0;
        }




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
    }
}
