using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstLevel : CoreLevel
{
    [SerializeField] private List<int> restrictedBuilds;
    [SerializeField] private List<int> restrictedUnits;


    [SerializeField] string tasksHeader = "Harvesting";
    [SerializeField] string objective1 = "Build a harvesting room";
    [SerializeField] string objective2 = "Assign 3 workers to harvesting room";
    [SerializeField] string objective3 = "Send workers to collect food or wood";
    public override void SetGrid()
    {
        Debug.Log("first level started");


        DrawMask(4);

        ObjectiveDisplay.Instance.DisplayNewObjectiveIndicator(tasksHeader);

        ObjectiveDisplay.Instance.SetTaskHeader(tasksHeader);
        ObjectiveDisplay.Instance.AddObjective(objective1);
        ObjectiveDisplay.Instance.AddObjective(objective2);
        ObjectiveDisplay.Instance.AddObjective(objective3);


    }
    #region UnitRestriction
    // limit unit evolution 
    public override void SetAvialableUnits()
    {
        levelManager.uiController.RestrictUnits(restrictedUnits, true);
    }
    #endregion
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
                ObjectiveDisplay.Instance.ObjectiveCompleted(objective1);
                List<CoreBug> bugs = hr.GetAssignedBugs();
                if (bugs.Count > 2)
                {
                    ObjectiveDisplay.Instance.ObjectiveCompleted(objective2);
                    if (hr.gather_destination != null)
                    {
                        ObjectiveDisplay.Instance.ObjectiveCompleted(objective3);
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
        ObjectiveDisplay.Instance.AllCompleted();
    }

}
