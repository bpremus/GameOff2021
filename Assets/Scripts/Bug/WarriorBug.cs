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

    public override void SetAnimation()
    {
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

    protected override void Start()
    {
        base.Start();
        current_cell = asigned_cell;
        target = asigned_cell.transform.position + z_offset;
        AssignToAroom(asigned_cell);
    }

    public float attack_speed = 0.1f;

    public override void MoveToPosition()
    {
        if (siege_mode)
        {
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

    float _attack_t = 0;
    public override void InteractWithEnemy(CoreBug otherBug)
    {
        target = otherBug.transform.position;

        bugAnimation = BugAnimation.idle;
        if (_attack_t > 0.1f)
        {
            _attack_t = 0;
        }
        else
            return;
       
        bugAnimation = BugAnimation.attack;
        otherBug.OnInteract(this);
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

            look_direction = Quaternion.LookRotation(direction, normal_direction); // replace me with a normal
            transform.rotation = Quaternion.Slerp(transform.rotation, look_direction, Time.deltaTime * rotation_speed);
        
    }

}
