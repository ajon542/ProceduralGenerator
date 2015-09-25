
namespace MapBuilder.Tutorials
{
    using UnityEngine;
    using System.Collections.Generic;
    using Dungeon;

    public class Level : MonoBehaviour
    {
        public int roomCount = 5;

        private System.Random rand = new System.Random();

        private Area[] areas;

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
            areas = new Area[2 * numberOfRooms - 1];
            areas[0] = null;
            areas[1] = new Area(mapWidth, mapHeight);

            int index = 1;
            Queue<int> queue = new Queue<int>();
            queue.Enqueue(index);

            while (queue.Count > 0)
            {
                index = queue.Dequeue();

                int left = Left(index);
                int right = Right(index);

                Area areaA;
                Area areaB;

                // Split the room
                if (areas[index].bounds.width > areas[index].bounds.height)
                {
                    areas[index].SplitAlongVerticalAxis(rand.Next(30, 70), out areaA, out areaB);
                }
                else
                {
                    areas[index].SplitAlongHorizontalAxis(rand.Next(30, 70), out areaA, out areaB);
                }

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