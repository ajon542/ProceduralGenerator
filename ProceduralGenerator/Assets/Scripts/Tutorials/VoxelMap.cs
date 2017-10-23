namespace MapBuilder.Tutorials
{
    using UnityEngine;

    /// <summary>
    /// Component to manage multiple voxel grids.
    /// </summary>
    /// <remarks>
    /// 
    /// VoxelMap where a, b, c and d are individual VoxelGrids.
    /// --------------------
    /// |        |         |
    /// |   a    |    b    |
    /// |________|_________|
    /// |        |         |
    /// |   c    |    d    |
    /// |        |         |
    /// --------------------
    /// 
    /// </remarks>
    public class VoxelMap : MonoBehaviour
    {
        public float size = 2f;
        public int voxelResolution = 8;
        public int chunkResolution = 2;
        public VoxelGrid voxelGridPrefab;
        private VoxelGrid[] chunks;
        private float chunkSize;
        private float voxelSize;
        private float halfSize;

        private void Awake()
        {
            // Calculate the half size, chunk size and voxel size based on the input.
            halfSize = size * 0.5f;
            chunkSize = size / chunkResolution;
            voxelSize = chunkSize / voxelResolution;

            // Initialize each chunk.
            chunks = new VoxelGrid[chunkResolution * chunkResolution];
            for (int i = 0, y = 0; y < chunkResolution; y++)
            {
                for (int x = 0; x < chunkResolution; x++, i++)
                {
                    CreateChunk(i, x, y);
                }
            }

            // Add a collider to detect user interaction.
            BoxCollider box = gameObject.AddComponent<BoxCollider>();
            box.size = new Vector3(size, size);
        }

        private void CreateChunk(int i, int x, int y)
        {
            VoxelGrid chunk = Instantiate(voxelGridPrefab);
            chunk.Initialize(voxelResolution, chunkSize);
            chunk.transform.parent = transform;
            chunk.transform.localPosition = new Vector3(x * chunkSize - halfSize, y * chunkSize - halfSize);
            chunks[i] = chunk;

            if (x > 0)
            {
                chunks[i - 1].xNeighbor = chunk;
            }
            if (y > 0)
            {
                chunks[i - chunkResolution].yNeighbor = chunk;
                if (x > 0)
                {
                    chunks[i - chunkResolution - 1].xyNeighbor = chunk;
                }
            }
        }

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                RaycastHit hitInfo;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo))
                {
                    if (hitInfo.collider.gameObject == gameObject)
                    {
                        EditVoxels(transform.InverseTransformPoint(hitInfo.point));
                    }
                }
            }
        }

        private void EditVoxels(Vector3 point)
        {
            int centerX = (int)((point.x + halfSize) / voxelSize);
            int centerY = (int)((point.y + halfSize) / voxelSize);

            int xStart = (centerX - 1) / voxelResolution;
            if (xStart < 0)
            {
                xStart = 0;
            }
            int xEnd = (centerX) / voxelResolution;
            if (xEnd >= chunkResolution)
            {
                xEnd = chunkResolution - 1;
            }
            int yStart = (centerY - 1) / voxelResolution;
            if (yStart < 0)
            {
                yStart = 0;
            }
            int yEnd = (centerY) / voxelResolution;
            if (yEnd >= chunkResolution)
            {
                yEnd = chunkResolution - 1;
            }

            VoxelStencil activeStencil = new VoxelStencil();
            activeStencil.Initialize(true, 0);

            int voxelYOffset = yEnd * voxelResolution;
            for (int y = yEnd; y >= yStart; y--)
            {
                int i = y * chunkResolution + xEnd;
                int voxelXOffset = xEnd * voxelResolution;
                for (int x = xEnd; x >= xStart; x--, i--)
                {
                    activeStencil.SetCenter(centerX - voxelXOffset, centerY - voxelYOffset);
                    chunks[i].Apply(activeStencil);
                    voxelXOffset -= voxelResolution;
                }
                voxelYOffset -= voxelResolution;
            }
        }
    }
}