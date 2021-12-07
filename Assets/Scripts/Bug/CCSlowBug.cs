using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCSlowBug : WarriorBug
{

    protected override void Start()
    {
        base.Start();
    }

    public override void SetEvolution()
    {
        bug_evolution = BugEvolution.cc_bug;
    }


    public override void InteractWithEnemies(List<CoreBug> othrBugs)
    {
        //target = underlaying_cell.transform.position + z_offset;

        if (asigned_cell.IsInTheRoomRange(othrBugs[0].transform.position))
        {
            if (othrBugs[0].underlaying_cell != underlaying_cell)
                GoTo(othrBugs[0].underlaying_cell);
            else
            {
                StopPath();
                target = othrBugs[0].transform.position;
            }
        }
        else
        {
            GoTo(asigned_cell);
        }


     // bugAnimation = BugAnimation.idle;
     // if (_attack_t > 0.1f)
     // {
     //     _attack_t = 0;
     // }
     // else
     //     return;

        for (int i = 0; i < othrBugs.Count; i++)
        {
            bugAnimation = BugAnimation.attack;
            Debug.Log("Slowing");
            // flame thrower like animation
            othrBugs[i].OnBugSlowdown(GetDefinedSpeed * 0.5f);
            Debug.DrawLine(transform.position, othrBugs[i].transform.position, Color.red);

        }
    }

 
    public override void InteractWithEnemy(CoreBug otherBug)
    {
    }

    protected override void OnCanRangeShoot()
    {

    }

    public override void OnWalkStart()
    {
        // Debug.Log("Bug started walking");

 
    }

    public override void OnIdleStart()
    {

        // Debug.Log("Bug is idle");

  
    }

    public override void OnBugIsDead()
    {


    }
}
