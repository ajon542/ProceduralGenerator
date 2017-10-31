namespace MapBuilder.Tutorials
{
    using UnityEngine;
    using System.Collections.Generic;

    [SelectionBase]
    public class VoxelGrid : MonoBehaviour
    {
        public int resolution;

        public GameObject voxelPrefab;

        private Voxel[] voxels;

        private float voxelSize;

        private Material[] voxelMaterials;

        private Mesh mesh;

        private List<Vector3> vertices;

        private List<Vector3> normals;

        private List<int> triangles;

        public VoxelGrid xNeighbor, yNeighbor, xyNeighbor;
        private float gridSize;
        private Voxel dummyX, dummyY, dummyT;

        public void Initialize(int resolution, float size)
        {
            // Initialize the grid data.
            this.resolution = resolution;
            gridSize = size;
            voxelSize = size / resolution;
            voxels = new Voxel[resolution * resolution];
            voxelMaterials = new Material[voxels.Length];

            dummyX = new Voxel();
            dummyY = new Voxel();
            dummyT = new Voxel();

            // Create the voxel game objects.
            for (int i = 0, y = 0; y < resolution; y++)
            {
                for (int x = 0; x < resolution; x++, i++)
                {
                    CreateVoxel(i, x, y);
                }
            }

            GetComponent<MeshFilter>().mesh = mesh = new Mesh();
            mesh.name = "VoxelGrid Mesh";
            vertices = new List<Vector3>();
            triangles = new List<int>();
            normals = new List<Vector3>();

            // Refresh the grid.
            Refresh();
        }

        private void CreateVoxel(int i, int x, int y)
        {
            GameObject o = Instantiate(voxelPrefab);
            o.transform.parent = transform;
            o.transform.localPosition = new Vector3((x + 0.5f) * voxelSize, (y + 0.5f) * voxelSize, -0.01f);
            o.transform.localScale = Vector3.one * voxelSize * 0.1f;
            voxelMaterials[i] = o.GetComponent<MeshRenderer>().material;
            voxels[i] = new Voxel(x, y, voxelSize);
        }

        private void Refresh()
        {
            SetVoxelColors();
            Triangulate();
        }

        private void Triangulate()
        {
            vertices.Clear();
            triangles.Clear();
            normals.Clear();
            mesh.Clear();

            if (xNeighbor != null)
            {
                dummyX.BecomeXDummyOf(xNeighbor.voxels[0], gridSize);
            }
            TriangulateCellRows();
            if (yNeighbor != null)
            {
                TriangulateGapRow();
            }

            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.normals = normals.ToArray();
        }

        private void TriangulateCellRows()
        {
            int cells = resolution - 1;
            for (int i = 0, y = 0; y < cells; y++, i++)
            {
                for (int x = 0; x < cells; x++, i++)
                {
                    TriangulateCell(
                        voxels[i],
                        voxels[i + 1],
                        voxels[i + resolution],
                        voxels[i + resolution + 1]);
                }
                if (xNeighbor != null)
                {
                    TriangulateGapCell(i);
                }
            }
        }

        private void TriangulateGapCell(int i)
        {
            Voxel dummySwap = dummyT;
            dummySwap.BecomeXDummyOf(xNeighbor.voxels[i + 1], gridSize);
            dummyT = dummyX;
            dummyX = dummySwap;
            TriangulateCell(voxels[i], dummyT, voxels[i + resolution], dummyX);
        }

        private void TriangulateGapRow()
        {
            dummyY.BecomeYDummyOf(yNeighbor.voxels[0], gridSize);
            int cells = resolution - 1;
            int offset = cells * resolution;

            for (int x = 0; x < cells; x++)
            {
                Voxel dummySwap = dummyT;
                dummySwap.BecomeYDummyOf(yNeighbor.voxels[x + 1], gridSize);
                dummyT = dummyY;
                dummyY = dummySwap;
                TriangulateCell(voxels[x + offset], voxels[x + offset + 1], dummyT, dummyY);
            }

            if (xNeighbor != null)
            {
                dummyT.BecomeXYDummyOf(xyNeighbor.voxels[0], gridSize);
                TriangulateCell(voxels[voxels.Length - 1], dummyX, dummyY, dummyT);
            }
        }

        private void AddWall(Vector3 a, Vector3 b, Vector3 c)
        {
            // a = a.position
            // b = a.yEdgePosition
            // c = a.xEdgePosition

            // Add the wall.
            AddTriangle(b, new Vector3(b.x, b.y, -0.1f), c);
            AddTriangle(c, new Vector3(b.x, b.y, -0.1f), new Vector3(c.x, c.y, -0.1f));
        }

        /// <summary>
        /// Triangulates the cell.
        /// </summary>
        /// <param name="a">First voxel in the cell.</param>
        /// <param name="b">Second voxel in the cell.</param>
        /// <param name="c">Third voxel in the cell.</param>
        /// <param name="d">Forth voxel in the cell.</param>
        /// <remarks>
        /// 
        /// Voxel positions:
        /// c---d
        /// |   |
        /// a---b
        /// 
        /// There are 16 possible cell triangulation configurations:
        /// 0:
        /// o---o
        /// |   |
        /// o---o
        /// 
        /// 1, 2, 4, 8:
        /// o---o
        /// |   |
        /// x---o
        /// 
        /// 3, 5, 10, 12:
        /// o---o
        /// |   |
        /// x---x
        /// 
        /// 7, 11, 13, 14
        /// x---o
        /// |   |
        /// x---x
        /// 
        /// 15:
        /// x---x
        /// |   |
        /// x---x
        /// 
        /// 6, 9:
        /// x---o
        /// |   |
        /// o---x
        /// </remarks>
        private void TriangulateCell(Voxel a, Voxel b, Voxel c, Voxel d)
        {
            int cellType = 0;
            if (a.state)
            {
                cellType |= 1;
            }
            if (b.state)
            {
                cellType |= 2;
            }
            if (c.state)
            {
                cellType |= 4;
            }
            if (d.state)
            {
                cellType |= 8;
            }

            switch (cellType)
            {
                case 0:
                    return;
                    // Triangles.
                case 1:
                    AddTriangle(a.position, a.yEdgePosition, a.xEdgePosition);
                    break;
                case 2:
                    AddTriangle(b.position, a.xEdgePosition, b.yEdgePosition);
                    break;
                case 4:
                    AddTriangle(c.position, c.xEdgePosition, a.yEdgePosition);
                    break;
                case 8:
                    AddTriangle(d.position, b.yEdgePosition, c.xEdgePosition);
                    break;
                    // Quads.
                case 3:
                    AddQuad(a.position, a.yEdgePosition, b.yEdgePosition, b.position);
                    break;
                case 5:
                    AddQuad(a.position, c.position, c.xEdgePosition, a.xEdgePosition);
                    break;
                case 10:
                    AddQuad(a.xEdgePosition, c.xEdgePosition, d.position, b.position);
                    break;
                case 12:
                    AddQuad(a.yEdgePosition, c.position, d.position, b.yEdgePosition);
                    break;
                case 15:
                    AddQuad(a.position, c.position, d.position, b.position);
                    break;
                    // Pentagons.
                case 7:
                    AddPentagon(a.position, c.position, c.xEdgePosition, b.yEdgePosition, b.position);
                    break;
                case 11:
                    AddPentagon(b.position, a.position, a.yEdgePosition, c.xEdgePosition, d.position);
                    break;
                case 13:
                    AddPentagon(c.position, d.position, b.yEdgePosition, a.xEdgePosition, a.position);
                    break;
                case 14:
                    AddPentagon(d.position, b.position, a.xEdgePosition, a.yEdgePosition, c.position);
                    break;
                case 6:
                    AddTriangle(b.position, a.xEdgePosition, b.yEdgePosition);
                    AddTriangle(c.position, c.xEdgePosition, a.yEdgePosition);
                    break;
                case 9:
                    AddTriangle(a.position, a.yEdgePosition, a.xEdgePosition);
                    AddTriangle(d.position, b.yEdgePosition, c.xEdgePosition);
                    break;
            }
        }

        private void AddTriangle(Vector3 a, Vector3 b, Vector3 c)
        {
            int vertexIndex = vertices.Count;
            vertices.Add(a);
            vertices.Add(b);
            vertices.Add(c);
            triangles.Add(vertexIndex);
            triangles.Add(vertexIndex + 1);
            triangles.Add(vertexIndex + 2);

            Vector3 crossProduct = Vector3.Cross(a - b, b - c);
            normals.Add(crossProduct);
            normals.Add(crossProduct);
            normals.Add(crossProduct);
        }

        private void AddQuad(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
        {
            AddTriangle(a, b, c);
            AddTriangle(a, c, d);
        }

        private void AddPentagon(Vector3 a, Vector3 b, Vector3 c, Vector3 d, Vector3 e)
        {
            AddTriangle(a, b, c);
            AddTriangle(a, c, d);
            AddTriangle(a, d, e);
        }

        private void SetVoxelColors()
        {
            for (int i = 0; i < voxels.Length; i++)
            {
                voxelMaterials[i].color = voxels[i].state ? Color.black : Color.white;
            }
        }

        public void Apply(VoxelStencil stencil)
        {
            int xStart = stencil.XStart;
            if (xStart < 0)
            {
                xStart = 0;
            }
            int xEnd = stencil.XEnd;
            if (xEnd >= resolution)
            {
                xEnd = resolution - 1;
            }
            int yStart = stencil.YStart;
            if (yStart < 0)
            {
                yStart = 0;
            }
            int yEnd = stencil.YEnd;
            if (yEnd >= resolution)
            {
                yEnd = resolution - 1;
            }

            for (int y = yStart; y <= yEnd; y++)
            {
                int i = y * resolution + xStart;
                for (int x = xStart; x <= xEnd; x++, i++)
                {
                    voxels[i].state = stencil.Apply(x, y, voxels[i].state);
                }
            }
            Refresh();
        }
    }
}