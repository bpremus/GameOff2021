using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WarriorBug : CoreBug
{

    public float attack_speed = 0.1f;

    public override void AssignToAroom(HiveCell cell)
    {

    }

    public override void MoveToPosition()
    {
        MoveSiegedBug();
    }


    public override void OnInteract(CoreBug otherBug)
    {
        base.OnInteract(otherBug);
    }

    float _attack_t = 0;
    public override void InteractWithEnemy(CoreBug otherBug)
    {
       
        if (_attack_t > 0.1f)
        {
            _attack_t = 0;
        }
        else
            return;

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
        Vector3 normal_direction = new Vector3(0, 0, 1);
        Quaternion look_direction = transform.rotation;
        if (direction == Vector3.zero)
        {
            return;
        }

        look_direction = Quaternion.LookRotation(direction, normal_direction); // replace me with a normal
        transform.rotation = Quaternion.Slerp(transform.rotation, look_direction, Time.deltaTime * rotation_speed);

    }


    public override void WalkPath()
    { 
        
    }

    


}
