using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLevel : CoreLevel
{
    [SerializeField] int food_objective = 100;
    [SerializeField] int wood_objective = 10;

    public override void SetGrid() {
        Debug.Log("tutorial started");

        // starting resources 
        GameController.Instance.SetResrouces(20, 0);

        // stop spending resources 
        GameController.Instance.SetFoodTick(300); // every 20 sec 1 tick

        // build mask, how far we can build
        DrawMask(2);

        // message to log 
        GameLog.Instance.WriteLine("New objective");
        GameLog.Instance.WriteLine("Gather " + food_objective + " food");
        GameLog.Instance.WriteLine("Gather " + wood_objective + " wood");
    }

    // focus camera when game start on hive
    public override void SetCamera() {

        HiveCell hc = levelManager.hiveGenerator.hive_cell;
        CameraController cam_controller = Camera.main.GetComponent<CameraController>();
        cam_controller.SetFocus(hc);
    }

    // limit unit evolution 
    public override void SetAvialableUnits() { }

    // limit ui elements user can click
    public override void SetAvialableRooms() 
    {
        // disable build elements 
        levelManager.uiController.DisableBuildCards();
        levelManager.uiController.EnableUIElement(UIController.UIElements.build_corridor);
        levelManager.uiController.EnableUIElement(UIController.UIElements.build_harvester);

        // disable all room buttons 
        // enable queen create drone 
        // enable queen send on task 

        // disable all drone ui 
        // enable assign bug 
        // enable fallow 

    }

    // if goal is completed (x food, y wood) go to next level
    public override bool IsTaskCompleted() {

        int food = GameController.Instance.GetFood();
        int wood = GameController.Instance.GetWood();

        if (food >= food_objective && wood >= wood_objective)
        {
            OnLevelComplete();


            return true;
        }

        return false;
    }

    public override void OnLevelComplete()
    {
        GameLog.Instance.WriteLine("Task completed sucessfully");
    }

}
