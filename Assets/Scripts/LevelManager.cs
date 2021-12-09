using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private bool _debugCheats;
    [SerializeField]
    HiveGenerator hive_generator;

    [SerializeField] 
    UIController ui_controller;

    [SerializeField]
    CoreLevel[] game_levels;

    CoreLevel curent_level = null;
    private int level_index = 0;
    private void Start()
    {
        if (_debugCheats) { Debug.LogWarning("CHEATS ARE ENABLED!"); }
    }
    private void RunLevel()
    {
        if (curent_level == null)
        {
            curent_level = game_levels[level_index];
            curent_level.StartLevel(this);
        }

        if (curent_level != null)
        {
            curent_level.RunLevel();
            if (curent_level.IsTaskCompleted() == true)
            {
                level_index++;
                curent_level = game_levels[level_index];
                curent_level.StartLevel(this);
            }
        }
    }

    private void Update()
    {
        RunLevel();
        if (_debugCheats) DebugCheats();
    }
    private void DebugCheats()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            curent_level.SwitchRoomBuildRestriction(0);
            ActionLogger.Instance.AddLog("[CHEAT] Switched restriction of roomID 0",2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            curent_level.SwitchRoomBuildRestriction(1);
            ActionLogger.Instance.AddLog("[CHEAT] Switched restriction of roomID 1",2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            curent_level.SwitchRoomBuildRestriction(2);
            ActionLogger.Instance.AddLog("[CHEAT] Switched restriction of roomID 2",2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            curent_level.SwitchRoomBuildRestriction(3);
            ActionLogger.Instance.AddLog("[CHEAT] Switched restriction of roomID 3",2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            curent_level.SwitchRoomBuildRestriction(4);
            ActionLogger.Instance.AddLog("[CHEAT] Switched restriction of roomID 4",2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            int foodtoAdd = 10;
            int woodtoAdd = 5;
            GameController.Instance.SetResources(GameController.Instance.GetFood() + foodtoAdd, GameController.Instance.GetWood() + woodtoAdd);
            ActionLogger.Instance.AddLog("[CHEAT] Added " + foodtoAdd + " food and " + woodtoAdd + " wood",2);
        }
    }
    public HiveGenerator hiveGenerator { get => hive_generator; }
    public UIController uiController { get => ui_controller; }

}
