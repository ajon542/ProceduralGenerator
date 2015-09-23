
using System;

namespace MapBuilder
{
    using UnityEngine;
    using System.Collections.Generic;

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

            ProcessMap();

            // Generate a border.
            int borderSize = 5;
            int[,] borderedMap = new int[width + borderSize * 2, height + borderSize * 2];
            for (int x = 0; x < borderedMap.GetLength(0); ++x)
            {
                for (int y = 0; y < borderedMap.GetLength(1); ++y)
                {
                    if (x >= borderSize && x < width + borderSize && y >= borderSize && y < height + borderSize)
                    {
                        borderedMap[x, y] = map[x - borderSize, y - borderSize];
                    }
                    else
                    {
                        borderedMap[x, y] = 1;
                    }
                }
            }

            MeshGenerator meshGen = GetComponent<MeshGenerator>();
            meshGen.GenerateMesh(borderedMap, 1);
        }

        private bool IsInMapRange(int x, int y)
        {
            return x >= 0 && x < width && y >= 0 && y < height;
        }

        private List<Coord> GetRegionTiles(int startX, int startY)
        {
            int tileType = map[startX, startY];
            bool[,] visited = new bool[width, height];

            List<Coord> tiles = new List<Coord>();

            Queue<Coord> queue = new Queue<Coord>();
            queue.Enqueue(new Coord(startX, startY));
            visited[startX, startY] = true;

            while (queue.Count > 0)
            {
                Coord tile = queue.Dequeue();
                tiles.Add(tile);

                for (int x = tile.x - 1; x <= tile.x + 1; ++x)
                {
                    for (int y = tile.y - 1; y <= tile.y + 1; ++y)
                    {
                        if (IsInMapRange(x, y) && (y == tile.y || x == tile.x))
                        {
                            if (visited[x, y] == false && map[x, y] == tileType)
                            {
                                visited[x, y] = true;
                                queue.Enqueue(new Coord(x, y));
                            }
                        }
                    }
                }
            }

            return tiles;
        }

        private List<List<Coord>> GetRegions(int tileType)
        {
            List<List<Coord>> regions = new List<List<Coord>>();
            bool[,] visited = new bool[width, height];

            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    if (visited[x, y] == false && map[x, y] == tileType)
                    {
                        List<Coord> newRegion = GetRegionTiles(x, y);
                        regions.Add(newRegion);

                        foreach (Coord tile in newRegion)
                        {
                            visited[tile.x, tile.y] = true;
                        }
                    }
                }
            }

            return regions;
        }

        private void ProcessMap()
        {
            List<List<Coord>> wallRegions = GetRegions(1);

            // Remove wall regions.
            // Any region made up of less than the threshold will be removed from the map.
            int wallThresholdSize = 50;
            foreach (List<Coord> wallRegion in wallRegions)
            {
                if (wallRegion.Count < wallThresholdSize)
                {
                    foreach (Coord tile in wallRegion)
                    {
                        map[tile.x, tile.y] = 0;
                    }
                }
            }

            // Remove room regions.
            // Any region made up of less than the threshold will be removed from the map.
            List<List<Coord>> roomRegions = GetRegions(0);
            int roomThresholdSize = 50;

            List<Room> survivingRooms = new List<Room>();

            foreach (List<Coord> roomRegion in roomRegions)
            {
                if (roomRegion.Count < roomThresholdSize)
                {
                    foreach (Coord tile in roomRegion)
                    {
                        map[tile.x, tile.y] = 1;
                    }
                }
                else
                {
                    survivingRooms.Add(new Room(roomRegion, map));
                }
            }

            // Room graph must be connected.
            survivingRooms.Sort();
            survivingRooms[0].isMainRoom = true;
            survivingRooms[0].isAccessibleFromMainRoom = true;
            ConnectClosestRooms(survivingRooms);
        }

        private void ConnectClosestRooms(List<Room> allRooms, bool forceAccessibilityFromMainRoom = false)
        {
            List<Room> roomListA = new List<Room>();
            List<Room> roomListB = new List<Room>();

            // Try to connect all the rooms in listA which aren't accessible from the main room
            // to any of the rooms in listB which are accessible from the main room.
            if (forceAccessibilityFromMainRoom)
            {
                foreach (Room room in allRooms)
                {
                    if (room.isAccessibleFromMainRoom)
                    {
                        roomListB.Add(room);
                    }
                    else
                    {
                        roomListA.Add(room);
                    }
                }
            }
            else
            {
                roomListA = allRooms;
                roomListB = allRooms;
            }

            int bestDistance = 0;
            Coord bestTileA = new Coord();
            Coord bestTileB = new Coord();

            Room bestRoomA = new Room();
            Room bestRoomB = new Room();

            bool possibleConnectionFound = false;
            
            // TODO: This has to be addressed in a more efficient manner...
            foreach (Room roomA in roomListA)
            {
                if (!forceAccessibilityFromMainRoom)
                {
                    possibleConnectionFound = false;
                    if (roomA.connectedRooms.Count > 0)
                    {
                        continue;
                    }
                }

                foreach (Room roomB in roomListB)
                {
                    if (roomA == roomB || roomA.IsConnected(roomB))
                    {
                        continue;
                    }

                    for (int tileIndexA = 0; tileIndexA < roomA.edgeTiles.Count; ++tileIndexA)
                    {
                        for (int tileIndexB = 0; tileIndexB < roomB.edgeTiles.Count; ++tileIndexB)
                        {
                            Coord tileA = roomA.edgeTiles[tileIndexA];
                            Coord tileB = roomB.edgeTiles[tileIndexB];
                            int distanceBetweenRooms = (int)(Mathf.Pow(tileA.x - tileB.x, 2) + Mathf.Pow(tileA.y - tileB.y, 2));

                            if (distanceBetweenRooms < bestDistance || !possibleConnectionFound)
                            {
                                bestDistance = distanceBetweenRooms;
                                possibleConnectionFound = true;
                                bestTileA = tileA;
                                bestTileB = tileB;
                                bestRoomA = roomA;
                                bestRoomB = roomB;
                            }
                        }
                    }
                }

                if (possibleConnectionFound && !forceAccessibilityFromMainRoom)
                {
                    CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
                }
            }

            if (possibleConnectionFound && forceAccessibilityFromMainRoom)
            {
                CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
                ConnectClosestRooms(allRooms, true);
            }

            // Any rooms that are not connected to the main room, are forced to find a connection.
            if (!forceAccessibilityFromMainRoom)
            {
                ConnectClosestRooms(allRooms, true);
            }
        }

        void CreatePassage(Room roomA, Room roomB, Coord tileA, Coord tileB)
        {
            Room.ConnectRooms(roomA, roomB);
            Debug.DrawLine(CoordToWorldPoint(tileA), CoordToWorldPoint(tileB), Color.green, 100);
        }

        private Vector3 CoordToWorldPoint(Coord tile)
        {
            return new Vector3(-width / 2.0f + 0.5f + tile.x, 2, -height / 2.0f + 0.5f + tile.y);
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

        struct Coord
        {
            public readonly int x;
            public readonly int y;

            public Coord(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
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
                    if (IsInMapRange(x, y))
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

        class Room : IComparable<Room>
        {
            public List<Coord> tiles;
            public List<Coord> edgeTiles;
            public List<Room> connectedRooms;
            public int roomSize;
            public bool isAccessibleFromMainRoom;
            public bool isMainRoom;

            public Room()
            {
            }

            public Room(List<Coord> roomTiles, int[,] map)
            {
                tiles = roomTiles;
                roomSize = tiles.Count;
                connectedRooms = new List<Room>();
                edgeTiles = new List<Coord>();

                // Find the edge tiles for the room.
                foreach (Coord tile in tiles)
                {
                    for (int x = tile.x - 1; x <= tile.x + 1; ++x)
                    {
                        for (int y = tile.y - 1; y <= tile.y + 1; ++y)
                        {
                            if(x == tile.x || y == tile.y)
                            {
                                if (map[x, y] == 1)
                                {
                                    edgeTiles.Add(tile);
                                }
                            }
                        }
                    }
                }
            }

            public void SetAccessibleFromMainRoom()
            {
                if (!isAccessibleFromMainRoom)
                {
                    isAccessibleFromMainRoom = true;
                    foreach (Room conectedRoom in connectedRooms)
                    {
                        conectedRoom.SetAccessibleFromMainRoom();
                    }
                }
            }

            public static void ConnectRooms(Room roomA, Room roomB)
            {
                if (roomA.isAccessibleFromMainRoom)
                {
                    roomB.SetAccessibleFromMainRoom();
                }
                else if(roomB.isAccessibleFromMainRoom)
                {
                    roomA.SetAccessibleFromMainRoom();
                }
                roomA.connectedRooms.Add(roomB);
                roomB.connectedRooms.Add(roomA);
            }

            public bool IsConnected(Room otherRoom)
            {
                return connectedRooms.Contains(otherRoom);
            }

            public int CompareTo(Room otherRoom)
            {
                return otherRoom.roomSize.CompareTo(roomSize);
            }
        }

    }
}