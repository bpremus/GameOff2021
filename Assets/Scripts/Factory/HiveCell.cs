using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HiveCell : MonoBehaviour
{

    public bool isCellEmpty;
    protected CoreRoom childRoom;
    [SerializeField]
    public CellMesh cell_mesh;

    public void SetNode(int x, int y)
    {
        // cell placement and pathfinding 
        this.x = x;
        this.y = y;

        // pathfinding 
        gCost = 0;
        hCost = 0;
        walkable = 0;

        // room building
        isCellEmpty = true;
        this.name = "call_" + x + "_" + y;
    }


    // this will determine what kind of cell mesh (walls) are going to be drawn
    public CellMesh.Cell_type cell_Type = CellMesh.Cell_type.dirt;
    // cell still has a room that inside have a room mesh 


    public void BuildRoom(GameObject room)
    {
        if (isCellEmpty)
        {
            Vector3 pos = transform.position;

            // each room prefab inherit from core room
            childRoom = Instantiate(room, pos, Quaternion.identity).GetComponent<CoreRoom>();
            if (childRoom)
            {
                childRoom.cell = this;
                childRoom.transform.SetParent(transform.parent);
            }
            walkable = 1;
            isCellEmpty = false;
        }
        else
        {
            Debug.LogError("Cell is already taken! You want to destroy?");
        }
    }

    public void DestroyRoom()
    {
        if (isCellEmpty)
        {
            Debug.LogError("Nothing to destroy!");
        }
        else
        {
            Destroy(childRoom);
            walkable = 0;
            isCellEmpty = true;
        }
    }

    // drawing the room mesh 
    #region room_mesh

    public int GetX { get => x; }
    public int GetY { get => y; }

    public int mesh_index = -1;
    int[] connections = new int[4] { -1, -1, -1, -1 };

    public void SetConnection(int[] connections)
    {
        this.connections = connections;
    }

 
    public int GetConnectionInDirection(int index)
    {
        if (neighbour_cells[index] != null)
        {
            if (neighbour_cells[index].cell_Type == CellMesh.Cell_type.corridor) return 1;
            if (neighbour_cells[index].cell_Type == CellMesh.Cell_type.room)     return 1;

            if (index == 0) // up
                if (neighbour_cells[index].cell_Type == CellMesh.Cell_type.entrance) return 1;
        }
        return 0;
    }

    #endregion

    // Everything regarding pathfinding 
    #region path_finding 

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

    #endregion
}
