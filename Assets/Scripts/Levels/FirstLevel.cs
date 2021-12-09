using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstLevel : CoreLevel
{
    [SerializeField] private List<int> restrictedBuilds;
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
    #region RoomRestrictions
    public override void SetAvialableRooms()
    {
        levelManager.uiController.RestrictBuilds(restrictedBuilds); // <- this solution restricts rooms instead of hiding them
                                                                    //   levelManager.uiController.DisableBuildCards();
                                                                    //   levelManager.uiController.EnableUIElement(UIController.UIElements.build_corridor);
                                                                    //   levelManager.uiController.EnableUIElement(UIController.UIElements.build_harvester);
    }
    public override void UnlockRestrictedRoom(int roomid)
    {

        if (!IsRoomBuildLocked(roomid))
        {
            restrictedBuilds.Remove(roomid);
            Debug.Log(roomid + "'s room is now unlocked");
        }
        else
        {
            Debug.LogWarning("Room " + roomid + " is already unlocked");
        }

        SetAvialableRooms();

    }
    public override void RestrictRoomBuild(int roomid)
    {
        if (IsRoomBuildLocked(roomid)) return;

        restrictedBuilds.Add(roomid);
        SetAvialableRooms();
        Debug.Log(roomid + "'s room is now locked");
    }
    public override void SwitchRoomBuildRestriction(int roomid)
    {
        if (IsRoomBuildLocked(roomid))
        {
            restrictedBuilds.Remove(roomid);
            Debug.Log(roomid + "'s room is now unlocked");
        }
        else
        {
            restrictedBuilds.Add(roomid);
            Debug.Log(roomid + "'s room is now locked");
        }
        SetAvialableRooms();
    }
    private bool IsRoomBuildLocked(int roomid)
    {
        if (restrictedBuilds.Contains(roomid)) return true;
        return false;
    }
    #endregion
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
