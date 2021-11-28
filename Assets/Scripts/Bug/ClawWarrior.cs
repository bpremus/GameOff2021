using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawWarrior : WarriorBug
{
    public override void SetEvolution()
    {
        bug_evolution = BugEvolution.claw;
    }
}
