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
        BuildGrid();
        //  Setneighbours();
        //  BuildTopLevel();
        //  BuildOusideEntry();
        //  BuildDebugPath();
        SetFixedCells();
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
      //  BuildDebugPath();

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
                        hc.Setneighbour(cells[i + 1][j], 1);
                    if (j - 1 > 0)
                        hc.Setneighbour(cells[i][j - 1], 2);
                    if (i - 1 > 0)
                        hc.Setneighbour(cells[i - 1][j], 3);
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
    GameObject[] dbg_rooms;

    public void BuildDebugPath()
    {
        HiveCell a = cells[3][height - 1];
        a.BuildRoom(dbg_rooms[0]);
        a = cells[3][height - 2];
        a.BuildRoom(dbg_rooms[0]);
        a = cells[3][height - 3];
        a.BuildRoom(dbg_rooms[0]);
        a = cells[3][height - 4];
        a.BuildRoom(dbg_rooms[0]);
        a = cells[3][height - 5];
        a.BuildRoom(dbg_rooms[0]);
        a = cells[4][height - 5];
        a.BuildRoom(dbg_rooms[0]);
        a = cells[5][height - 5];
        a.BuildRoom(dbg_rooms[0]);
        a = cells[6][height - 5];
        a.BuildRoom(dbg_rooms[1]);
    }


    [SerializeField]
    CellMesh[] cell_mesh_prefabs;

   
    public class GridSolver
    {
        public string name = "";
        int x;
        int y;
        public GridSolver(int x, int y)
        {
            this.x = x;
            this.y = y;

            name = "CS_" + x + "_" + y;
        }

        public int GetX { get => x; }
        public int GetY { get => y; }

        // tile 
        public int mesh_index = -1;
        int[] connections = new int[4] { -1, -1, -1, -1 };
        public CellMesh.Cell_type cell_Type = CellMesh.Cell_type.dirt;

        public void SetConnection(int[] connections)
        {
            this.connections = connections;
        }

        public GridSolver[] bordering_tiles = new GridSolver[4]; // all null by default 
        public void SetBorder(GridSolver solver, int index)
        {
            bordering_tiles[index] = solver;
        }

        public int GetConnectionInDirection(int index)
        {
            if (bordering_tiles[index] != null)
            {
                if (bordering_tiles[index].cell_Type == CellMesh.Cell_type.corridor) return 1;
                if (bordering_tiles[index].cell_Type == CellMesh.Cell_type.room)     return 1;
                
                if (index == 0) // up
                    if (bordering_tiles[index].cell_Type == CellMesh.Cell_type.entrance) return 1;
            }
            return 0;
        }

        public void Dump()
        {

            for (int i = 0; i < 4; i++)
            {
                if (bordering_tiles[i] != null)
                    Debug.Log(bordering_tiles[i].name + " > " + bordering_tiles[i].cell_Type);
            }
        }
            
    }

    public void PlaceCooridors(List<List<GridSolver>> cell_mesh)
    {
        for (int i = 0; i < cell_mesh.Count; i++)
        {
            for (int j = 0; j < cell_mesh[i].Count; j++)
            {
                GridSolver gs = cell_mesh[i][j];
                if (gs.cell_Type == CellMesh.Cell_type.corridor || gs.cell_Type == CellMesh.Cell_type.room)
                {
                    // get neighbouring tiles 
                    // prepare connections 

                    int[] connections = new int[4] { 0, 0, 0, 0 };
                    for (int c = 0; c < 4; c++)
                    {
                        connections[c] = gs.GetConnectionInDirection(c);
                        Debug.Log(c + " >> " + connections[c]);
                    }

                    // find best tile 
                    for (int m = 0; m < cell_mesh_prefabs.Length; m++)
                    {
                        CellMesh cm = cell_mesh_prefabs[m];
                        if (connections.SequenceEqual(cm.connections))
                        {
                            if (cm.cell_Type == gs.cell_Type)
                            {
                                gs.mesh_index = m;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    // resolve rest 
                    if (gs.mesh_index == -1)
                        gs.mesh_index = 1;
                }
            }
        }
    }

  
    public void SetFixedCells()
    {
        // placing fixe cells like entrance
        List<List<GridSolver>> cell_mesh = new List<List<GridSolver>>();
        
        // build inital grid 
        for (int i = 0; i < width; i++)
        {
            List<GridSolver> rows = new List<GridSolver>();
            for (int j = 0; j < height; j++)
            {
                //Vector3 pos = new Vector3(i * cell_offset - width / 2, j * cell_offset - height, 0);
                //CellMesh cm = Instantiate(cell_mesh_prefabs[1], pos, Quaternion.identity);
                GridSolver gs = new GridSolver(i,j);
                // gs.SetConnection(cell_mesh_prefabs[gs.mesh_index].connections);
                rows.Add(gs);
            }
            cell_mesh.Add(rows);
        }

        // map borders 
        for (int i = 0; i < cell_mesh.Count; i++)
        {
            for (int j = 0; j < cell_mesh[i].Count; j++)
            {
                GridSolver gs = cell_mesh[i][j];
                if (i + 1 < cell_mesh.Count)
                {
                    gs.SetBorder(cell_mesh[i + 1][j], 3);
                }
                if (i - 1 >= 0)
                {
                    gs.SetBorder(cell_mesh[i - 1][j], 1);
                }

                if (j + 1 < cell_mesh[i].Count)
                {
                    gs.SetBorder(cell_mesh[i][j +1], 0);
                }
                if (j - 1 >= 0)
                {
                    gs.SetBorder(cell_mesh[i][j - 1], 2);
                }
            }
        }

        // fixed cells 
        for (int i = 0; i < width; i++)
        {
            cell_mesh[i][9].mesh_index = 0;
            cell_mesh[i][9].cell_Type = CellMesh.Cell_type.top;
        }

        cell_mesh[5][9].mesh_index = 2;
        cell_mesh[5][9].cell_Type = CellMesh.Cell_type.entrance;


        // we need to solve non dirt first, or the one with lowest choices 
        // we have simple solution as each tile can connect any other with corridor 
        cell_mesh[5][8].cell_Type = CellMesh.Cell_type.corridor;
        cell_mesh[5][7].cell_Type = CellMesh.Cell_type.corridor;
        cell_mesh[5][6].cell_Type = CellMesh.Cell_type.corridor;
        cell_mesh[6][6].cell_Type = CellMesh.Cell_type.room;
        cell_mesh[7][6].cell_Type = CellMesh.Cell_type.corridor;
        cell_mesh[4][6].cell_Type = CellMesh.Cell_type.room;
        cell_mesh[4][5].cell_Type = CellMesh.Cell_type.corridor;

        // solver 
        PlaceCooridors(cell_mesh);


        // solve grid 
        for (int i = 0; i < cell_mesh.Count; i++)
        {
            for (int j = 0; j < cell_mesh[i].Count; j++)
            {
                GridSolver gs = cell_mesh[i][j];
                int idx = gs.mesh_index;
                if (idx < 0) continue;

                Vector3 pos = new Vector3(gs.GetX * offset - width / 2, gs.GetY * offset - height, 0);
                CellMesh cm = Instantiate(cell_mesh_prefabs[idx], pos, Quaternion.identity);
                cm.name = gs.name;
                gs.Dump();

                cm.transform.SetParent(this.transform);

            }
        }

    }


}






