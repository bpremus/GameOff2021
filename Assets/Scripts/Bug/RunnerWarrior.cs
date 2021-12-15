using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RunnerWarrior : RunnerBug
{
    
    // do not chase outside zone 
    // do not move if 

    public float attack_speed = 0.5f;

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

    protected override void Start()
    {
        SetEvolution();

        if (asigned_cell == null)
        {
            asigned_cell = GetUndelayingCell();
        }

        base.Start();
        current_cell = asigned_cell;
        target = asigned_cell.transform.position + z_offset;
        AssignToAroom(asigned_cell);
    }

    public override void DetectEnemy()
    {
        if (bugTask == BugTask.none) return;

        int layerId = 7; //bugs
        int layerMask = 1 << layerId;

        hitColliders = Physics.OverlapSphere(transform.position, interraction_range, layerMask);
        hitColliders = hitColliders.OrderBy((d) => (d.transform.position -
        transform.position).sqrMagnitude).ToArray();

        List<CoreBug> bugs_to_interract = new List<CoreBug>();

        int cnt = 0;
        foreach (var hitCollider in hitColliders)
        {
            CoreBug cb = hitCollider.GetComponent<CoreBug>();
            if (cb)
            {
                // we do not interact with ourself and we don't do anything if we are idle
                if (cb == this) continue;

                if (bugTask == BugTask.fight)
                {
                    // enemy is dead 
                    if (cb.IsDead() == true) continue;
                    if (cb.coalition == coalition) continue;

                    bug_action = Bug_action.fighting;
                    InteractWithEnemy(cb);
                    bugs_to_interract.Add(cb);
                    cnt++;

                }
            }
        }

        if (cnt > 0)
            InteractWithEnemies(bugs_to_interract);

        // if we are here we stop
        if (bug_action == Bug_action.fighting && cnt == 0)
        {
            bug_action = Bug_action.idle;
            StopInteracitonWithEnemy();
        }
    }

    protected override void OnDamageBoost()
    {
        
    }

    public virtual void SetEvolution()
    {
        bug_evolution = BugEvolution.warrior;
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

    public override void MoveToPosition()
    {   
        base.MoveToPosition();    
    }

    public override void OnInteract(CoreBug otherBug)
    {
        base.OnInteract(otherBug);
    }

    public override void StopInteracitonWithEnemy()
    {
        bugAnimation = BugAnimation.idle;
        ContinueToAndBack(destination_cell,target_cell);
    }

    public override void InteractWithEnemies(List<CoreBug> othrBugs)
    {

        if (othrBugs.Count == 0) return;

        // if distance is less than 1 go for it 
        // if greater, update the path

        StopPath();
        target = othrBugs[0].transform.position;

        bugAnimation = BugAnimation.idle;
        if (_attack_t > attack_speed)
        {
            _attack_t = 0;
        }
        else
            return;

        for (int i = 0; i < othrBugs.Count; i++)
        {
            bugAnimation = BugAnimation.attack;
            if (othrBugs[i].IsDead() == false)
            {
                othrBugs[i].OnInteract(this);
                // did we killed it?
                if (othrBugs[i].IsDead()) bug_kill_count++;
            }
        }
    }

    protected float _attack_t = 0;
    public override void InteractWithEnemy(CoreBug otherBug)
    {

    }

    public override void SetTimers()
    {
        base.SetTimers();
        _attack_t += Time.deltaTime;
    }
}
