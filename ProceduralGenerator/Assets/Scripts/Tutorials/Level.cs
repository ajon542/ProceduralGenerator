namespace MapBuilder.Tutorials
{
    using UnityEngine;
    using System.Collections.Generic;

    public class Room
    {
        public Bounds bounds;

        public Room()
        {
            bounds = new Bounds();
        }

        public Room(int width, int height)
        {
            bounds = new Bounds(0, width, 0, height);
        }

        public Room(Bounds bounds)
        {
            this.bounds = bounds;
        }

        public void Draw()
        {
            bounds.Draw();
        }

        public void SplitAlongHorizontalAxis(float percentage, out Room roomA, out Room roomB)
        {
            // Split the current level along the horizontal axis.
            float splitPoint = ((bounds.top - bounds.bottom) * percentage / 100.0f) + bounds.bottom;

            // Create the sub areas with the new bounds.
            roomA = new Room(new Bounds(bounds.left, bounds.right, splitPoint, bounds.top));
            roomB = new Room(new Bounds(bounds.left, bounds.right, bounds.bottom, splitPoint));
        }

        public void SplitAlongVerticalAxis(float percentage, out Room roomA, out Room roomB)
        {
            // Split the current level along the horizontal axis.
            float splitPoint = ((bounds.right - bounds.left) * percentage / 100.0f) + bounds.left;

            // Create the sub areas with the new bounds.
            roomA = new Room(new Bounds(bounds.left, splitPoint, bounds.bottom, bounds.top));
            roomB = new Room(new Bounds(splitPoint, bounds.right, bounds.bottom, bounds.top));
        }

        public override string ToString()
        {
            return string.Format("[Room] {0}", bounds);
        }
    }

    public class Level : MonoBehaviour
    {
        public int roomCount = 5;

        private System.Random rand = new System.Random();

        private Room[] rooms;

        private int Parent(int i)
        {
            return i >> 1;
        }

        private int Left(int i)
        {
            return i << 1;
        }

        private int Right(int i)
        {
            return (i << 1) + 1;
        }
        
        private void GenerateRooms(int numberOfRooms, int mapWidth, int mapHeight)
        {
            // Tree node formulas
            // n = mi + 1
            // n = i + l
            //
            // l = numberOfRooms
            // m = 2 (binary tree)
            //
            // i = n - l
            // n = 2(n - l) + 1
            // n = 2n - 2l + 1
            // n = 2l - 1
            rooms = new Room[2*numberOfRooms - 1];
            rooms[0] = null;
            rooms[1] = new Room(mapWidth, mapHeight);

            int index = 1;
            Queue<int> queue = new Queue<int>();
            queue.Enqueue(index);

            while (queue.Count > 0)
            {
                index = queue.Dequeue();

                int left = Left(index);
                int right = Right(index);

                Room roomA;
                Room roomB;

                // Split the room
                if (rooms[index].bounds.width > rooms[index].bounds.height)
                {
                    rooms[index].SplitAlongVerticalAxis(rand.Next(30, 70), out roomA, out roomB);
                }
                else
                {
                    rooms[index].SplitAlongHorizontalAxis(rand.Next(30, 70), out roomA, out roomB);
                }

                if (left < rooms.Length)
                {
                    queue.Enqueue(left);
                    rooms[left] = roomA;
                }
                if (right < rooms.Length)
                {
                    queue.Enqueue(right);
                    rooms[right] = roomB;
                }
            }
        }

        private void Start()
        {
            GenerateRooms(roomCount, 200, 200);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GenerateRooms(roomCount, 200, 200);
            }
        }

        private void OnDrawGizmos()
        {
            if (rooms == null)
            {
                return;
            }

            for (int i = 1; i < rooms.Length; ++i)
            {
                rooms[i].Draw();
            }
        }
    }

    public class Bounds
    {
        public readonly float left;
        public readonly float right;
        public readonly float bottom;
        public readonly float top;
        public readonly float width;
        public readonly float height;

        public Bounds()
        {
            left = 0;
            right = 0;
            top = 0;
            bottom = 0;

            width = right - left;
            height = top - bottom;
        }

        public Bounds(float left, float right, float bottom, float top)
        {
            this.left = left;
            this.right = right;
            this.bottom = bottom;
            this.top = top;

            width = right - left;
            height = top - bottom;
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

        public override string ToString()
        {
            return string.Format("[Bounds] l:{0} r:{1} t:{2} b:{3}", left, right, top, bottom);
        }
    }
}