using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstLevel : CoreLevel
{
    public override void SetGrid()
    {
        Debug.Log("first level started");

        GameLog.Instance.WriteLine("New objective");
        GameLog.Instance.WriteLine("Build Harvesting building");

        DrawMask(4);

        // message to log 
        GameLog.Instance.WriteLine("New objective");
        GameLog.Instance.WriteLine("Build a harvesting room");
        GameLog.Instance.WriteLine("Assign drones to harvesting room");
        GameLog.Instance.WriteLine("Send drones to collect food and wood");
    }

    public override bool IsTaskCompleted()
    {
        List<HiveCell> rooms = levelManager.hiveGenerator.GetAllRooms();
        for (int i = 0; i < rooms.Count; i++)
        {
            HarversterRoom hr = rooms[i].GetRoom().GetComponent<HarversterRoom>();
            if (hr)
            {
                List<CoreBug> bugs = hr.GetAssignedBugs();
                if (bugs.Count > 0)
                {
                    if (hr.gather_destination != null)
                    {
                        OnLevelComplete();
                        return true;
                    }
                }
            }
        }

        return false;
    }

    public override void OnLevelComplete()
    {
        GameLog.Instance.WriteLine("Task completed sucessfully");
    }

}
