using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugMovement : MonoBehaviour
{
    [Header("Bug movement")]

    [HideInInspector]
    Vector3 position = Vector3.zero;

    [HideInInspector]
    public Vector3 target;

    [SerializeField]
    protected float rotation_speed = 10f;
    [SerializeField]
    protected float move_speed = 2f;

    protected float slow_penalty_speed = 0;

    protected bool _isDead = false;

    [SerializeField]
    protected Animator[] animators;

    [SerializeField]
    protected bool Debug_movement_mode = false;

    public enum BugAnimation { idle, walk, fly, attack, dead, dragged };

    // move stop distance
    public float stop_distance = 0.05f;


    [SerializeField]
    protected BugAnimation bugAnimation = BugAnimation.idle;

    public BugAnimation GetState()
    {
        return bugAnimation;
    }
    public void SetState(BugAnimation animState)
    {
        bugAnimation = animState;
    }

    public float GetDefinedSpeed { get => move_speed; }

    public virtual bool IsValidState()
    {
        if (bugAnimation == BugAnimation.idle) return true;
        if (bugAnimation == BugAnimation.walk) return true;

        return false;
    }

    protected virtual void Start()
    {
        position = transform.position;
        last_pos = transform.position;

        animators = transform.GetComponentsInChildren<Animator>();

        this.gameObject.AddComponent<CoreColorShader>();
    }

    protected float speed = 0;
    private Vector3 last_pos = Vector3.zero;
    public virtual void SetAnimation()
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
            animators[0].speed = move_speed * 2.5f;
            animators[0].SetInteger("State", 0);
            animators[1].SetInteger("State", 0);
            animators[2].SetInteger("State", 0);
        }
        
        last_pos = transform.position;
    }

    public void Update()
    {
        if (!_isDead) // Alive
        {
            MoveToPosition();
            WalkPath();
            DetectEnemy();
            SetTimers();
            OnCanRangeShoot();
        }
        if (_isDead) // dead
        {
            FlipBug();
        }

        FaceBugUp();
        SetAnimation();

        // dbg
        // GetUndelayingCell();

    }

    protected virtual void OnCanRangeShoot()
    { 
    
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
        if (Debug_movement_mode) return;

        Vector3 direction = destination - transform.position;
        Vector3 normal_direction = new Vector3(0, 0, 1);
        Quaternion look_direction = transform.rotation;
        if (direction == Vector3.zero)
        {
            return;
        }

        if (orientation == 1)
            normal_direction = new Vector3(0, 1, 0);

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
         //      // we are on top of other bug
         //      if (transform.position == bm.transform.position)
         //      {
         //         direction = transform.forward * stop_distance / 2 ; // just move slightly
         //      }
         //  }

        BugAnimation _bugAnimation = bugAnimation;
        float d = Vector3.Distance(transform.position, destination);
        if (d > stop_distance)
        {
            direction = direction.normalized * move_speed;
            Vector3 new_position = transform.position + direction;

            float final_speed = move_speed - slow_penalty_speed;
            slow_penalty_speed = 0;

            transform.position = Vector3.Lerp(transform.position, new_position, Time.deltaTime * final_speed);

            OnWalk();

            if (_bugAnimation != bugAnimation)
                OnWalkStart();
        }
        else
        {
            OnIdle();

            if (_bugAnimation != bugAnimation)
                OnIdleStart();
        }
    }

    public void OnBugSlowdown(float speed = 0.5f)
    {
        slow_penalty_speed = speed;
    }


    public virtual void OnWalkStart()
    {
        // Debug.Log("Bug started walking");
    }

    public virtual void OnIdleStart()
    {

        // Debug.Log("Bug is idle");
    }

    public virtual void OnWalk()
    {
        if (Debug_movement_mode)
        {
            return;
        }


        if (bugAnimation != BugAnimation.fly)
            bugAnimation = BugAnimation.walk;
    }

    public virtual void OnIdle()
    {
        if (Debug_movement_mode)
        {
            return;
        }

        if (bugAnimation != BugAnimation.fly)
            bugAnimation = BugAnimation.idle;

    }

    protected void FlipBug()
    {
        Vector3 normal_direction = new Vector3(0, 0, -1);
        Vector3 direction = transform.forward;
        Quaternion look_direction = Quaternion.LookRotation(direction, normal_direction); // replace me with a normal
        transform.rotation = Quaternion.Slerp(transform.rotation, look_direction, Time.deltaTime * rotation_speed);
        Vector3 pos = transform.position;
        pos.z = 0.1f;
        transform.position = pos;
        //bugAnimation = BugAnimation.dead;
    }

    /*
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
    */

    int orientation = 0;
    public void FaceBugUp()
    {
        Vector3 pos = transform.position;
        if (pos.y < 2f)
        {
            orientation = 0;
        }
        else
        {
            orientation = 1;
            
        }
    }

    public HiveCell GetUndelayingCell()
    {
        RaycastHit hit;
        int layerId   = 6; //cells
        int layerMask = 1 << layerId;
        Vector3 pos = transform.position + new Vector3(0, 0, 5f);
        //Debug.DrawRay(pos, new Vector3(0, 0, -1) * 10f);
        if (Physics.Raycast(pos, new Vector3(0, 0, -1), out hit, 10f, layerMask))
        {
            HiveCell hc = hit.transform.GetComponent<HiveCell>();
            if (hc)
            {
                return hc;
            }
                
        }
        return null;
    }
}
