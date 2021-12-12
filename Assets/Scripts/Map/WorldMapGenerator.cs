using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldMapGenerator : MonoBehaviour
{

    // Save and Load 
    // ----------------------------
    [System.Serializable]
    public class SaveWorldMapCell
    {
        public int width;
        public int height;
        public int map_sid;
        public int[] visited_cells;
    }

    public SaveWorldMapCell GetSaveWorldMapData()
    {
        SaveWorldMapCell data = new SaveWorldMapCell();
        data.width = this.width;
        data.height = this.height;
        data.map_sid = this.map_sid;

       // int size = this.visited_cells.Count;
       // data.visited_cells = new int[size];
       // for (int i = 0; i < this.visited_cells.Count; i++)
       // {
       //     data.visited_cells[i] = this.visited_cells[i].uuid;
       // }
       // 
        return data;
    }

    public void SetSaveWorldMapData(SaveWorldMapCell data)
    {
        this.width   = data.width;
        this.height  = data.height;
        this.map_sid = data.map_sid;

      //  // if needed here we can regenerate map
      //  EnableMap();
      //
      //  // continue load 
      //  int size = data.visited_cells.Length;
      //  for (int d = 0; d < size; d++)
      //  {
      //      for (int i = 0; i < width; i++)
      //      {
      //          for (int j = 0; j < height; j++)
      //          {
      //              if (cells[i][j].uuid == data.visited_cells[d])
      //              {
      //                  ReportVisitedCell(cells[i][j]);
      //              }
      //          }
      //      }
      //  }
      //
      //  CloseMap();
    }


    public int width  = 10;
    public int height = 10;

    [SerializeField] GameObject map_prefab;
    [SerializeField] float x_offset = 50;
    [SerializeField] float y_offset = 50;
    [SerializeField] int   map_sid = 1;
    [SerializeField] Transform parrent;
    [SerializeField] protected GameObject grid_ui_node;

    List<List<WorldMapCell>> cells = new List<List<WorldMapCell>>();

    // this is important 
    List<WorldMapCell> visited_cells = new List<WorldMapCell>();
    bool map_generated = false;

    protected void Start()
    {
        // GenerateGrid();
        // grid_ui_node = transform.GetChild(0).gameObject;
    }

    HiveRoom selected_room;
    public void OpenMap(HiveRoom qr)
    {
        if (qr.GetAssignedBugs().Count > 0)
        {
            selected_room = qr;
            EnableMap();
        }
        else
        {
            //GameLog.Instance.WriteLine("You don't have enough assigned bugs in this room");
            ActionLogger.Instance.AddLog("You need bugs in this room to perform this action", 1);
        }
    }

    public void ReportVisitedCell(WorldMapCell cell)
    {
        // on successful scout or gather
        if (visited_cells.Contains(cell) == false)
        {
            visited_cells.Add(cell);
            cell.ExpandCell();
            //GameLog.Instance.WriteLine("We have explored a new area");
            ActionLogger.Instance.AddLog("We have explored a new area", 0);
        }
        // we are just collecting 
        cell.OnBugVisit();
    }

    public void SetRoomAsDesitnation(WorldMapCell cell)
    {
        if (selected_room != null)
        {
            // visited_cells.Add(cell);
            selected_room.SendToCollect(cell);
            CloseMap();
        }
    }

    // ----------------------------------------

    public void EnableMap()
    {
        grid_ui_node.SetActive(true); // and build the first time       
        if (map_generated == false)
        {         
            GenerateGrid();
            map_generated = true;
        } 
    }

    public void CancelMap()
    {
        grid_ui_node.SetActive(false);
    }

    public WorldMapCell CloseMap()
    {
        grid_ui_node.SetActive(false);
        return null;
    }

    protected void GenerateGrid()
    {
        // Generate map 
        
        for (int i = 0; i < width; i++)
        {
            List<WorldMapCell> col = new List<WorldMapCell>();
            for (int j = 0; j < height; j++)
            {
                col.Add(SpawnButton(i, j));
            }
            cells.Add(col);
        }

        // Generate neighbors 

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                WorldMapCell cell = cells[i][j];
                cell.SetNeighbours(cells);
            }
        }

        // Place hive
        WorldMapCell hive = cells[width / 2][height / 2];
        hive.cell_Type = WorldMapCell.Cell_type.player_hive;
        hive.ExpandCell();
    }

    protected WorldMapCell SpawnButton(int x, int y)
    {
        Vector3 pos = new Vector3((x * x_offset) - (x_offset * width) / 2, (y * y_offset) - (y_offset * height) / 2, 1);
        if (x % 2 == 0)
        {
            pos.y += y_offset / 2;
        }
        pos = parrent.position + pos;
        WorldMapCell g = Instantiate(map_prefab, pos, Quaternion.identity, parrent).GetComponent<WorldMapCell>();
        g.SetPos(x, y, this);
        g.cell_Type = WorldMapCell.Cell_type.hidden;

        float perl = Mathf.PerlinNoise((float) x / width -1,(float)y / height -1);
        float dist = Vector3.Distance(transform.position, pos);
        g.peral = perl;
        g.dist = dist;

        return g;           
    }

    private static WorldMapGenerator _instance;
    public static WorldMapGenerator Instance
    {
        get { return _instance; }
    }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        _instance = this;
    }


}
