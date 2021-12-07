using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldMapGenerator : MonoBehaviour
{
    public int width  = 10;
    public int height = 10;

    [SerializeField] GameObject map_prefab;
    [SerializeField] float x_offset = 50;
    [SerializeField] float y_offset = 50;
    [SerializeField] Transform parrent;
    [SerializeField] protected GameObject grid_ui_node;

    List<List<WorldMapCell>> cells = new List<List<WorldMapCell>>();
    List<WorldMapCell> visited_cells = new List<WorldMapCell>();
    bool map_generated = false;

    protected void Start()
    {
        // GenerateGrid();
        // grid_ui_node = transform.GetChild(0).gameObject;
    }

    QueenRoom selected_room;
    public void OpenMap(QueenRoom qr)
    {
        if (qr.GetAssignedBugs().Count > 0)
        {
            selected_room = qr;
            EnableMap();
        }
        else
        {
            GameLog.Instance.WriteLine("You dont have enough assigned bugs in this room");
        }
    }

    public void SetRoomAsDesitnation(WorldMapCell cell)
    {
        if (selected_room != null)
        {
            visited_cells.Add(cell);
            selected_room.SendToCollect();
            CloseMap();
        }
        // on sucessfull scout or gather
        cell.ExpandCell();
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

        // Generate neighbours 

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                WorldMapCell cell = cells[i][j];
                cell.SetNeighbours(cells);
            }
        }


        // Generate visited cells (on load)

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


        // Text t = g.GetComponentInChildren<Text>();



        // t.text = ""; // + x + ":" + y;
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
