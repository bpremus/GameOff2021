using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellSelectProto : MonoBehaviour
{

    [SerializeField]
    Transform frame_border;
    [SerializeField]
    GameObject path_sphere;

    // changed to singleton 
    private static CellSelectProto _instance;
    public static CellSelectProto Instance
    {
        get { return _instance; }
    }

    public enum SelectState { none, bug_selected, cell_selected, build_cell, bug_assign};
    public SelectState selection_state = SelectState.none;

    private void Awake()
    {
    
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
    }

    int last_selected_room_id = 0;

    public void SetBuildID(int index)
    {
        last_selected_room_id = index;
        OnDeselect();
        selection_state = SelectState.build_cell;
    }

    public void OnCellSelect_dep()
    {
       // if (hc)
       // {
       //     if (hc.cell_Type == CellMesh.Cell_type.dirt)
       //     {
       //         if (last_selected_room_id == 0)
       //             hc.cell_Type = CellMesh.Cell_type.corridor;
       //         if (last_selected_room_id > 0)
       //             hc.cell_Type = CellMesh.Cell_type.room;
       //
       //         hc.BuildRoom();
       //     }
       //     else
       //     {
       //         Debug.Log("delete room");
       //         hc.cell_Type = CellMesh.Cell_type.dirt;
       //         hc.DestroyRoom();
       //         hc.BuildRoom();
       //
       //     }
       // }
    }

    // selection
    CoreBug  _hover_bug;
    HiveCell _hover_cell;
    CoreBug _bug_selected;
    HiveCell _cellSelected;

    public void OnCellHover(HiveCell hc)
    {
     //   Debug.Log("cell however");
        _hover_cell = hc;
        _hover_bug = null;

        frame_border.transform.position = hc.transform.position + new Vector3(0, 0, 1);
    }

    public void OnBugHover(CoreBug bug)
    {
      //  Debug.Log("bug however");
        _hover_bug = bug;
        _hover_cell = null;

        frame_border.transform.position = bug.transform.position + new Vector3(0, 0, 1);

    }

    public void OnCellSelect(HiveCell hc)
    {
        Debug.Log("cell select");
        _cellSelected = hc;

        DBG_RoomUI.Instance.Show(hc);
        DBG_UnitUI.Instance.Hide();
    }

    public void OnBugSelect(CoreBug bug)
    {
        Debug.Log("bug select");
        _bug_selected = bug;

        DBG_UnitUI.Instance.Show(bug);
        DBG_RoomUI.Instance.Hide();
    }
    public void OnPlaceBuilding(HiveCell cell)
    {
        Debug.Log("building placed");
        BuildOnCell(cell);
        selection_state = SelectState.none;
    }

    public void SetAssignBugState()
    {
        selection_state = SelectState.bug_assign;
    }

    public void SetBugSelection(CoreBug bug)
    {
        OnDeselect();
        OnBugSelect(bug);
        frame_border.transform.position = bug.transform.position + new Vector3(0, 0, 1);
    }

    public void SetRoomSelection(HiveCell cell)
    {
        OnDeselect();
        OnCellSelect(cell);
        frame_border.transform.position = cell.transform.position + new Vector3(0, 0, 1);
    }

    public void OnAssignBug(HiveCell destination)
    {
        Debug.Log("Asigning bugs");
       
        if (destination.AssignDrone(_bug_selected))
        {
            _bug_selected.GoTo(destination);
        }

        selection_state = SelectState.none;

        DBG_UnitUI.Instance.Hide();
    }

    protected void BuildOnCell(HiveCell hc)
    {
        if (hc.cell_Type == CellMesh.Cell_type.dirt)
        {
            if (last_selected_room_id == 0)
            {
                hc.BuildCooridor();
            }
            // if its a room 
            else if (last_selected_room_id == 1)
            {
                hc.BuildRoom(HiveCell.RoomContext.harvester);
            }
            else if (last_selected_room_id == 2)
            {
                hc.BuildRoom(HiveCell.RoomContext.war);
            }
            else if (last_selected_room_id >= 3)
            {
                hc.BuildRoom(HiveCell.RoomContext.harvester);
            }

            hc.BuildRoom();      
        }
    }

    public void SetDestroyRoom(HiveCell cell)
    {
        cell.DestroyRoom();
        OnDeselect();
    }


    public bool CheckRoomConnections(HiveCell imacted_cell)
    {
       

        // hive generator, find all room 
        // cheheck form each room to exit if its ok 

        QueenRoom qr = imacted_cell.GetComponent<QueenRoom>();
        if (qr)
        { 
        
        }
        imacted_cell.walkable = 0;



        imacted_cell.walkable = 1;
        return false;
    }


    public void OnDeselect()
    {
        _hover_bug = null;
        _hover_cell = null;
        _bug_selected = null;
        _cellSelected = null;

        DBG_UnitUI.Instance.Hide();
        DBG_RoomUI.Instance.Hide();

        frame_border.transform.position = new Vector3(0, 0, -5);

    }

    private void OnDrawGizmos()
    {
        if (_hover_bug)
        {
            Gizmos.DrawWireSphere(_hover_bug.transform.position, 1);
        }
        if (_hover_cell)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_hover_cell.transform.position, 1);
        }

        if (_bug_selected)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(_bug_selected.transform.position, 1);
        }

        if (_cellSelected)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_cellSelected.transform.position, 1);
        }
    }

    List<GameObject> path_spheres = new List<GameObject>();
    public void DrawPath(CoreBug bug, HiveCell destination)
    {
        for (int i = 0; i < path_spheres.Count; i++)
        {
            Destroy(path_spheres[i].gameObject);
        }
      
        if (destination == null) return;
        if (bug == null) return;

        path_spheres.Clear();

        HiveCell start = bug.current_cell;
        List<HiveCell> path = AiController.GetPath(start, destination);
        for (int i = 1; i < path.Count; i++)
        {
            Vector3 pos = path[i].transform.position + new Vector3(0, 0, 1);
            GameObject g = Instantiate(path_sphere, pos, Quaternion.identity);
            path_spheres.Add(g);
        }
    }

    public void OnMapOpen()
    { 
    
    }

    public void OnMapHover(WorldMapCell map)
    {
        map.OnHover();
    }

    public void OnMapSelect(WorldMapCell selected_map)
    {


        OnDeselect();
    }


    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Debug.Log("ESC");
            selection_state = SelectState.none;
            OnDeselect();
        }

        if (selection_state == SelectState.bug_assign)
        {
            DrawPath(_bug_selected, _hover_cell);
        }
        else
        {
            DrawPath(null, null);
        }


        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        LayerMask selection_mask = 0;
        //selection_mask = 1 << 6 | 1 << 7;
        bool isOverUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
        if (isOverUI)
        {
            frame_border.transform.position = new Vector3(0, 0, -5);
        }

        if (Physics.Raycast(ray, out hit) && !isOverUI)
        {
            WorldMapCell map = hit.collider.transform.GetComponent<WorldMapCell>();
            if (map)
            {
                OnMapHover(map);
                return;
            }

            CoreBug bug = hit.collider.transform.GetComponent<CoreBug>();
            if (bug)
            {
                if (bug.GetState() == BugMovement.BugAnimation.dead) return;

                if (Input.GetMouseButtonDown(0))
                {
                    selection_state = SelectState.bug_selected;
                    OnDeselect();
                    OnBugSelect(bug);
                }
                else
                    OnBugHover(bug);
            }

            HiveCell cell = hit.collider.transform.GetComponent<HiveCell>();
            if (cell)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    // new empty cell 
                    if (cell.cell_Type == CellMesh.Cell_type.dirt)
                    {
                        if (selection_state == SelectState.none)
                        {
                            // click on empty cell
                        }
                        else if (selection_state == SelectState.cell_selected)
                        {
                            // click on empty cell
                            selection_state = SelectState.none;
                            OnDeselect();
                        }
                        else if (selection_state == SelectState.bug_selected)
                        {
                            // click on empty cell
                            selection_state = SelectState.none;
                            OnDeselect();
                        }
                        else if (selection_state == SelectState.build_cell)
                        {
                            OnDeselect();
                            OnPlaceBuilding(cell);
                        }
                        else if (selection_state == SelectState.bug_assign)
                        {
                            selection_state = SelectState.none;
                            OnDeselect();
                        }

                    }
                    // we have clicked inside room 
                    if (cell.cell_Type == CellMesh.Cell_type.corridor || cell.cell_Type == CellMesh.Cell_type.room)
                    {
                        if (selection_state == SelectState.none)
                        {
                            selection_state = SelectState.cell_selected;
                            OnCellSelect(cell);
                        }
                        else if (selection_state == SelectState.cell_selected)
                        {
                            // reselect
                            selection_state = SelectState.cell_selected;
                            OnCellSelect(cell);
                        }
                        else if (selection_state == SelectState.bug_selected)
                        {
                            // reselect
                            selection_state = SelectState.cell_selected;
                            OnDeselect();
                            OnCellSelect(cell);
                        }
                        else if (selection_state == SelectState.bug_assign)
                        {
                            OnAssignBug(cell);
                        }
                    }
                }
                else
                {
                    if (selection_state == SelectState.build_cell)
                    { 
                        // we can hover a building we want to make 
                    }

                    OnCellHover(cell);
                }
                   

            }
        }
    }

}
