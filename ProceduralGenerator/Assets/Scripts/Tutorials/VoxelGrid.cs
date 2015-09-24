namespace MapBuilder.Tutorials
{
    using UnityEngine;
    using System.Collections.Generic;

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
        private Voxel[] voxels;

        /// <summary>
        /// Size of the individual voxels.
        /// </summary>
        private float voxelSize;

        /// <summary>
        /// Individual voxel materials for setting the colour based on the voxel state.
        /// </summary>
        private Material[] voxelMaterials;

        /// <summary>
        /// Mesh for the voxel grid.
        /// </summary>
        private Mesh mesh;

        /// <summary>
        /// List of vertices for the voxel grid mesh.
        /// </summary>
        private List<Vector3> vertices;

        /// <summary>
        /// List of triangles for the voxel grid mesh.
        /// </summary>
        private List<int> triangles;

        public VoxelGrid xNeighbor, yNeighbor, xyNeighbor;
        private float gridSize;
        private Voxel dummyX, dummyY, dummyT;

        /// <summary>
        /// Initialize the voxel grid.
        /// </summary>
        /// <param name="resolution">The resolution of the grid.</param>
        /// <param name="size">The size of the grid.</param>
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

            // Refresh the grid.
            Refresh();
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
            o.transform.localPosition = new Vector3((x + 0.5f) * voxelSize, (y + 0.5f) * voxelSize, -0.01f);
            o.transform.localScale = Vector3.one * voxelSize * 0.1f;
            voxelMaterials[i] = o.GetComponent<MeshRenderer>().material;
            voxels[i] = new Voxel(x, y, voxelSize);
        }

        /// <summary>
        /// Set the colours of the voxels dpeending on their state.
        /// </summary>
        private void Refresh()
        {
            SetVoxelColors();
            Triangulate();
        }

        /// <summary>
        /// Generate the mesh.
        /// </summary>
        private void Triangulate()
        {
            vertices.Clear();
            triangles.Clear();
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
        }

        /// <summary>
        /// Generate the mesh.
        /// </summary>
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

        /// <summary>
        /// Adds a triangle to the current mesh data.
        /// </summary>
        /// <param name="a">First vertex of the triangle.</param>
        /// <param name="b">Second vertex of the triangle.</param>
        /// <param name="c">Third vertex of the triangle.</param>
        private void AddTriangle(Vector3 a, Vector3 b, Vector3 c)
        {
            int vertexIndex = vertices.Count;
            vertices.Add(a);
            vertices.Add(b);
            vertices.Add(c);
            triangles.Add(vertexIndex);
            triangles.Add(vertexIndex + 1);
            triangles.Add(vertexIndex + 2);
        }

        /// <summary>
        /// Adds a quad to the current mesh data.
        /// </summary>
        /// <param name="a">First vertex of the quad.</param>
        /// <param name="b">Second vertex of the quad.</param>
        /// <param name="c">Third vertex of the quad.</param>
        /// <param name="d">Forth vertex of the quad.</param>
        private void AddQuad(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
        {
            int vertexIndex = vertices.Count;
            vertices.Add(a);
            vertices.Add(b);
            vertices.Add(c);
            vertices.Add(d);
            triangles.Add(vertexIndex);
            triangles.Add(vertexIndex + 1);
            triangles.Add(vertexIndex + 2);
            triangles.Add(vertexIndex);
            triangles.Add(vertexIndex + 2);
            triangles.Add(vertexIndex + 3);
        }

        /// <summary>
        /// Adds a pentagon to the current mesh data.
        /// </summary>
        /// <param name="a">First vertex of the pentagon.</param>
        /// <param name="b">Second vertex of the pentagon.</param>
        /// <param name="c">Third vertex of the pentagon.</param>
        /// <param name="d">Forth vertex of the pentagon.</param>
        /// <param name="e">Fifth vertex of the pentagon.</param>
        private void AddPentagon(Vector3 a, Vector3 b, Vector3 c, Vector3 d, Vector3 e)
        {
            int vertexIndex = vertices.Count;
            vertices.Add(a);
            vertices.Add(b);
            vertices.Add(c);
            vertices.Add(d);
            vertices.Add(e);
            triangles.Add(vertexIndex);
            triangles.Add(vertexIndex + 1);
            triangles.Add(vertexIndex + 2);
            triangles.Add(vertexIndex);
            triangles.Add(vertexIndex + 2);
            triangles.Add(vertexIndex + 3);
            triangles.Add(vertexIndex);
            triangles.Add(vertexIndex + 3);
            triangles.Add(vertexIndex + 4);
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