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

    public override void OnWalkStart()
    {
        // Debug.Log("Bug started walking");

        AkSoundEngine.PostEvent("Play_Small_Bug_Movement", gameObject);
    }

    public override void OnIdleStart()
    {

        // Debug.Log("Bug is idle");

        AkSoundEngine.PostEvent("Stop_Small_Bug_Movement", gameObject);
    }

    public override void OnBugIsDead()
    {

        AkSoundEngine.PostEvent("Stop_Small_Bug_Movement", gameObject);
    }
}
