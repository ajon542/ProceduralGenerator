namespace MapBuilder.Tutorials
{
    using UnityEngine;
    using System.Collections.Generic;

    public class FloodFill : MonoBehaviour
    {
        public int width = 200;
        public int height = 70;
        public int fillPercent = 45;
        public int smooth = 5;
        public string seed;
        public bool useRandomSeed = true;
        private int[,] map;

        private void Start()
        {
            BuildMap();
        }

        public void BuildMap()
        {
            map = new int[width, height];
            RandomFillMap();

            for (int i = 0; i < smooth; ++i)
            {
                map = SmoothMap();
            }
        }

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

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                BuildMap();
                Fill(0, 0);
            }
        }

        private void OnDrawGizmos()
        {
            if (map == null)
            {
                return;
            }

            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    Gizmos.color = (map[x, y] == 1) ? Color.black : Color.white;

                    if (map[x, y] == 2)
                    {
                        Gizmos.color = Color.red;
                    }

                    Vector3 pos = new Vector3(-width / 2 + x + 0.5f, -height / 2 + y + 0.5f);
                    Gizmos.DrawCube(pos, Vector3.one);
                }
            }
        }

        
        private void Fill(int x, int y)
        {
            bool[,] visited = new bool[width, height];

            int mapStatus = map[x, y];

            Queue<Location> fillLocations = new Queue<Location>();
            fillLocations.Enqueue(new Location(x, y));

            while (fillLocations.Count > 0)
            {
                Location location = fillLocations.Dequeue();

                // Add top neighbour.
                if ((location.y + 1 < height) && (visited[location.x, location.y + 1] == false) && (map[location.x, location.y + 1] == mapStatus))
                {
                    fillLocations.Enqueue(new Location(location.x, location.y + 1));
                    map[location.x, location.y + 1] = 2;
                }

                // Add bottom location.
                if ((location.y - 1 >= 0) && (visited[location.x, location.y - 1] == false) && (map[location.x, location.y - 1] == mapStatus))
                {
                    fillLocations.Enqueue(new Location(location.x, location.y - 1));
                    map[location.x, location.y - 1] = 2;
                }

                // Add left location.
                if ((location.x - 1 >= 0) && (visited[location.x - 1, location.y] == false) && (map[location.x - 1, location.y] == mapStatus))
                {
                    fillLocations.Enqueue(new Location(location.x - 1, location.y));
                    map[location.x - 1, location.y] = 2;
                }

                // Add right location.
                if ((location.x + 1 < width) && (visited[location.x + 1, location.y] == false) && (map[location.x + 1, location.y] == mapStatus))
                {
                    fillLocations.Enqueue(new Location(location.x + 1, location.y));
                    map[location.x + 1, location.y] = 2;
                }
            }
        }
    }

    class Location
    {
        public readonly int x;
        public readonly int y;

        public Location(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}