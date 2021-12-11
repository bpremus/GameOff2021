using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Formatter_BugName : MonoBehaviour
{
    [SerializeField] private string droneName = "Worker";
    [SerializeField] private string superdroneName = "Super worker";
    [SerializeField] private string warriorName = "Warrior";
    [SerializeField] private string clawName = "Claw";
    [SerializeField] private string rangeName = "Toxic ranger";
    [SerializeField] private string ccBugName = "Spiker";
    #region Instance
    private static Formatter_BugName instance;
    public static Formatter_BugName Instance 
    {
        get{ return instance; }
    }
    private void Awake()
    {

        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
    }
    #endregion


    public string GetBugName(CoreBug.BugEvolution evolution)
    {
        string bugName = "Bug";
        if (evolution == CoreBug.BugEvolution.drone) bugName = droneName;
        else if (evolution == CoreBug.BugEvolution.super_drone) bugName = superdroneName;
        else if (evolution == CoreBug.BugEvolution.warrior) bugName = warriorName;
        else if (evolution == CoreBug.BugEvolution.claw) bugName = clawName;
        else if (evolution == CoreBug.BugEvolution.range) bugName = rangeName;
        else if (evolution == CoreBug.BugEvolution.cc_bug) bugName = ccBugName;
        else
            bugName = evolution.ToString();

        return bugName;
    }
    public string GetBugAction(CoreBug.BugTask bugTask)
    {
        string task;
        if (bugTask == CoreBug.BugTask.none) task = "Idle";
        else if (bugTask == CoreBug.BugTask.fight) task = "Defending";
        else if (bugTask == CoreBug.BugTask.harvesting) task = "Harvesting";
        else if (bugTask == CoreBug.BugTask.salvage) task = "Salvaging";
        else
            task = bugTask.ToString();
        return task;
    }
}
