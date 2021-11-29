using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CellSelectProto : MonoBehaviour
{
    public enum SelectState { none, bug_selected, cell_selected, build_cell, bug_assign };
    public SelectState selection_state = SelectState.none;
    [SerializeField]
    Transform frame_border;
    [SerializeField]
    GameObject path_sphere;
    [Header("GFX")]
    [SerializeField] private Sprite unselectedFrame;
    [SerializeField] private Sprite selectedFrame;
    private Vector3 borderStartSize;
    [SerializeField] private float onClickFrameAnimtime = 0.2f;
    [SerializeField] private LeanTweenType onClickFrameEase;

    [Header("Cell selected GFX")]
    [SerializeField] private GameObject cellSelected_GFX;
    private LTDescr cellSelectedAnim;

    [Header("Bug selected GFX")]
    [SerializeField] private GameObject bugSelected_GFX;

    [Header("Bug hover GFX")]
    [SerializeField] private GameObject bugHover_GFX;

    [Header("CellselectorCanvas")]
    [SerializeField] private GameObject cellHoverCanvas;
    [SerializeField] private GameObject occupiedSpotForRoomText_hover;

    [SerializeField] private GameObject cellSelectedCanvas;
    [SerializeField] private GameObject occupiedSpotForRoomText_selected;

    [SerializeField] private Color greenColor;
    [SerializeField] private Color redColor;

    [Header("Room and Bug detection ranges")]
    [SerializeField] private GameObject range_distance_ui;

    // changed to singleton 
    private static CellSelectProto _instance;
    public static CellSelectProto Instance
    {
        get { return _instance; }
    }
    Vector2 selectionPosition = Vector2.zero;

    private void Awake()
    {
    
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;

        borderStartSize = frame_border.transform.localScale;
    }

    int last_selected_room_id = 0;

    public void SetBuildID(int index)
    {
        last_selected_room_id = index;
        OnDeselect();
        selection_state = SelectState.build_cell;
    }
    public void ClearSelectionState() => selection_state = SelectState.none;
    public Transform GetFrameTransform() { return frame_border.transform; }

    // selection
    CoreBug  _hover_bug;
    HiveCell _hover_cell;
    CoreBug _bug_selected;
    HiveCell _cellSelected;
    #region Selector GFX
    //---------------------------------------------------------
    private void GFX_SelectorCellHover(HiveCell cell)
    {

        if (_hover_cell != null)
        {
            if (UIController.instance.inBuildingMode)
            {
                frame_border.gameObject.SetActive(true);

            }
            else
            {
                if (cell.cell_Type == CellMesh.Cell_type.dirt)
                {
                    frame_border.gameObject.SetActive(false);
                }
                else if (cell.cell_Type == CellMesh.Cell_type.room || cell.cell_Type == CellMesh.Cell_type.corridor)
                {
                    frame_border.gameObject.SetActive(true);
                }
            }

            frame_border.transform.position = _hover_cell.transform.position + new Vector3(0, 0, 1);
            SetDefaultFrameSize();
        }


    }
    public void HideBugSelector()
    {
        _hover_bug = null;
        _bug_selected = null;
    }
    private void GFX_SelectorBugHover()
    {

        //enable overlay shader on bug?

    }
    private void SetDefaultFrameSize()
    {
        frame_border.transform.localScale = borderStartSize;
    }

    private void GFX_SelectorBugSelect()
    {
        frame_border.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        //its circle over bug for now
        bugSelected_GFX.SetActive(true);
        bugSelected_GFX.GetComponent<CellSelector_Locked>().ScaleUp();
        //enable overlay shader on bug?
    }
    //-----------------------------------------------------------
    #endregion
    public void OnCellHover(HiveCell hc)
    {
        _hover_cell = hc;
        _hover_bug = null;

        GFX_SelectorCellHover(hc);
        
        if(selection_state == SelectState.bug_assign)
        {
            CheckForSpotToAssignGFX(hc);
        }
        else
        {
            cellHoverCanvas.SetActive(false);
        }
    }

    private string GetRoomCapacityInfo(HiveCell hc)
    {
        string text;
        if (hc.cell_Type == CellMesh.Cell_type.corridor || hc.cell_Type == CellMesh.Cell_type.room)
        {
            int max = hc.GetMaxAvailableSlots();
            int current = hc.GetLeftAvaiableSlots();
            text = current + "/" + max;
        }
        else text = null;

        return text;

    }
    private Color GetColorForRoomCapacity(HiveCell hc)
    {
        Color color = Color.white;
        int max = hc.GetMaxAvailableSlots();
        int current = hc.GetLeftAvaiableSlots();
        if (current == max) color = redColor;
        else color = greenColor;

        return color;
    }
    private void ShowRoomCapacityDisplay_Hover(HiveCell hc)
    {
        TextMeshProUGUI txtDisplayer = occupiedSpotForRoomText_hover.GetComponent<TextMeshProUGUI>();
        txtDisplayer.text = GetRoomCapacityInfo(hc);
        txtDisplayer.color = GetColorForRoomCapacity(hc);

        cellHoverCanvas.SetActive(true);
    }
    private void ShowRoomCapacityDisplay_Selected(HiveCell hc)
    {
        TextMeshProUGUI txtDisplayer = occupiedSpotForRoomText_selected.GetComponent<TextMeshProUGUI>();
        txtDisplayer.text = GetRoomCapacityInfo(hc);
        txtDisplayer.color = GetColorForRoomCapacity(hc);

        cellSelectedCanvas.SetActive(true);
    }

    private void HideRoomCapacityDisplay()
    {
        cellHoverCanvas.SetActive(true);
    }
    private void CheckForSpotToAssignGFX(HiveCell hc)
    {
       if(hc.cell_Type == CellMesh.Cell_type.corridor || hc.cell_Type == CellMesh.Cell_type.room)
        {
            int max = hc.GetMaxAvailableSlots();
            int current = hc.GetLeftAvaiableSlots();
            string text = current + "/" + max;
            TextMeshProUGUI txtDisplayer = occupiedSpotForRoomText_hover.GetComponent<TextMeshProUGUI>();

            if (current == max) txtDisplayer.color = redColor;
            else txtDisplayer.color = greenColor;

            txtDisplayer.text = text;
            cellHoverCanvas.SetActive(true);

        }
        else
        {
            cellHoverCanvas.SetActive(false);
        }

    }
    public void OnCellSelect(HiveCell hc)
    {
        _cellSelected = hc;
        selectionPosition = hc.transform.position;

        Room_UI.Instance.Show(hc);
        DBG_UnitUI.Instance.Hide();
        cellSelected_GFX.GetComponent<CellSelector_Locked>().ScaleUp();
        ShowRoomCapacityDisplay_Selected(hc);

        if (range_distance_ui)
        { 
            range_distance_ui.transform.position = hc.transform.position + new Vector3(0, 0, 1);
            float d = hc.GetRoom().GetRomRange();
            range_distance_ui.transform.localScale = new Vector3(d * 2, d *2, 0);
        }

        CoreRoom hroom = hc.GetRoom();
        if (hroom)
        {
            hroom.OnRoomSelect();
        }
    }

    public void OnBugHover(CoreBug bug)
    {
      //  Debug.Log("bug however");
        _hover_bug = bug;
        _hover_cell = null;
      

        GFX_SelectorBugHover();
    }
    public void OnBugSelect(CoreBug bug)
    {
        _bug_selected = bug;

        DBG_UnitUI.Instance.Show(bug);
        Room_UI.Instance.Hide();
        
        if (selection_state != SelectState.cell_selected) selectionPosition = bug.transform.position;

        bugSelected_GFX.transform.position = new Vector3(bug.transform.position.x, bug.transform.position.y, bugSelected_GFX.transform.position.z);
        GFX_SelectorBugSelect();
        //  SetUnitUIPosition();

        if (range_distance_ui)
        {

                range_distance_ui.transform.position = bug.transform.position + new Vector3(0, 0, 1);
                float d = bug.interraction_range;
                range_distance_ui.transform.localScale = new Vector3(d * 2, d * 2, 0);
            
        }

    }

    public void OnPlaceBuilding(HiveCell cell)
    {
       // Debug.Log("building placed");
        BuildOnCell(cell);
        selection_state = SelectState.none;
        CloseUI();
    }
    public void SetBuildRoomState()
    {
        selection_state = SelectState.build_cell;
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
        frame_border.transform.localScale = borderStartSize;
    }

    
    public void OnAssignBug(HiveCell destination)
    {
      //  Debug.Log("Asigning bugs");
      //  Debug.Log("Dest:" + destination.name);

        if (_bug_selected != null)
        {
            int max_assign = destination.GetAvailableAssignSlots();
            if (max_assign > 0)
            {
                if (destination.AssignDrone(_bug_selected))
                {
                    _bug_selected.GoTo(destination);
                }
            }
        }

        if (_cellSelected != null)
        {
            CoreRoom room = _cellSelected.GetRoom();
            if (room)
            {
                int max_assign = destination.GetAvailableAssignSlots();

                List<CoreBug> bugs = room.GetAssignedBugs();
                for (int i = 0; i < bugs.Count && i < max_assign; i++)
                {

                    Bug_batch_transfer bug_queue = new Bug_batch_transfer(bugs[i], destination);
                    bug_batch_assign.Enqueue(bug_queue);
                   // if (destination.AssignDrone(bugs[i]))
                   // {
                   //     bugs[i].GoTo(destination);
                   // }
                }
            }
        }

        selection_state = SelectState.none;
        DBG_UnitUI.Instance.Hide();
    }

    Queue<Bug_batch_transfer> bug_batch_assign = new Queue<Bug_batch_transfer>();
    struct Bug_batch_transfer 
    {
        CoreBug  bug;
        HiveCell destination;
        public Bug_batch_transfer(CoreBug bug, HiveCell destination)
        {
            this.bug = bug;
            this.destination = destination;
        }
        public void Assign()
        {
            if (destination.AssignDrone(bug))
            {
                bug.GoTo(destination);
            }
        }
    }

    float _t_separation = 0;
    protected void QueueSendBugs()
    {
        _t_separation += Time.deltaTime;
        if (bug_batch_assign.Count > 0)
        {
            if (_t_separation > 0.2f)
            {
                _t_separation = 0;
            }
            else
                return;

            bug_batch_assign.Dequeue().Assign();
        }
    }

    protected void BuildOnCell(HiveCell hc)
    {
       
        if (hc.cell_Type == CellMesh.Cell_type.dirt)
        {
            HiveCell.RoomContext context = HiveCell.RoomContext.empty;
            if (last_selected_room_id == 0)
            {
                hc.BuildCooridor();
                context = HiveCell.RoomContext.corridor;
                Debug.Log("Created corridor");

            }
            // if its a room 
            else if (last_selected_room_id == 1)
            {
                hc.BuildRoom(HiveCell.RoomContext.salvage);
                context = HiveCell.RoomContext.salvage;
                Debug.Log("Created Storage");
            }
            else if (last_selected_room_id == 2)
            {
                hc.BuildRoom(HiveCell.RoomContext.war);
                context = HiveCell.RoomContext.war;
                Debug.Log("Created barracks");
            }
            else if (last_selected_room_id == 3)
            {
                hc.BuildRoom(HiveCell.RoomContext.harvester);
                context = HiveCell.RoomContext.harvester;
                Debug.Log("Created harvester");
            }
            else if (last_selected_room_id == 4)
            {
                hc.BuildRoom(HiveCell.RoomContext.queen);
                context = HiveCell.RoomContext.queen;
                Debug.Log("Created Queen room");
            }

            GameController.Instance.OnRooomBuild(context);
            hc.BuildRoom();
            
           
        }
    }

    public void DestroyCell(HiveCell hc)
    {
        //
        hc.DestroyRoom();
        
        OnDeselect();
    }

    public bool CheckRoomConnections(HiveCell imacted_cell)
    {
        // hive generator, find all room 
        // cheheck form each room to exit if its ok 

 
        return false;
    }   

    public void OnDeselect()
    {
       // Debug.Log("deselect");
        _hover_bug = null;
        _hover_cell = null;
        _bug_selected = null;
        _cellSelected = null;

        DBG_UnitUI.Instance.Hide();
        Room_UI.Instance.Hide();

        frame_border.transform.position = new Vector3(0, 0, -5);
       // frame_border.GetComponent<SpriteRenderer>().sprite = unselectedFrame;
        frame_border.transform.localScale = borderStartSize;
        HideRoomCapacityDisplay();

        if (range_distance_ui)
        {
            range_distance_ui.transform.position = new Vector3(0, 0, -5);
            range_distance_ui.transform.localScale = Vector3.zero;
        }
                

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
    public void CloseUI()
    {
        if (UIController.instance.GetUIState() == UIController.State.Building) UIController.instance.SetDefaultState();
        Debug.Log("Closing All UI elements");
        selection_state = SelectState.none;
        UIController.instance.HideRoomUI();
        UIController.instance.CloseBuildMenu();
        UIController.instance.CloseSettingsMenu();
        UIController.instance.HideBugUI();
        OnDeselect();
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            CloseUI();
        }

        if (selection_state == SelectState.bug_assign)
        {
            DrawPath(_bug_selected, _hover_cell);
        }
        else
        {
            DrawPath(null, null);
        }
        if(selection_state == SelectState.none)
        {
            SetDefaultFrameSize();
        }

        if (_cellSelected)
        {
            cellSelected_GFX.SetActive(true);
            
            cellSelected_GFX.transform.position = _cellSelected.transform.position + new Vector3(0, 0, 1);
            bugSelected_GFX.SetActive(true);
        }
        else
        {
            cellSelected_GFX.SetActive(false);
        }

        if (_bug_selected)
        {
            bugSelected_GFX.transform.position = _bug_selected.transform.position + new Vector3(0,0,1);
            bugSelected_GFX.SetActive(true);
        }
        else bugSelected_GFX.SetActive(false);

        // send bugs that are in queue
        QueueSendBugs();


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
            if (UIController.instance.GetUIState() == UIController.State.Default)
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
                        //Hovering
                        if (selection_state == SelectState.build_cell)
                        {
                            // we can hover a building we want to make 
                            if (cell.cell_Type == CellMesh.Cell_type.dirt)
                            {
                                GhostRoomDisplayer.instance.Display_CanBuildHere();
                            }
                            else
                            {
                                GhostRoomDisplayer.instance.Display_CantBuildHere();
                            }
                        }
                        OnCellHover(cell);
                    }

                

            }
        }
    }

}
