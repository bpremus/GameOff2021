using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerBug : CoreBug
{
    public override void InteractWithEnemy(CoreBug otherBug)
    {
        // workers ignore enemy
    }

    public override void DetectEnemy()
    {
        // workers ignore enemy
    }
}
