using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HiveGenerator : MonoBehaviour
{
    [SerializeField]
    int width = 10;
    [SerializeField]
    int height = 10;
    [SerializeField]
    float offset = 1.2f;

    [SerializeField]
    GameObject cell_prefab;

    [SerializeField]
    public List<List<HiveCell>> cells = new List<List<HiveCell>>();

    public void Start()
    {
        // use the buttons in inspector of a grid editor to test 
        // this initial placement will be reworked later
        DebugGrid();
    }

    public void DeleteGrid()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            DestroyImmediate(this.transform.GetChild(i).gameObject);
        }
        cells.Clear();
    }

    public void GenerateStaticGrid()
    {
        // grid does not change
        BuildGrid();
        Setneighbours();
        SetFixedCells();
    }

    public void CollecteCells()
    {
        // lazy collect untill i fix the lifeclycle

        HiveCell[] firstList = GameObject.FindObjectsOfType<HiveCell>();
        for (int i = 0; i < width; i++)
        {
            List<HiveCell> rows = new List<HiveCell>();
            for (int j = 0; j < height; j++)
            {
                Vector3 pos = new Vector3(i * offset - width / 2, j * offset - height, 0);
                for (int k = 0; k < firstList.Length; k++)
                {
                    HiveCell hc = firstList[k];
                    if (hc.GetX == i && hc.GetY == j)
                    {
                        rows.Add(hc);
                    }
                }
            }
            cells.Add(rows);
        }
    }

    public void BuildGrid()
    {
        for (int i = 0; i < width; i++)
        {
            List<HiveCell> rows = new List<HiveCell>();
            for (int j = 0; j < height; j++)
            {
                Vector3 pos = new Vector3(i * offset - width / 2, j * offset - height, 0);
                HiveCell c = CreateTile(pos);
                c.SetNode(i, j);
                rows.Add(c);
            }
            cells.Add(rows);
        }
    }

    public void Setneighbours()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                HiveCell hc = cells[i][j];
                if (hc)
                {
                    if (j + 1 < height)
                        hc.Setneighbour(cells[i][j + 1], 0);
                    if (i + 1 < width)
                        hc.Setneighbour(cells[i + 1][j], 3);
                    if (j - 1 > 0)
                        hc.Setneighbour(cells[i][j - 1], 2);
                    if (i - 1 > 0)
                        hc.Setneighbour(cells[i - 1][j], 1);
                }
            }
        }
    }

    public HiveCell CreateTile(Vector3 position)
    {
        GameObject g = Instantiate(cell_prefab, position, Quaternion.identity);
        if (g)
        {
            HiveCell c = g.GetComponent<HiveCell>();
            if (c)
            {
                g.transform.SetParent(this.transform);
                return c;
            }
        }
        return null;
    }

    [SerializeField]
    CellMesh[] cell_mesh_prefabs;

    public void PlaceCooridors(List<List<HiveCell>> cell_mesh)
    {
        for (int i = 0; i < cell_mesh.Count; i++)
        {
            for (int j = 0; j < cell_mesh[i].Count; j++)
            {
                HiveCell gs = cell_mesh[i][j];
                if (UpdateCell(gs)) 
                { 
                    // cell ok
                }
                else
                {
                    // resolve rest 
                    if (gs.mesh_index == -1)
                        gs.mesh_index = 0;
                }
            }
        }
    }

    public void DebugGrid()
    {

        CollecteCells();

        cells[5][7].cell_Type = CellMesh.Cell_type.corridor;
        cells[5][6].cell_Type = CellMesh.Cell_type.corridor;
        cells[6][6].cell_Type = CellMesh.Cell_type.room;
        cells[7][6].cell_Type = CellMesh.Cell_type.corridor;
        cells[4][6].cell_Type = CellMesh.Cell_type.room;
        cells[4][5].cell_Type = CellMesh.Cell_type.corridor;

        RefreshAllCells();

    }

    public void RefreshAllCells()
    {
        for (int i = 0; i < cells.Count; i++)
        {
            for (int j = 0; j < cells[i].Count; j++)
            {
                HiveCell gs = cells[i][j];
                UpdateCell(gs);
            }
        }
        RedrawGrid();
    }
    
    public bool UpdateCell(HiveCell cell)
    {
        if (cell.cell_Type == CellMesh.Cell_type.corridor || cell.cell_Type == CellMesh.Cell_type.room)
        {
            // get neighbouring tiles 
            // prepare connections 

            int[] connections = new int[4] { 0, 0, 0, 0 };
            for (int c = 0; c < 4; c++)
            {
                connections[c] = cell.GetConnectionInDirection(c);
            }

            // find best tile 
            for (int m = 0; m < cell_mesh_prefabs.Length; m++)
            {
                CellMesh cm = cell_mesh_prefabs[m];
                if (connections.SequenceEqual(cm.connections))
                {
                    if (cm.cell_Type == cell.cell_Type)
                    {
                        cell.mesh_index = m;
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public void RedrawGrid()
    {
        // solve grid 
        for (int i = 0; i < cells.Count; i++)
        {
            for (int j = 0; j < cells[i].Count; j++)
            {
                HiveCell gs = cells[i][j];
                int idx = gs.mesh_index;
                if (idx < 0) continue;

                if (gs.cell_mesh != null)
                {
                    Destroy(gs.cell_mesh.gameObject);
                }

                Vector3 pos = new Vector3(gs.GetX * offset - width / 2, gs.GetY * offset - height, 0);
                CellMesh cm = Instantiate(cell_mesh_prefabs[idx], pos, Quaternion.identity);
                cm.name = gs.name;
                cm.transform.SetParent(this.transform);
                gs.cell_mesh = cm;
            }
        }
    }

    public void ShowGhostCell()
    { 
    
    }
  

    public void SetFixedCells()
    {
        
        // fixed cells 
        for (int i = 0; i < width; i++)
        {
            cells[i][9].mesh_index = 32;
            cells[i][9].cell_Type = CellMesh.Cell_type.top;
        }

        cells[5][9].mesh_index = 1;
        cells[5][9].cell_Type = CellMesh.Cell_type.entrance;

        // we need to solve non dirt first, or the one with lowest choices 
        // we have simple solution as each tile can connect any other with corridor 
        cells[5][8].cell_Type = CellMesh.Cell_type.corridor;

        // solver 
        PlaceCooridors(cells);

        // redraw grid
        RedrawGrid();
    }

}






