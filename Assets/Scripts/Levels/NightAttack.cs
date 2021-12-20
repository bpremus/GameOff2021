using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightAttack : CoreLevel
{

    [SerializeField] private List<int> restrictedBuilds;
    [SerializeField] private List<int> restrictedUnits;

    [SerializeField] string tasksHeader;
    [SerializeField] string objective1;

    public override void SetGrid()
    {
        Debug.Log("NightAttack to start started");

        DrawMask(4);



        if (GameController.Instance.ISDayCycle())
        {
            objective1 = "Prepare for night";
        }
        else
        {
            objective1 = "Survive night";
        }
        SetObjectives();

    }
    private void Update()
    {

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


    float _t_spread = 0;

    #endregion
    public void SpawnNightAttack()
    {
        _t_spread += Time.deltaTime;
        if (_t_spread > 1)
        {
            _t_spread = 0;
        }
        else
        {
            return;
        }

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

    public override bool IsTaskCompleted()
    {
        if (GameController.Instance.ISDayCycle() == false)
        {
            SpawnNightAttack();
        }

        return false;
    }

    public override void OnLevelComplete()
    {
        ObjectiveDisplay.Instance.AllCompleted();
    }
}
