using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorLevel : CoreLevel
{
    [SerializeField] private List<int> restrictedBuilds;
    [SerializeField] private List<int> restrictedUnits;
    [SerializeField] string tasksHeader;
    [SerializeField] string objective1;


    public override void SetGrid()
    {
        Debug.Log("warrior level started");

        DrawMask(4);
        SetObjectives();
    }
    private void SetObjectives()
    {
        ObjectiveDisplay.Instance.DisplayNewObjectiveIndicator(tasksHeader);


        ObjectiveDisplay.Instance.SetTaskHeader(tasksHeader);
        ObjectiveDisplay.Instance.AddObjective(objective1);
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
        List<HiveCell> corridors = levelManager.hiveGenerator.GetAllCooridors();
        foreach (HiveCell cell in corridors)
        {
            List<CoreBug> bugs = cell.GetRoom().GetAssignedBugs();
            foreach (CoreBug bug in bugs)
            {
                WarriorBug wb = bug.GetComponent<WarriorBug>();
                {
                    if (wb)
                    {
                        OnLevelComplete();
                        return true; // task completed 
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
