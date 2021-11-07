using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugMovement : MonoBehaviour
{
    [SerializeField]
    Vector3 position = Vector3.zero;

    [SerializeField]
    public Transform target;

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
        if (target != null)
        MoveBugToPosition(target.position);
        SetAnimation();
        FaceBugUp();
    }

    
    public void MoveBugToPosition(Vector3 destination)
    { 
        Vector3 direction = transform.position - destination;
        // direction.y = 0;
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
            if (bm == null)
            {           
                continue;
            }
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


    private Vector3 GetMeshColliderNormal(RaycastHit hit)
    {
        MeshCollider collider = (MeshCollider)hit.collider;
        Mesh mesh = collider.sharedMesh;
        Vector3[] normals = mesh.normals;
        int[] triangles = mesh.triangles;

        Vector3 n0 = normals[triangles[hit.triangleIndex * 3 + 0]];
        Vector3 n1 = normals[triangles[hit.triangleIndex * 3 + 1]];
        Vector3 n2 = normals[triangles[hit.triangleIndex * 3 + 2]];
        Vector3 baryCenter = hit.barycentricCoordinate;
        Vector3 interpolatedNormal = n0 * baryCenter.x + n1 * baryCenter.y + n2 * baryCenter.z;
        interpolatedNormal.Normalize();
        interpolatedNormal = hit.transform.TransformDirection(interpolatedNormal);
        return interpolatedNormal;
    }

    public void FaceBugUp()
    {
        // unless its dead 
        // RaycastHit rayHit;
        // if (Physics.Raycast(transform.position, Vector3.down, out rayHit))
        // {
        //     Debug.Log("hit:" + rayHit.collider.name);
        //     Vector3 normal = GetMeshColliderNormal(rayHit);
        //     Debug.DrawRay(transform.position, normal * 10f, Color.blue);
        //      transform.position = rayHit.point + normal.normalized * 0.11f;
        //     transform.rotation = Quaternion.LookRotation(normal) * Quaternion.FromToRotation(Vector3.up, Vector3.forward);
        //  }

        Vector3 normal = Vector3.left; //the normal of the surface, using 'up' for demo purposes

        Quaternion deltaRot = Quaternion.FromToRotation(this.transform.up, normal);
        Quaternion targRot = deltaRot * this.transform.rotation;
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targRot, Time.deltaTime * speed);



    }
}
