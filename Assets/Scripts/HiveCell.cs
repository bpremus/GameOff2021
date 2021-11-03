using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HiveCell : MonoBehaviour
{

    [SerializeField]
    GameObject[] room_prefabs;


    // pathfinding 
    // -------------------------
    public int x;
    public int y;
    public int gCost;
    public int hCost;
    public int walkable = 0;
    public int dbg_select = 0;

    public int fCost
    {
        get => gCost + hCost;
    }

    public void SetNode(int x, int y)
    {
        this.x = x;
        this.y = y;
        gCost = 0;
        hCost = 0;
        walkable = 0;

        this.name = "call_" + x + "_" + y;

    }



    public void OnHover()
    {
       
    }

    public void OnSelect()
    {
        Vector3 pos = transform.position;
        GameObject g = Instantiate(room_prefabs[0], pos, Quaternion.identity);
        g.transform.parent = transform.parent;

        // set node to be walkable
        walkable = 1;
    }

    public void SetEntry()
    {
        Vector3 pos = transform.position;
        GameObject g = Instantiate(room_prefabs[0], pos, Quaternion.identity);
        g.transform.parent = transform.parent;
        walkable = 1;
    }

    public void SetTopLevel()
    {
        Vector3 pos = transform.position;
        GameObject g = Instantiate(room_prefabs[0], pos, Quaternion.identity);
        g.transform.parent = transform.parent;
        walkable = 1;
    }

    [SerializeField]
    protected HiveCell[] neighbour_cells = new HiveCell[4];

    public void Setneighbour(HiveCell cell, int index)
    {
        neighbour_cells[index] = cell;
    }

    public List<HiveCell> GetNeighbours()
    {
        List<HiveCell> list = new List<HiveCell>();
        for (int i = 0; i < neighbour_cells.Length; i++)
        {
            if (neighbour_cells[i] != null)
                list.Add(neighbour_cells[i]);
        }

        return list;
    }
}
