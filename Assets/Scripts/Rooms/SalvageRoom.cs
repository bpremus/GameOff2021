using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalvageRoom : HiveRoom
{
    public override void Start()
    {
        for (int i = 0; i < assigned_bugs.Count; i++)
        {
            CoreBug b = assigned_bugs[i].GetComponent<CoreBug>();
            b.CurrentPositon(this.parent_cell);
            b.bugTask = CoreBug.BugTask.salvage;

        }
    }


    public override void DetectEnemy()
    {
        int layerId = 7; //bugs
        int layerMask = 1 << layerId;
        hitColliders = Physics.OverlapSphere(transform.position, room_detect_distance, layerMask);
        foreach (var hitCollider in hitColliders)
        {
            CoreBug cb = hitCollider.GetComponent<CoreBug>();
            if (cb)
            {
                if (cb.GetState() == BugMovement.BugAnimation.dead)
                {
                    SendBugToIntercept(cb.current_cell);
                    return;
                }
            }
        }
        ReturnHome();
    }
}
