using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorLevel : CoreLevel
{

    public override bool IsTaskCompleted()
    {

       

        return false;
    }

    public override void OnLevelComplete()
    {
        GameLog.Instance.WriteLine("Task completed sucessfully");
    }

}
