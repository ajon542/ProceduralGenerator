namespace MapBuilder
{
    using UnityEngine;

    /// <summary>
    /// Component to contain the voxels.
    /// </summary>
    [SelectionBase]
    public class VoxelGrid : MonoBehaviour
    {
        /// <summary>
        /// Resolution of the grid.
        /// </summary>
        /// <example>
        /// For an 8x8 grid, set the resolution to 8.
        /// </example>
        public int resolution;

        /// <summary>
        /// Voxel prefab.
        /// </summary>
        public GameObject voxelPrefab;

        /// <summary>
        /// Voxel state.
        /// </summary>
        private bool[] voxels;

        /// <summary>
        /// Size of the individual voxels.
        /// </summary>
        private float voxelSize;

        /// <summary>
        /// Individual voxel materials for setting the colour based on the voxel state.
        /// </summary>
        private Material[] voxelMaterials;

        /// <summary>
        /// Initialize the voxel grid.
        /// </summary>
        /// <param name="resolution">The resolution of the grid.</param>
        /// <param name="size">The size of the grid.</param>
        public void Initialize(int resolution, float size)
        {
            // Initialize the grid data.
            this.resolution = resolution;
            voxelSize = size / resolution;
            voxels = new bool[resolution * resolution];
            voxelMaterials = new Material[voxels.Length];

            // Create the voxel game objects.
            for (int i = 0, y = 0; y < resolution; y++)
            {
                for (int x = 0; x < resolution; x++, i++)
                {
                    CreateVoxel(i, x, y);
                }
            }

            // Set the colours of the voxels.
            SetVoxelColors();
        }

        /// <summary>
        /// Create a voxel game object.
        /// </summary>
        /// <param name="i">The index of the voxel.</param>
        /// <param name="x">Voxel x-coordinate.</param>
        /// <param name="y">Voxel y-coordinate.</param>
        private void CreateVoxel(int i, int x, int y)
        {
            GameObject o = Instantiate(voxelPrefab);
            o.transform.parent = transform;
            o.transform.localPosition = new Vector3((x + 0.5f) * voxelSize, (y + 0.5f) * voxelSize);
            o.transform.localScale = Vector3.one * voxelSize * 0.9f;
            voxelMaterials[i] = o.GetComponent<MeshRenderer>().material;
        }

        /// <summary>
        /// Set the voxel state and update their colours.
        /// </summary>
        /// <param name="x">Voxel x-coordinate.</param>
        /// <param name="y">Voxel y-coordinate.</param>
        /// <param name="state">The new state of the voxel.</param>
        public void SetVoxel(int x, int y, bool state)
        {
            voxels[y * resolution + x] = state;
            SetVoxelColors();
        }

        /// <summary>
        /// Set the colours of the voxels dpeending on their state.
        /// </summary>
        private void SetVoxelColors()
        {
            for (int i = 0; i < voxels.Length; i++)
            {
                voxelMaterials[i].color = voxels[i] ? Color.black : Color.white;
            }
        }
    }
}