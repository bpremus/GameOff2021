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

    private void Start()
    {
        position = transform.position;
    }

    public void Update()
    {
        MoveBugToPosition(target.position);
    }

    
    public void MoveBugToPosition(Vector3 destination)
    { 
        Vector3 direction = transform.position - destination;
        direction.y = 0;
        Quaternion look_direction = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(transform.rotation, look_direction, Time.deltaTime * rotation_speed);
        transform.position = Vector3.Lerp(transform.position, transform.position - direction, Time.deltaTime * move_speed);

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1);
        foreach (var hitCollider in hitColliders)
        {
            BugMovement bm = hitCollider.GetComponent<BugMovement>();
            if (bm == null) continue;
            if (bm == this) continue;

            direction = bm.transform.position - transform.position;
            direction = direction.normalized * 2f;

        }

        transform.position = Vector3.Lerp(transform.position, transform.position - direction, Time.deltaTime * move_speed);

    }

}
