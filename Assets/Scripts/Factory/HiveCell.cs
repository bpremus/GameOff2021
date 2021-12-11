using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class HiveCell : MonoBehaviour
{
    // Save and Load 
    // ----------------------------
    [System.Serializable]
    public class SaveHiveCell
    {
        public bool is_static_cell;

        public int x;
        public int y;
        public int gCost;
        public int hCost;
        public int walkable;
        public int dCost;

        public Cell_state cell_state;
        public RoomContext room_context;
        public CoreRoom.SaveCoreRoom child_room;
    }

    public SaveHiveCell GetSaveData()
    {
        SaveHiveCell data = new SaveHiveCell();

        data.is_static_cell = this.is_static_cell;
        data.x              = this.x;
        data.y              = this.y;
        data.gCost          = this.gCost;
        data.hCost          = this.hCost;
        data.walkable       = this.walkable;
        data.dCost          = this.dCost;

        data.cell_state     = this.cell_state;
        data.room_context   = this.room_context;
        data.child_room     = this.childRoom.GetSaveData();

        return data;
    }

    // cell still has a room that inside have a room mesh 
    public HiveGenerator hiveGenerator = null;

    // path-finding 
    // -------------------------
    public int x;
    public int y;
    public int gCost;
    public int hCost;
    public int walkable = 0;

    // death cost for enemy bugs 
    public int dCost = 0;

    public bool isCellEmpty;
    [SerializeField] protected CoreRoom childRoom;
    [SerializeField] public CellMesh cell_mesh;

    public enum Cell_state { none, disabled, enabled };
    public Cell_state cell_state = Cell_state.none;

    // this will determine what kind of cell mesh (walls) are going to be drawn
    public CellMesh.Cell_type cell_Type = CellMesh.Cell_type.dirt;
    public enum RoomContext { empty, hive, queen, harvester, war, salvage, corridor, entrance };
    public RoomContext room_context = RoomContext.empty;

    public void SetMeshColor(float r, float g, float b)
    {
        if (cell_mesh == null) return;
     
        MaterialPropertyBlock  block = new MaterialPropertyBlock();
        Renderer renderer = cell_mesh.GetComponentInChildren<Renderer>();
        if (renderer)
        {
            // Debug.Log("setting color");
            Color cell_color = new Color(r, g, b);
            block.SetColor("_baseColor", cell_color);
            renderer.SetPropertyBlock(block);
        }
    }

    public void ResetMeshColor()
    {
        if (cell_mesh == null) return;

        MaterialPropertyBlock block = new MaterialPropertyBlock();
        Renderer renderer = cell_mesh.GetComponentInChildren<Renderer>();
        if (renderer)
        {
            block.Clear();
            renderer.SetPropertyBlock(block);
        }
    }

    // -----------------------------------------------------------------------
    public bool is_static_cell = false;

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
        // Debug.Log("assinging a bug to a new room");
        bugs_to_assign.Enqueue(bug);
        return true;
    }

    public int GetAvailableAssignSlots()
    {
        return GetMaxAvailableSlots() - GetLeftAvaiableSlots();
    }

    public int GetMaxAvailableSlots()
    {
       
        return childRoom.GetMAxAssignUnits();
    }

    public int GetLeftAvaiableSlots()
    {
        return childRoom.GetAssignedBugs().Count;
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

    public bool IsInTheRoomRange(Vector3 target)
    {
        if (childRoom == null) return false;
        return childRoom.IsInTheRoomRange(target);
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

    public HiveGenerator SetGenerator
    {
        set => hiveGenerator = value;
    }

    #region Building 
    public virtual void OnRoomPlaced()
    {
        Debug.Log("Room has been built");
    }

    public virtual void OnRoomDestroyed()
    {
        Debug.Log("Room has been destroyed");
    }

    public void BuildEntrance()
    {
        cell_Type = CellMesh.Cell_type.entrance;
        ArtPrefabsInstance art = ArtPrefabsInstance.Instance;
        BuildRoom(ArtPrefabsInstance.Instance.RoomPrefabs[6]);
        walkable = 1;
        isCellEmpty = false;
        room_context = RoomContext.entrance;
        hiveGenerator.SetHiveRoom(this);
        hiveGenerator.RefreshAllCells();
    }

    public void BuildCooridor()
    {
        cell_Type = CellMesh.Cell_type.corridor;
        ArtPrefabsInstance art = ArtPrefabsInstance.Instance;
        BuildRoom(ArtPrefabsInstance.Instance.RoomPrefabs[2]);
        walkable = 1;
        isCellEmpty = false;
        room_context = RoomContext.corridor;

        if (hiveGenerator.isGameStarted)
            OnRoomPlaced();

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

        // register rooms
        hiveGenerator.SetHiveRoom(this);

        // place interior room 
        if (context == RoomContext.hive)
        {
            BuildRoom(ArtPrefabsInstance.Instance.RoomPrefabs[0]);
            room_context = RoomContext.hive;
        } else 
        if (context == RoomContext.queen)
        {
            BuildRoom(ArtPrefabsInstance.Instance.RoomPrefabs[5]);
            room_context = RoomContext.queen;
        } else 
        if (context == RoomContext.harvester)
        {
            BuildRoom(ArtPrefabsInstance.Instance.RoomPrefabs[1]);
            room_context = RoomContext.harvester;
        } else
        if (context == RoomContext.war)
        {
            BuildRoom(ArtPrefabsInstance.Instance.RoomPrefabs[3]);
            room_context = RoomContext.war;
        } else 
        if (context == RoomContext.salvage)
        {
            BuildRoom(ArtPrefabsInstance.Instance.RoomPrefabs[4]);
            room_context = RoomContext.salvage;
        }
 
        if (hiveGenerator.isGameStarted)
            OnRoomPlaced();


        hiveGenerator.RefreshAllCells();
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

    public bool CanDestroyRoom()
    {
        if (childRoom)
        {
            QueenRoom qr = childRoom.GetComponent<QueenRoom>();
            if (qr)
            {
                // cant delete queen room
                return false;
            }
        }
        return true;  // not working (?)

        walkable = 0;
        HiveCell entrace = hiveGenerator.GetAllEntrances()[0];
        Debug.Log("checking path from" + entrace.name);

        bool can_destroy = true;
        List<HiveCell> get_all_rooms = hiveGenerator.GetAllRooms();
        for (int i = 0; i < get_all_rooms.Count; i++)
        {
            Debug.Log("is " + get_all_rooms[i].GetRoom().name + " connected to exit");
            if (get_all_rooms[i] == this) continue;
            List <HiveCell> cells = AiController.GetPath(entrace, get_all_rooms[i]);
            int path_size = cells.Count;
            if (path_size == 0)
            {          
                can_destroy = false;
                break;
            }
        }
        walkable = 1;
        return can_destroy;
    }

    public void DestroyRoom(bool force = false)
    {
        Debug.Log("Destroy room");
        if (CanDestroyRoom() == false && force == false)
        {
            Debug.Log("Room cannot be destroyed");
            return;
        }
        if (childRoom)
        {
            List<CoreBug> bugs = childRoom.GetAssignedBugs();
            Destroy(childRoom.gameObject);

            for (int i = 0; i < bugs.Count; i++)
            {
                //bugs[i].GoTo(hiveGenerator.hive_entrance[0]);
                Debug.LogError("FIX ME");
            }     
        }

        // remove actual room rep
        //hiveGenerator.RemoveRoom(this);

        // replace with dirt
        room_context = RoomContext.empty;
        cell_Type = CellMesh.Cell_type.dirt;
        mesh_index = 0;
        isCellEmpty = true;
        walkable = 0;
        if (hiveGenerator.isGameStarted)
            OnRoomDestroyed();

        hiveGenerator.RefreshAllCells();
    }
    #endregion
    // drawing the room mesh 
    #region room_mesh

    public int GetX { get => x; }
    public int GetY { get => y; }

    public int mesh_index = -1;
    int[] connections = new int[4] { -1, -1, -1, -1 };

    public bool IsBuildRestricted = false;

    public void UpdateColors()
    {
        if (IsBuildRestricted == true)
            SetMeshColor(0.6f, 0.6f, 0.6f);
        else
            ResetMeshColor();
    }

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
    // Everything regarding path-finding 
    #region path_finding 

    public int fCost
    {
        get => gCost + hCost;
    }

    public int fEnemyCost
    {
        get => gCost + hCost + dCost;
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
