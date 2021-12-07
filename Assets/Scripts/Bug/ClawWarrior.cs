using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawWarrior : WarriorBug
{
    public override void SetEvolution()
    {
        bug_evolution = BugEvolution.claw;
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
