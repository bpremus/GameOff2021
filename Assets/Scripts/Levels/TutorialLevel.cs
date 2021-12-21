using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLevel : CoreLevel
{
    [SerializeField] int food_objective = 100;
    [SerializeField] int wood_objective = 10;
    [SerializeField] int population_objective = 5;
    [SerializeField] private List<int> restrictedBuilds;
    [SerializeField] private List<int> restrictedUnits;
    [SerializeField] string tasksHeader;
    [SerializeField] string objective1;
    [SerializeField] string objective2;
    [SerializeField] string objective3;


    CameraController cam_controller;
    public List<int> GetRestrictedList()
    {
        return restrictedBuilds;
    }
    private void Awake()
    {
        cam_controller = Camera.main.GetComponent<CameraController>();
    }
    public override void SetGrid() {
        Debug.Log("tutorial started");

        // starting resources 
        //GameController.Instance.SetResrouces(20, 0);

        // stop spending resources 
        GameController.Instance.SetFoodTick(300); // every 20 sec 1 tick

        // build mask, how far we can build
        DrawMask(2);

        SetObjectives();


    }
    private void SetObjectives()
    {
        tasksHeader = "New beginnings";
        objective1 = "Gather " + food_objective + " food";
        objective2 = "Gather " + wood_objective + " wood";
        objective3 = "Create "+ (population_objective - GameController.Instance.GetPopulation()) +" workers";

        ObjectiveDisplay.Instance.DisplayNewObjectiveIndicator(tasksHeader);


        ObjectiveDisplay.Instance.SetTaskHeader(tasksHeader);
        ObjectiveDisplay.Instance.AddObjective(objective3);
        ObjectiveDisplay.Instance.AddObjective(objective1);
        ObjectiveDisplay.Instance.AddObjective(objective2);
    }
    // focus camera when game start on hive
    public override void SetCamera() {

        HiveCell hc = levelManager.hiveGenerator.GetHiveQueenRoom();

        cam_controller.SetFocus(hc);
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
            Debug.LogWarning("Room "+ roomid+" is already unlocked");
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
    // if goal is completed (x food, y wood) go to next level
    public override bool IsTaskCompleted() {


        int food = GameController.Instance.GetFood();
        int wood = GameController.Instance.GetWood();
        int population = GameController.Instance.GetPopulation();
        if (food >= food_objective) ObjectiveDisplay.Instance.ObjectiveCompleted(objective1);
        if (wood >= wood_objective) ObjectiveDisplay.Instance.ObjectiveCompleted(objective2);
        if(population >= 5) ObjectiveDisplay.Instance.ObjectiveCompleted(objective3);
        if (food >= food_objective && wood >= wood_objective && population >= 5)
        {
            OnLevelComplete();
            return true;
        }
        return false;
    }

    public override void OnLevelComplete()
    {
        ObjectiveDisplay.Instance.AllCompleted();
    }

}
