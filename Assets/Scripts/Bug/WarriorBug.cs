using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WarriorBug : CoreBug
{
    // do not chase outside zone 
    // do not move if 

    [SerializeField]
    bool siege_mode = true;

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

    protected override void OnDamageBoost()
    {
        if (siege_mode)
        {
            extra_damage = damage * 1.5f;
            return;
        }
        if (asigned_cell.GetRoom().GetComponent<CommandCenter>())
        {
            extra_damage = damage * 1.1f;
            return;
        }
        if (asigned_cell.GetRoom().GetComponent<WarRoom>())
        {
            extra_damage = damage * 1.1f;
            return;
        }
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
        if (siege_mode)
        {
            // sieged bug will just turn 
            MoveSiegedBug();
        }
        else
        {
            base.MoveToPosition();
        }
    }

    public override void OnInteract(CoreBug otherBug)
    {
        base.OnInteract(otherBug);
    }

    public override void StopInteracitonWithEnemy()
    {
        bugAnimation = BugAnimation.idle;
    }

    public override void InteractWithEnemies(List<CoreBug> othrBugs)
    {
       // Debug.Log("attacking");

        //target = underlaying_cell.transform.position + z_offset;
        if (othrBugs.Count == 0) return;
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

    public void MoveSiegedBug()
    {

        Vector3 direction = target - transform.position;
        direction.z = 0;
        Vector3 normal_direction = new Vector3(0, 0, 1);
        Quaternion look_direction = transform.rotation;
        if (direction == Vector3.zero)
        {
            return;
        }

        float rot_speed = rotation_speed;
        if (bug_action == Bug_action.fighting)
        {
            rot_speed = 1.5f;
        }

        look_direction = Quaternion.LookRotation(direction, normal_direction); // replace me with a normal
        transform.rotation = Quaternion.Slerp(transform.rotation, look_direction, Time.deltaTime * rot_speed);

    }

    public override void SiegeBug(bool siege) 
    {
        siege_mode = siege;
        Debug.Log("Siege mode" + siege_mode);
    }


    public override bool GetSiegeState() { return siege_mode; }

}
