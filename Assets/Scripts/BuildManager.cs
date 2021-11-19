using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    // changed to singleton 
    private static BuildManager _instance;
    public static BuildManager Instance
    {
        get { return _instance; }
    }

    [SerializeField] 
    private GameObject[] room_prefabs;
    private GameObject selectedCell;
    private HiveCell cell;
    private CellSelection cellSelection;

    private void Awake()
    {
        cellSelection = FindObjectOfType<CellSelection>();

        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
    }
    public void SetCell(GameObject selectedCell)
    {
        this.selectedCell = selectedCell;
        cell = selectedCell.GetComponent<HiveCell>();
    }
    public void SetCell(HiveCell selectedCell)
    {
        this.selectedCell = selectedCell.gameObject;
        cell = selectedCell;
    }

    public void CreateNewRoom(int roomInd)
    {
        if (roomInd > room_prefabs.Length || roomInd < 0) { Debug.LogError("Room id with" + roomInd + " does not exists"); return; }

        if (CellSelected())
        {
            if (cell.isCellEmpty)
            {
                //check needed resources/costs etc...

                //bps1: check also if room is connected to ousdie world, no orphan rooms

                //Build room
                cell.BuildRoom(room_prefabs[roomInd]);
                ClearSelection();
            }
        }
    }
    public void CreateCorridor(int roomInd)
    {
        if (roomInd > room_prefabs.Length || roomInd < 0) { Debug.LogError("Room id with" + roomInd + " does not exists"); return; }

        if (CellSelected())
        {
            if (cell.isCellEmpty)
            {
                //check needed resources/costs etc...

                //Build room
                cell.BuildRoom(room_prefabs[roomInd]);
                ClearSelection();
            }
        }
    }
    public void DestroyRoom()
    {
        if (CellSelected())
        {
            if (!cell.isCellEmpty)
            {
                cell.DestroyRoom();
                ClearSelection();
            }

        }
    }

    private bool CellSelected()
    {
        if (selectedCell == null) { Debug.LogError("No cell selected!"); return false; }
        return true;
    }
    private void ClearSelection()
    {
        selectedCell = null;
        if (cellSelection != null)
            cellSelection.Unselect();
    }



}
