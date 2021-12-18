using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    //IDs: 
    // 0 - corridor   1 - storage room  2 - warriors room  3 - resource room  4- queen room
    [SerializeField] private int[] roomsIds;
    [SerializeField] private int[,] roomsCost; // roomCost[food,wood]

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

    public bool CanBuildRoom(int _roomInd)
    {
        return  GameController.Instance.EnoughResources(GameController.Instance.GetRoomCost(_roomInd));
    }
    public bool TryBuild(int _roomInd)
    {
        if(CanBuildRoom(_roomInd)) CellSelectProto.Instance.SetBuildID(_roomInd);
        return CanBuildRoom(_roomInd);
    }
    public void CreateNewRoom(int roomInd)
    {
        if (roomInd > room_prefabs.Length || roomInd < 0) { Debug.LogError("Room id with" + roomInd + " does not exists"); return; }

        if (CellSelected())
        {
            if (cell.isCellEmpty)
            {
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
