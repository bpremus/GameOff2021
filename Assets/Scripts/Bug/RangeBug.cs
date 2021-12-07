using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeBug : WarriorBug
{
    [SerializeField]
    ParticleSystem vfx_shoot;

    protected override void Start()
    {
        base.Start();
    }

    public override void SetEvolution()
    {
        bug_evolution = BugEvolution.range;
    }

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

        if (bugAnimation == BugAnimation.idle)
        {
            animators[0].speed = 0;
            animators[0].SetInteger("State", 0);
            animators[1].SetInteger("State", 0);
        }

        if (bugAnimation == BugAnimation.walk)
        {
            animators[0].speed = move_speed * 4;
            animators[0].SetInteger("State", 0);
            animators[1].SetInteger("State", 0);
        }

        if (bugAnimation == BugAnimation.attack)
        {
            animators[0].speed = 1;
            animators[0].SetInteger("State", 2);
            animators[1].SetInteger("State", 2);
        }
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

        bugAnimation = BugAnimation.idle;
        if (_attack_t > 0.1f)
        {
            _attack_t = 0;
        }
        else
            return;

        for (int i = 0; i < othrBugs.Count; i++)
        {
            bugAnimation = BugAnimation.attack;

            // flame thrower like animation
            bugAnimation = BugAnimation.attack;
            if (othrBugs[i].IsDead() == false)
            {
                othrBugs[i].OnInteract(this);
                // did we killed it?
                if (othrBugs[i].IsDead()) bug_kill_count++;
            }

            vfx_shoot.Play();

            Vector3 dir = othrBugs[i].transform.position - transform.position;
            Debug.DrawRay(transform.position, dir * interraction_range);
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

