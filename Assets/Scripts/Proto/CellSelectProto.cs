using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellSelectProto : MonoBehaviour
{

    // changed to singleton 
    private static CellSelectProto _instance;
    public static CellSelectProto Instance
    {
        get { return _instance; }
    }

    public enum SelectState { none, bug_selected, empty_cell_selected, room_selected, corridor_selected};
    public SelectState selection_state;

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
    HiveCell hc = null;

    public void SetBuildID(int index)
    {
        last_selected_room_id = index;
    }

    public void OnCellSelect()
    {
        if (hc)
        {
            if (hc.cell_Type == CellMesh.Cell_type.dirt)
            {
                if (last_selected_room_id == 0)
                    hc.cell_Type = CellMesh.Cell_type.corridor;
                if (last_selected_room_id > 0)
                    hc.cell_Type = CellMesh.Cell_type.room;

                hc.BuildRoom();
            }
            else
            {
                Debug.Log("delete room");
                hc.cell_Type = CellMesh.Cell_type.dirt;
                hc.DestroyRoom();
                hc.BuildRoom();

            }
        }
    }

    public void OnCellHover()
    {
        if (hc)
        {
           // Debug.Log(hc.name);
            DumpPath(hc);
        }
    }


    public void OnBugHover(CoreBug bug)
    {
        Debug.Log("bug however");
    }
    
 
    private void DumpPath(HiveCell destination)
    {
        Debug.DrawLine(destination.transform.position, destination.hiveGenerator.cells[9][9].transform.position);

        List<HiveCell> dbg_path =  AiController.GetPath(destination, destination.hiveGenerator.cells[9][9]);
        if (dbg_path.Count == 0) return;

       

        Vector3 p = dbg_path[0].transform.position;
        for (int i = 1; i < dbg_path.Count; i++)
        {
            Vector3 p2 = dbg_path[i].transform.position;

            Debug.DrawLine(p, p2, Color.green);
            p = p2;
        }
    }

    CoreBug _selected_bug;
    private void OnDrawGizmos()
    {
        
    }

    private void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            CoreBug bug = hit.transform.GetComponent<CoreBug>();
            if (bug)
            {
                OnBugHover(bug);
                
            }


            GameObject objectHit = hit.transform.gameObject;
            if (objectHit.layer == 6) //cell
            {
                hc = objectHit.GetComponent<HiveCell>();

                //aiming at cell and clicked on it
                if (Input.GetMouseButtonDown(0))
                {
                    OnCellSelect();
                }
                else
                {
                    OnCellHover();
                }
            }
        }
    }

}
