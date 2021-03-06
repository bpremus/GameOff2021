using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreLevel : MonoBehaviour
{
 
    // this class will describe the level layout, buildings and threats
    protected LevelManager levelManager;
    //events 
    public virtual void OnLevelComplete() { }

    // level configuration (called once)
    public virtual void SetGrid() { }

    public virtual void SetCamera() { }

    //0 super-worker /1 warrior / 2 claw / 3 range /4 cc
    public virtual void SetAvialableUnits() { }

    // 0 corridor /1 harvester /2 salvage  /3 barracks /4 hatchery
    public virtual void SetAvialableRooms() { }

    public virtual void UnlockRestrictedRoom(int roomID) { }

    public virtual void RestrictRoomBuild(int roomID) { }
    public virtual void SwitchRoomBuildRestriction(int roomID) { }

    // control move to next level
    public virtual bool IsTaskCompleted() { return false; }

    // control threats on each level
    public virtual void SpawnThread() { }
    public void StartLevel(LevelManager manager)
    {
        levelManager = manager;
        SetGrid();
        SetCamera();
        SetAvialableRooms();
        SetAvialableUnits();
    }
    public void RunLevel()
    {
        SpawnThread();
    }

    protected void DrawMask(int offset)
    {
        return;

        // get hive position 
        HiveCell hc = levelManager.hiveGenerator.GetHiveQueenRoom();
        List<List<HiveCell>> hive_cells = levelManager.hiveGenerator.GetAllCells();
        for (int i = 0; i < hive_cells.Count; i++)
        {
            for (int j = 0; j < hive_cells[i].Count; j++)
            {
                HiveCell cell = hive_cells[i][j];
                if (cell)
                {
                    int ii = hc.x;
                    int jj = hc.x;
                    if (Mathf.Abs(i - jj) < offset)
                    {
                        cell.IsBuildRestricted = false;
                        continue;
                    }
                    cell.IsBuildRestricted = true;
                }
            }
        }

       levelManager.hiveGenerator.RedrawGrid();
    }


}
