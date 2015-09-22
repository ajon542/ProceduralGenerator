namespace MapBuilder
{
    using UnityEngine;

    public class MapBuilder : MonoBehaviour
    {
        /// <summary>
        /// Width of the map.
        /// </summary>
        public int width = 200;

        /// <summary>
        /// Height of the map.
        /// </summary>
        public int height = 70;

        /// <summary>
        /// Fill percentage for the map.
        /// </summary>
        public int fillPercent = 45;

        /// <summary>
        /// Smoothing count for the map.
        /// </summary>
        public int smooth = 5;

        /// <summary>
        /// Current seed for the random map generation.
        /// </summary>
        public string seed;

        /// <summary>
        /// Whether to use a random seed or the given seed for map generation.
        /// </summary>
        public bool useRandomSeed = true;

        /// <summary>
        /// The map.
        /// </summary>
        private int[,] map;

        /// <summary>
        /// Build the map.
        /// </summary>
        private void Start()
        {
            BuildMap();
        }

        /// <summary>
        /// Build a new map on space key press.
        /// </summary>
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                BuildMap();
            }
        }

        /// <summary>
        /// Build a new map.
        /// </summary>
        public void BuildMap()
        {
            map = new int[width, height];
            RandomFillMap();

            for (int i = 0; i < smooth; ++i)
            {
                map = SmoothMap();
            }

            MeshGenerator meshGen = GetComponent<MeshGenerator>();
            meshGen.GenerateMesh(map, 1);
        }

        /// <summary>
        /// Fill the map with random data.
        /// </summary>
        private void RandomFillMap()
        {
            // Use a random seed.
            if (useRandomSeed)
            {
                seed = Time.time.ToString();
            }
            System.Random rand = new System.Random(seed.GetHashCode());

            // Generate the random data for each cell in the map.
            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    // Set the wall cells to filled.
                    if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                    {
                        map[x, y] = 1;
                    }
                    // Center cells have random data.
                    else
                    {
                        map[x, y] = (rand.Next(0, 100) < fillPercent) ? 1 : 0;
                    }
                }
            }
        }

        /// <summary>
        /// Smooth the map data based on simple rules.
        /// </summary>
        /// <returns>A new, smoother map.</returns>
        private int[,] SmoothMap()
        {
            int[,] newMap = new int[width, height];

            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    // Get neighbouring cell count.
                    int neighbourWallTiles = GetNeighbourCount(x, y);

                    // Set the new map fill based on the previous map data.
                    if (neighbourWallTiles > 4)
                    {
                        newMap[x, y] = 1;
                    }
                    else if (neighbourWallTiles < 4)
                    {
                        newMap[x, y] = 0;
                    }
                    else
                    {
                        newMap[x, y] = map[x, y];
                    }
                }
            }

            return newMap;
        }

        /// <summary>
        /// Get the count of neighbouring cells.
        /// </summary>
        /// <param name="gridX">The map x-location.</param>
        /// <param name="gridY">The map y-location.</param>
        /// <returns>The count of neighbouring cells.</returns>
        private int GetNeighbourCount(int gridX, int gridY)
        {
            int wallCount = 0;
            for (int x = gridX - 1; x <= gridX + 1; ++x)
            {
                for (int y = gridY - 1; y <= gridY + 1; ++y)
                {
                    if (x >= 0 && x < width && y >= 0 && y < height)
                    {
                        if (x != gridX || y != gridY)
                        {
                            wallCount += map[x, y];
                        }
                    }
                    else
                    {
                        wallCount++;
                    }
                }
            }

            return wallCount;
        }

        /// <summary>
        /// Draw a basic representation of the map using gizmos.
        /// </summary>
        private void OnDrawGizmos()
        {
            /*if (map == null)
            {
                return;
            }

            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    Gizmos.color = (map[x, y] == 1) ? Color.black : Color.white;
                    Vector3 pos = new Vector3(-width / 2 + x + 0.5f, -height / 2 + y + 0.5f);
                    Gizmos.DrawCube(pos, Vector3.one);
                }
            }*/
        }
    }
}