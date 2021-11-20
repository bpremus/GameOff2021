using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class HiveCell : MonoBehaviour
{
    public bool isCellEmpty;
    [SerializeField]
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

        // set name 
        SetTileName();
    }
    Queue<CoreBug> bugs_to_assign = new Queue<CoreBug>();
    public bool AssignDrone(CoreBug bug)
    {
        Debug.Log("assinging a bug to a new room");
        bugs_to_assign.Enqueue(bug);
        return true;
    }

    public void DetachDrone(CoreBug bug)
    {
        CoreRoom current_room = bug.asigned_cell.GetRoom();
        if (current_room)
        {
            current_room.DetachBug(bug);
        }
    }

    protected virtual void ProcessAssigments()
    {
        if (childRoom == null) return;

        if (bugs_to_assign.Count > 0)
        {
            CoreBug bug = bugs_to_assign.Dequeue();
            CoreRoom current_room = bug.asigned_cell.GetRoom();
            if (current_room)
            {
                current_room.DetachBug(bug);
            }

            childRoom.AssignBug(bug);
            bug.asigned_cell = this;
        }
    }

    protected virtual void Update()
    {
        ProcessAssigments();
    }

    public CoreRoom GetRoom()
    {
        if (childRoom)
            return childRoom;
        else
            return null;
    }

    public void SetTileName()
    {
        this.name = "hive_cell_" + cell_Type.ToString() + "_" + x + "_" + y;
    }

    // this will determine what kind of cell mesh (walls) are going to be drawn
    public CellMesh.Cell_type cell_Type = CellMesh.Cell_type.dirt;

    public enum RoomContext { empty, queen, harvester, war };

    // cell still has a room that inside have a room mesh 
    public HiveGenerator hiveGenerator = null;
    public HiveGenerator SetGenerator
    {
        set => hiveGenerator = value;
    }

    public void BuildRoom()
    {
        Debug.Log("building room");
        // room is just a rooridor or room layout mesh 
        // we have some kind of room context 
        // Queen room 
        // harwaster room 
        SetTileName();
        hiveGenerator.RefreshAllCells();
    }

    public void BuildCooridor()
    {
        cell_Type = CellMesh.Cell_type.corridor;
        ArtPrefabsInstance art = ArtPrefabsInstance.Instance;
        BuildRoom(ArtPrefabsInstance.Instance.RoomPrefabs[2]);
        walkable = 1;
        isCellEmpty = false;

        hiveGenerator.RefreshAllCells();
    }

    public void BuildOutside()
    {
        cell_Type = CellMesh.Cell_type.outside;
        walkable = 1;
        mesh_index = -2;
        isCellEmpty = false;
    }

    public void BuildRoom(RoomContext context)
    {
        // automatically set cell type to room
        cell_Type = CellMesh.Cell_type.room;

        // place interior room 
        if (context == RoomContext.queen)
        {
            BuildRoom(ArtPrefabsInstance.Instance.RoomPrefabs[0]);
            hiveGenerator.hive_cell = this;
            return;
        }

        // any other room
        if (context == RoomContext.harvester)
        {
            BuildRoom(ArtPrefabsInstance.Instance.RoomPrefabs[1]);
        }

        if (context == RoomContext.war)
        {
            BuildRoom(ArtPrefabsInstance.Instance.RoomPrefabs[3]);
        }

        if (hiveGenerator.rooms.Contains(this) == false)
        {
            hiveGenerator.rooms.Add(this);
        }
    }

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
                childRoom.gameObject.AddComponent<CoreColorShader>();
            }
            walkable = 1;
            isCellEmpty = false;
        }
        else
        {
         //   Debug.LogError("Cell is already taken! You want to destroy?");
        }
    }

    public void DestroyRoom()
    {
        // if (isCellEmpty)
        // {
        //     Debug.LogError("Nothing to destroy!");
        // }
        // else
        // {
        hiveGenerator.rooms.Remove(this);

        cell_Type = CellMesh.Cell_type.dirt;
        mesh_index = 0;
        isCellEmpty = true;
        walkable = 0;
        Destroy(childRoom);


        hiveGenerator.RefreshAllCells();
        // }
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
