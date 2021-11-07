using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorBug : CoreBug
{
    public override void AssignToAroom(HiveCell cell)
    {
        // siege mode 
        this.target = cell.transform.position + new Vector3(0, 0, 1);
    }

    public override void MoveToPosition()
    {
        
    }

    public override void WalkPath()
    { 
        
    }

}
