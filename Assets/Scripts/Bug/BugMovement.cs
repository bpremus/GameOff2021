using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugMovement : MonoBehaviour
{
   
    [SerializeField]
    Vector3 position = Vector3.zero;

    [SerializeField]
    public Vector3 target;

    [SerializeField]
    protected float rotation_speed = 10f;
    [SerializeField]
    protected float move_speed     = 2f;

    [SerializeField]
    protected Animator BodyAnimator;

    [SerializeField]
    protected Animator LegsAnimator;

    [SerializeField]
    Transform wings;

    

    public enum BugAnimation { idle, walk, fly, dead };

    // move stop distance
    public float stop_distance = 1;


    [SerializeField]
    protected BugAnimation bugAnimation = BugAnimation.idle;

    public BugAnimation GetState()
    {
        return bugAnimation;
    }
    public virtual bool IsValidState()
    {
        if (bugAnimation == BugAnimation.idle) return true;
        if (bugAnimation == BugAnimation.walk) return true;

        return false;
    }

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

        if (bugAnimation == BugAnimation.dead)
        {
            BodyAnimator.SetInteger("State", 0);
            LegsAnimator.SetInteger("State", 0);
            LegsAnimator.speed = 1f;
            speed = 0;
            wings.gameObject.SetActive(false);
        }
        
        last_pos = transform.position;
    }

    public void Update()
    {
        if (bugAnimation != BugAnimation.dead)
        {
            MoveToPosition();
            WalkPath();
            DetectEnemy();
            SetTimers();
        }
        if (bugAnimation == BugAnimation.dead)
        {
            FlipBug();
        }

            SetAnimation();

    }


    public virtual void SetTimers()
    { 
    
    }

    public virtual void DetectEnemy()
    { 
    
    }

    public virtual void MoveToPosition()
    {
        MoveBugToPosition(target);
    }

    public virtual void WalkPath()
    {
        Debug.LogError(this.name + " bug doesnt know how to walk, but you want it to walk anyways ");
    }

    protected void MoveBugToPosition(Vector3 destination)
    { 
        Vector3 direction = destination - transform.position;
        Vector3 normal_direction = new Vector3(0, 0, 1);
        Quaternion look_direction = transform.rotation;
        if (direction == Vector3.zero)
        {
            return;
        }
        
        look_direction = Quaternion.LookRotation(direction, normal_direction); // replace me with a normal
        transform.rotation = Quaternion.Slerp(transform.rotation, look_direction, Time.deltaTime * rotation_speed);

 
        // Debug.DrawRay(transform.position, transform.forward * 10, Color.green);
        // transform.position = Vector3.Lerp(transform.position, transform.position - direction, Time.deltaTime * move_speed);

      //  Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1);
      //  foreach (var hitCollider in hitColliders)
      //  {
      //      BugMovement bm = hitCollider.GetComponent<BugMovement>();
      //      if (bm == null)
      //      {           
      //          continue;
      //      }
      //      if (bm == this) continue;
      //
      //      direction = bm.transform.position - transform.position;
      //      direction = direction.normalized * stop_distance / 2;
      //  }
   
        float d = Vector3.Distance(transform.position, destination);
        if (d > stop_distance)
        {
            direction = direction.normalized * move_speed;
            transform.position = Vector3.Lerp(transform.position, transform.position + direction, Time.deltaTime * move_speed);

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

    protected void FlipBug()
    {
        Vector3 normal_direction = new Vector3(0, 0, -1);
        Vector3 direction = transform.forward;
        Quaternion look_direction = Quaternion.LookRotation(direction, normal_direction); // replace me with a normal
        transform.rotation = Quaternion.Slerp(transform.rotation, look_direction, Time.deltaTime * rotation_speed);

        bugAnimation = BugAnimation.dead;
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
       // if (Physics.Raycast(transform.position, new Vector3(0,0,-1), out rayHit))
       // {
       //
       //     Debug.DrawRay(transform.position, new Vector3(0, 0, -5));
       //     Debug.Log(rayHit.transform.name);
       //     Vector3 normal = GetMeshColliderNormal(rayHit);
       //  //  Debug.DrawRay(transform.position, normal * 10f, Color.blue);
       //    //  transform.position = rayHit.point + normal.normalized * 0.11f;
       //    //  transform.rotation = Quaternion.LookRotation(normal) * Quaternion.FromToRotation(Vector3.up, Vector3.forward);
       //  }

        //Vector3 direction = transform.up;
        //Quaternion look_direction = Quaternion.LookRotation(direction, Vector3.forward); // replace me with a normal


    }
}
