using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerBug : CoreBug
{
    protected override void Start()
    {
        bug_evolution = BugEvolution.drone;
        base.Start();
    }

    public override void InteractWithEnemy(CoreBug otherBug)
    {
        // workers ignore enemy
    }

    public override void DetectEnemy()
    {
        // workers ignore enemy
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
