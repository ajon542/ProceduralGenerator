using UnityEngine;
using System.Collections;

public class MapBuilder : MonoBehaviour
{

    public int fillPercent = 45;

    public int width = 200;
    public int height = 70;
    public int smooth = 5;

    public string seed;
    public bool useRandomSeed = true;

    private int[,] map;

    void Start()
    {
        BuildMap();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            BuildMap();
        }
    }

    void BuildMap()
    {
        map = new int[width, height];
        RandomFillMap();

        for (int i = 0; i < smooth; ++i)
        {
            SmoothMap();
        }
    }

    void RandomFillMap()
    {

        if (useRandomSeed)
        {
            seed = Time.time.ToString();
        }

        System.Random rand = new System.Random(seed.GetHashCode());

        for (int i = 0; i < width; ++i)
        {
            for (int j = 0; j < height; ++j)
            {

                if (i == 0 || i == width - 1 || j == 0 || j == height - 1)
                {
                    map[i, j] = 1;
                }
                else
                {
                    map[i, j] = (rand.Next(0, 100) < fillPercent) ? 1 : 0;
                }
            }
        }
    }

    void SmoothMap()
    {

        for (int i = 0; i < width; ++i)
        {
            for (int j = 0; j < height; ++j)
            {
                int neighbourWallTiles = GetNeighbourCount(i, j);

                if (neighbourWallTiles > 4)
                {
                    map[i, j] = 1;
                }
                else if (neighbourWallTiles < 4)
                {
                    map[i, j] = 0;
                }
            }
        }
    }


    int GetNeighbourCount(int gridX, int gridY)
    {
        int wallCount = 0;
        for (int i = gridX - 1; i <= gridX + 1; ++i)
        {
            for (int j = gridY - 1; j <= gridY + 1; ++j)
            {
                if (i >= 0 && i < width && j >= 0 && j < height)
                {
                    if (i != gridX || j != gridY)
                    {
                        wallCount += map[i, j];
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

    void OnDrawGizmos()
    {
        if (map == null)
        {
            return;
        }

        for (int i = 0; i < width; ++i)
        {
            for (int j = 0; j < height; ++j)
            {
                Gizmos.color = (map[i, j] == 1) ? Color.black : Color.white;
                Vector3 pos = new Vector3(-width / 2 + i + 0.5f, -height / 2 + j + 0.5f);
                Gizmos.DrawCube(pos, Vector3.one);
            }
        }
    }
}