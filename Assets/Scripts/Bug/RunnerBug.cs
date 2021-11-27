using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerBug : CoreBug
{
    public override void DetectEnemy()
    {
        // enemy runner bug, do not stop to fight
    }

    [SerializeField]
    private float walk_animation_adjust = 1;

    public override void SetAnimation()
    {
        // if its dead just stop all
        if (_isDead)
        {
            for (int i = 0; i < animators.Length; i++)
            {
                animators[0].speed = 0;
            }
            return;
        }

        // idle 

        if (bugAnimation == BugAnimation.idle)
        {
            animators[0].speed = 0;
            animators[0].SetInteger("State", 0);
            animators[1].SetInteger("State", 0);
            animators[2].SetInteger("State", 0);
        }

        // walking

        if (bugAnimation == BugAnimation.walk)
        {
            animators[0].speed = move_speed * 2.5f * walk_animation_adjust;
            animators[0].SetInteger("State", 0);
            animators[1].SetInteger("State", 0);
            animators[2].SetInteger("State", 0);
        }

        last_pos = transform.position;
    }

}
