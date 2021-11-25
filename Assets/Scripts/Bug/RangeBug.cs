using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeBug : WarriorBug
{

    protected override void Start()
    {
        bug_evolution = BugEvolution.range;
        base.Start();
    }

    public override void InteractWithEnemy(CoreBug otherBug)
    {
        // target = otherBug.transform.position;

        bugAnimation = BugAnimation.idle;
        if (_attack_t > 0.1f)
        {
            _attack_t = 0;
        }
        else
            return;
       
        bugAnimation = BugAnimation.attack;
        otherBug.OnInteract(this);

        Vector3 dir = otherBug.transform.position - transform.position;
        Debug.DrawRay(transform.position, dir * interraction_range);

        // shoot bug
        // slow down bug 

        // flame thrower like animation
        //  otherBug.OnBugSlowdown(GetDefinedSpeed * 0.5f);
        //  shoot_vfx.Play();
    }

    protected override void OnCanRangeShoot()
    {

    }


}

