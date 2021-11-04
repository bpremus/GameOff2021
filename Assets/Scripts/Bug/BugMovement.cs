using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugMovement : MonoBehaviour
{
    [SerializeField]
    Vector3 position = Vector3.zero;

    [SerializeField]
    Transform target;

    [SerializeField]
    float rotation_speed = 10f;
    [SerializeField]
    float move_speed = 2f;

    [SerializeField]
    protected Animator BodyAnimator;

    [SerializeField]
    protected Animator LegsAnimator;

    [SerializeField]
    Transform wings;


    public enum BugAnimation { idle, walk, fly };

    public float stop_distance = 1;


    [SerializeField]
    BugAnimation bugAnimation = BugAnimation.idle;

    private void Start()
    {
        position = transform.position;
        last_pos = transform.position;
    }

    [SerializeField]
    protected float speed = 0;
    private Vector3 last_pos = Vector3.zero;
    public void SetAnimation()
    {
        if (bugAnimation == BugAnimation.idle)
        {
            BodyAnimator.SetInteger("State", 0);
            LegsAnimator.SetInteger("State", 0);
            LegsAnimator.speed = 1f;
            speed = 0;
            wings.gameObject.SetActive(false);
        }

        if (bugAnimation == BugAnimation.walk)
        {
            BodyAnimator.SetInteger("State", 2);
            LegsAnimator.SetInteger("State", 2);
            LegsAnimator.speed = Mathf.Clamp(speed,0,1.5f);
            speed = (last_pos - transform.position).magnitude / Time.deltaTime;
            wings.gameObject.SetActive(false);
            //  if (speed < 5) speed = 0;
        }

        if (bugAnimation == BugAnimation.fly)
        {
            BodyAnimator.SetInteger("State", 4);
            LegsAnimator.SetInteger("State", 0);
            wings.gameObject.SetActive(true);
        }
        
        last_pos = transform.position;
    }

    public void Update()
    {
        MoveBugToPosition(target.position);
        SetAnimation();
    }

    
    public void MoveBugToPosition(Vector3 destination)
    { 
        Vector3 direction = transform.position - destination;
        direction.y = 0;
        if (direction == Vector3.zero)
        {
            OnIdle();
            return;
        }
        Quaternion look_direction = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(transform.rotation, look_direction, Time.deltaTime * rotation_speed);
        // transform.position = Vector3.Lerp(transform.position, transform.position - direction, Time.deltaTime * move_speed);

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1);
        foreach (var hitCollider in hitColliders)
        {
            BugMovement bm = hitCollider.GetComponent<BugMovement>();
            if (bm == null) continue;
            if (bm == this) continue;

            direction = bm.transform.position - transform.position;
            direction = direction.normalized * stop_distance / 2;
        }
   
        float d = Vector3.Distance(transform.position, destination);
        if (d > stop_distance)
        {
            transform.position = Vector3.Lerp(transform.position, transform.position - direction, Time.deltaTime * move_speed);
            OnWalk();
        }
        else
        {
            OnIdle();
        }
    }

    public void OnWalk()
    {
       if (bugAnimation != BugAnimation.fly)
       bugAnimation = BugAnimation.walk;
    }

    public void OnIdle()
    {
        if (bugAnimation != BugAnimation.fly)
            bugAnimation = BugAnimation.idle;
    }

}
