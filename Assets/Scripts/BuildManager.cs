using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    [SerializeField] private GameObject[] room_prefabs;
    private GameObject selectedCell;
    private HiveCell cell;
    private CellSelection cellSelection;

    private void Awake()
    {
        cellSelection = FindObjectOfType<CellSelection>();
    }
    public void SetCell(GameObject selectedCell) 
    { 
        this.selectedCell = selectedCell;
        cell = selectedCell.GetComponent<HiveCell>();
    }

    public void CreateNewRoom(int roomInd)
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
        cellSelection.Unselect();
    }
}
