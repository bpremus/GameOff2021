using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCSlowBug : WarriorBug
{

    protected override void Start()
    {
        bug_evolution = BugEvolution.cc_bug;
        base.Start();
    }

    public override void InteractWithEnemy(CoreBug otherBug)
    {
        //target = otherBug.transform.position;
        bugAnimation = BugAnimation.attack;
     
        // flame thrower like animation
        otherBug.OnBugSlowdown(GetDefinedSpeed * 0.5f);
        Debug.DrawLine(transform.position, otherBug.transform.position, Color.red);

    }

    protected override void OnCanRangeShoot()
    {

    }
}
