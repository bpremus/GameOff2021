using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    [SerializeField]
    HiveGenerator hive_generator;

    [SerializeField] 
    UIController ui_controller;

    [SerializeField]
    CoreLevel[] game_levels;

    CoreLevel curent_level = null;
    private int level_index = 0;

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
    }

    public HiveGenerator hiveGenerator { get => hive_generator; }
    public UIController uiController { get => ui_controller; }

}
