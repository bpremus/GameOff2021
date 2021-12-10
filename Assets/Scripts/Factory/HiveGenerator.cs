using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HiveGenerator : MonoBehaviour
{
    // Save and Load 
    // ----------------------------
    [System.Serializable]
    public class SaveHiveGenerator
    {
        public int width;
        public int height;
        public bool isGameStarted;
        public HiveCell.SaveHiveCell[] saved_cells;
    }

    public SaveHiveGenerator GetSaveData()
    {
        SaveHiveGenerator data = new SaveHiveGenerator();

        // Save each cell
        //     cell saves room
        //          room saves bugs
        data.width = this.width;
        data.height = this.height;
        data.isGameStarted = this.isGameStarted;

        List<HiveCell> all_modified_cells = GetAllBuiltRooms();
        data.saved_cells = new HiveCell.SaveHiveCell[all_modified_cells.Count];
        for (int i = 0; i < all_modified_cells.Count; i++)
        {
            data.saved_cells[i] = all_modified_cells[i].GetSaveData();
        }
        
        return data;
    }

    public void SetSaveData(SaveHiveGenerator data)
    {
        this.width = data.width;
        this.height = data.height;
        this.isGameStarted = data.isGameStarted;
        for (int i = 0; i < data.saved_cells.Length; i++)
        {
            HiveCell.SaveHiveCell cell_data = data.saved_cells[i];
            HiveCell hive_cell = cells[cell_data.x][cell_data.y];

            if (cell_data.room_context == HiveCell.RoomContext.corridor)
            {
                hive_cell.BuildCooridor();
            }
            else if (cell_data.room_context == HiveCell.RoomContext.entrance)
            {
                hive_cell.BuildEntrance();
            }
            else
            {
                hive_cell.BuildRoom(cell_data.room_context);
            }
        }
    }

    // Prefabs needed to generate grid 
    // could be moved to art-instance
    [SerializeField] GameObject cell_prefab;
    [SerializeField] CellMesh[] cell_mesh_prefabs;

    [SerializeField]
    int width = 10;
    [SerializeField]
    int height = 10;
    [SerializeField]
    float offset = 1.2f;

    public List<List<HiveCell>> cells = new List<List<HiveCell>>();
    public List<HiveCell> rooms = new List<HiveCell>();
    public HiveCell hive_cell = null;
    public List<HiveCell> hive_entrance = new List<HiveCell>();

    public bool isGameStarted = false;
    public int [] GetSize()
    {
        return new int[] { width, height };
    }

    public List<HiveCell> GetAllBuiltRooms()
    {
        List<HiveCell> rooms = new List<HiveCell>();
        for (int i = 0; i < cells.Count; i++)
        {
            for (int j = 0; j < cells[i].Count; j++)
            {
                HiveCell gs = cells[i][j];
                CoreRoom room = gs.GetRoom();
                if (room)
                {
                    if (rooms.Contains(gs) == false)
                        rooms.Add(gs);
                }
            }
        }
        return rooms;
    }

    public List<HiveCell> GetAllRooms()
    {
        List<HiveCell> rooms = new List<HiveCell>();

        for (int i = 0; i < cells.Count; i++)
        {
            for (int j = 0; j < cells[i].Count; j++)
            {
                HiveCell gs = cells[i][j];
                CoreRoom room = gs.GetRoom();
                if (room)
                {
                    if (room.GetComponent<HiveCorridor>()) continue;
                    if (room.GetComponent<HiveEntrance>()) continue;

                    if (rooms.Contains(gs) == false)
                            rooms.Add(gs);
                    
                }
            }
        }
        return rooms;
    }
    public List<HiveCell> GetAllCooridors()
    {
        List<HiveCell> rooms = new List<HiveCell>();

        for (int i = 0; i < cells.Count; i++)
        {
            for (int j = 0; j < cells[i].Count; j++)
            {
                HiveCell gs = cells[i][j];
                if (gs.cell_Type == CellMesh.Cell_type.corridor)
                {
                    rooms.Add(gs);
                }
            }
        }
        return rooms;
    }
    public List<HiveCell> GetAllEntrances()
    {
        List<HiveCell> rooms = new List<HiveCell>();

        for (int i = 0; i < cells.Count; i++)
        {
            for (int j = 0; j < cells[i].Count; j++)
            {
                HiveCell gs = cells[i][j];
                if (gs.cell_Type == CellMesh.Cell_type.entrance)
                {
                    rooms.Add(gs);
                }
            }
        }
        return rooms;
    }

    public void Start()
    {
        // use the buttons in inspector of a grid editor to test 
        // this initial placement will be reworked later
        CollecteCells();
        isGameStarted = true;
    }

    public void DeleteGrid()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            DestroyImmediate(this.transform.GetChild(i).gameObject);
        }
        cells.Clear();
    }

    public void CleanGrid()
    {
        for (int i = 0; i < cells.Count; i++)
        {
            for (int j = 0; j < cells[i].Count; j++)
            {
                HiveCell gs = cells[i][j];
                CoreRoom room = gs.GetRoom();
                if (room)
                {
                    List<CoreBug> bugs = room.GetAssignedBugs();
                    for (int b = 0; b < bugs.Count; b++)
                    {
                        Destroy(bugs[b].gameObject);
                    }
                    gs.DestroyRoom(true);
                }
                Destroy(gs.gameObject);
            }
        }

       BuildGrid();
       Setneighbours();
       BuildStaticTop();
       RefreshAllCells();
    }

    public void GenerateStaticGrid()
    {
        // grid does not change
        BuildGrid();
        Setneighbours();
        BuildStaticTop();
        BuildQueenRoom();

        RefreshAllCells();
    }

    public void BuildStaticTop()
    {
        BuildTopLevel();
        BuildDirtBorder();
    }

    public void CollecteCells()
    {
        // lazy collect until i fix the life-cycle
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
                    hc.SetGenerator = this;
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
        cells.Clear();
        for (int i = 0; i < width; i++)
        {
            List<HiveCell> rows = new List<HiveCell>();
            for (int j = 0; j < height; j++)
            {
                Vector3 pos = new Vector3(i * offset - width, (j * offset) - height * offset, 0);
                HiveCell c = CreateTile(pos);
                c.SetNode(i, j);
                c.SetGenerator = this;
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

    public void BuildTopLevel()
    {
        // outside top row
        for (int i = 0; i < width; i++)
        {
            cells[i][height - 1].BuildOutside();
        }
    }

    public void BuildDirtBorder()
    {
        // earth border row with opening 
        for (int i = 0; i < width; i++)
        {
            cells[i][height - 2].mesh_index = 32;
            cells[i][height - 2].cell_Type = CellMesh.Cell_type.top;
            cells[i][height - 2].is_static_cell = true;

            cells[i][height - 3].mesh_index = 0;
            cells[i][height - 3].cell_Type = CellMesh.Cell_type.dirt;
            cells[i][height - 3].is_static_cell = true;
        }
    }

    public void BuildQueenRoom()
    {
        int d = width / 2;
        cells[d][height - 2].BuildEntrance();
        cells[d][height - 3].BuildCooridor();
        cells[d][height - 3].BuildCooridor();
        cells[d][height - 4].BuildCooridor();
        cells[d][height - 5].BuildRoom(HiveCell.RoomContext.hive);
        hive_entrance.Add(cells[d][height - 2]);
    }

    public HiveCell CreateTile(Vector3 position)
    {
        position += transform.position;

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
            // get neighboring tiles 
            // prepare connections 
            cell.walkable = 1;
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
        if (cell.cell_Type == CellMesh.Cell_type.entrance)
        {
            cell.walkable = 1;
            cell.mesh_index = 1;
        }

        if (cell.cell_Type == CellMesh.Cell_type.dirt)
        {
            cell.walkable = 0;
            cell.mesh_index = 0;
        }
        if (cell.cell_Type == CellMesh.Cell_type.outside)
        {
            cell.mesh_index = -2;
            cell.walkable = 1;
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
               
                if (gs.cell_mesh != null)
                {
                    if (Application.isEditor)
                        DestroyImmediate(gs.cell_mesh.gameObject);
                    else
                        Destroy(gs.cell_mesh.gameObject);       
                }

                if (idx < 0) continue;

                Vector3 pos = new Vector3(gs.GetX * offset - width, (gs.GetY * offset) - height * offset, 0);
                pos += transform.position;
                CellMesh cm = Instantiate(cell_mesh_prefabs[idx], pos, Quaternion.identity);
                cm.name = "mesh_" + gs.x + "_" + gs.y;
                gs.name = "cell_" + gs.x + "_" + gs.y;
                cm.transform.SetParent(gs.transform);
                gs.cell_mesh = cm;

                gs.UpdateColors();
            }
        }
    }

  

    

}






