using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorLevel : CoreLevel
{
    private const string NLS_OnSuccess = "Task completed successfully";


    [SerializeField] private List<int> restrictedBuilds;
    [SerializeField] private List<int> restrictedUnits;

    #region UnitRestriction
    // limit unit evolution 
    public override void SetAvialableUnits()
    {
        levelManager.uiController.RestrictUnits(restrictedUnits, true);
    }
    #endregion

    public override void SetGrid()
    {
        Debug.Log("first level started");

        GameLog.Instance.WriteLine("New objective");
        GameLog.Instance.WriteLine("Build a warrior bug to protect the hive");

        DrawMask(4);

        List<HiveCell> rooms = levelManager.hiveGenerator.GetAllRooms();
        for (int i = 0; i < rooms.Count; i++)
        {
            if (rooms[i].room_context == HiveCell.RoomContext.harvester)
            {
                EnemyController.Instance.SapwnScount(CoreBug.BugEvolution.ai_scout, rooms[i]);
            }
        }
        EnemyController.Instance.SapwnScount(CoreBug.BugEvolution.ai_scout, levelManager.hiveGenerator.GetHiveQueenRoom());

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
        GameLog.Instance.WriteLine(NLS_OnSuccess);
    }

}
