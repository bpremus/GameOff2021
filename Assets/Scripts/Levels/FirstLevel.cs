using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstLevel : CoreLevel
{
    public override void SetGrid()
    {
        Debug.Log("first level started");

        GameLog.Instance.WriteLine("New objective");
        GameLog.Instance.WriteLine("Build Harvesting building");


        DrawMask(4);
    }

}
