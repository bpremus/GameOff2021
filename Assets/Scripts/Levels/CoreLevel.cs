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

    public virtual void SetAvialableUnits() { }

    public virtual void SetAvialableRooms() { }


    // control move to next level
    public virtual bool IsTaskCompleted() { return false; }

    // control threats on each level
    public virtual void SpawnTreat() { }

    public void StartLevel(LevelManager manager)
    {
        levelManager = manager;
        SetGrid();
        SetCamera();
        SetAvialableRooms();
    }
    public void RunLevel()
    {
        SpawnTreat();
    }

    protected void DrawMask(int offset)
    {
        // get hive position 
        HiveCell hc = levelManager.hiveGenerator.hive_cell;
        List<List<HiveCell>> hive_cells = levelManager.hiveGenerator.cells;
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
                        cell.ResetMeshColor();
                        continue;
                    }
                    cell.SetMeshColor(0.6f, 0.6f, 0.6f);
                }
            }
        }
    }


}
