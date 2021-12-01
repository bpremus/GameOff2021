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

        AkSoundEngine.PostEvent("Play_Big_Bug_Movement", gameObject);
    }

    public override void OnIdleStart()
    {

        // Debug.Log("Bug is idle");

        AkSoundEngine.PostEvent("Stop_Big_Bug_Movement", gameObject);
    }

    public override void OnBugIsDead()
    {

        AkSoundEngine.PostEvent("Stop_Big_Bug_Movement", gameObject);

        AkSoundEngine.PostEvent("Play_Small_Bug_Death", gameObject);
    }
}
