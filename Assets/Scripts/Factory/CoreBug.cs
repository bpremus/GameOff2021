using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CoreBug : BugMovement
{
    [HideInInspector]
    public Queue<HiveCell> path = new Queue<HiveCell>();
    [HideInInspector]
    public HiveCell current_cell = null;
    [HideInInspector]
    public HiveCell destination_cell = null;
    [HideInInspector]
    public HiveCell target_cell = null;
    [SerializeField]
    public HiveCell asigned_cell = null;

    // stats 
    [Header("Bug stats")]
    public float health = 10f;
    public float damage = 1f;
    public float interraction_range = 3f;

    // offset to camera above cells 
    // may not be needed once tiles are replaced with mesh
    protected Vector3 z_offset = new Vector3(0, 0, -0.1f);

    public enum BugTask { none, fight, salvage }
    public BugTask bugTask = BugTask.none;

    public enum Bug_action { idle, traveling, gathering, fighting, salvaging, returning, dead };
    [SerializeField]
    protected Bug_action bug_action = Bug_action.idle;

    public Bug_action GetAction { get => bug_action; }

    public int coalition = 0;

    public virtual void SetAction(Bug_action action)
    {
        bug_action = action;
    }

    public virtual void NextAction()
    {
        if (bug_action == CoreBug.Bug_action.idle)
            SetAction(Bug_action.traveling);
        else if (bug_action == CoreBug.Bug_action.traveling)
            SetAction(Bug_action.gathering);
        else if (bug_action == CoreBug.Bug_action.gathering)
            SetAction(Bug_action.returning);
        else if (bug_action == CoreBug.Bug_action.returning)
            SetAction(Bug_action.idle);
    }

    public virtual void AssignToAroom(HiveCell cell)
    {
        cell.AssignDrone(this);
    }

    public override void SetAnimation()
    {
        base.SetAnimation();
    }

    public virtual void OnInteract(CoreBug otherBug)
    {
     
        // Debug.Log("we got interraced by " + otherBug.name);

        // override me with appropriate behaviour 
        if (GetState() == BugAnimation.dead)
        {
            Debug.Log("someone is interracting with a dead enemy");
            Destroy(this.gameObject);
            return;
        }

        // if other is enemy
        OnTakeDamage(otherBug);
    }

    public virtual void OnTakeDamage(CoreBug otherBug)
    {
        health -= otherBug.damage;
        if (health <= 0) LateDie();
    }

    public virtual void OnDestinationReach()
    {
        Debug.Log("destination reached");
    }

    public virtual void OnLateDie()
    {
        // Debug.Log("died");
        FlipBug();
    }

    public virtual void OnTargetReach()
    {
        Debug.Log("target reached");
    }

    public void CurrentPositon(HiveCell position)
    {
        current_cell = position;
        transform.position = current_cell.transform.position + z_offset;
        this.target = transform.position;
    }

    public void GoToAndBack(HiveCell start, HiveCell destination, float wait_timer = 0)
    {
        // if (destination_cell == start) return;

        path.Clear();
        target_cell = destination;
        destination_cell = start;

        List<HiveCell> move_path = AiController.GetPath(start, destination);
        for (int i = 0; i < move_path.Count; i++)
        {
            path.Enqueue(move_path[i]);
        }

        path.Enqueue(destination);
        move_path.Reverse();

        for (int i = 0; i < move_path.Count; i++)
        {
            path.Enqueue(move_path[i]);
        }

        this.target = current_cell.transform.position + z_offset;
    }

    public void GoToAndIdle(HiveCell destination)
    {
        GoTo(destination);
    }

    public void GoTo(HiveCell destination)
    {
        if (destination_cell == destination) return;

        path.Clear();
        target_cell = destination;
        destination_cell = destination;

        path.Enqueue(current_cell);

        List<HiveCell> move_path = AiController.GetPath(current_cell, destination);
        for (int i = 1; i < move_path.Count; i++)
        {
            path.Enqueue(move_path[i]);
        }

        this.target = current_cell.transform.position + z_offset;
    }

    float _last_dist = 0;
    public override void WalkPath()
    {

        if (path.Count > 0)
        {
            HiveCell c = path.Peek();

            Vector3 next_cell_pos = c.transform.position;

            // move bugs outside
            if (c.cell_Type == CellMesh.Cell_type.outside)
            {
                // offset to bottom
                next_cell_pos += new Vector3(0, -1, 0);
            }

            // inside 
            if (c.cell_Type == CellMesh.Cell_type.corridor)
            {
                // slight offset in direction 

                float ang = Vector3.SignedAngle(transform.forward, Vector3.up, z_offset);
                if (ang > 0)
                {
                    next_cell_pos += new Vector3(-0.1f, -0.1f, 0);
                }
                else
                {
                    next_cell_pos += new Vector3(0.1f, 0.1f, 0);
                }
            }

            float t = Vector3.Distance(next_cell_pos + z_offset, transform.position);
            if (t <= stop_distance || _last_dist == t)
            {
                current_cell = path.Dequeue();
                if (path.Count > 0)
                {
                    if (target_cell == current_cell)
                    {
                        OnTargetReach();
                    }
                }
                else
                {
                    current_cell = target_cell;
                    target = current_cell.transform.position + z_offset;
                    OnDestinationReach();
                }
                return;
            }

            _last_dist = t;
            this.target = next_cell_pos + z_offset;
        }
        else
        {
            if (destination_cell != null)
                this.target = destination_cell.transform.position + z_offset;
        }
    }


    public void LateDie()
    {
        // unregister bug 
        OnLateDie();

        bugAnimation = BugAnimation.dead;

        // place dead bug 
    }

    [SerializeField]
    Collider[] hitColliders;

    public virtual void OnCombatStop()
    {

    }

    public override void OnWalkStart()
    {
       
    }

    public override void OnIdleStart()
    {
     
    }

    public virtual void StopInteracitonWithEnemy()
    {
     //  if (bugAnimation == BugAnimation.attack)
     //  {
     //      bugAnimation = BugAnimation.idle;
     //      OnCombatStop();
     //  }
    }

    public virtual void InteractWithEnemy(CoreBug otherBug)
    {
        /*
        if (_interact_t > 0.1f)
        {
            _interact_t = 0;
        }
        else
            return;
        otherBug.OnInteract(this);
        */
    }

    float _interact_t = 0;
    public override void SetTimers()
    {
        _interact_t += Time.deltaTime;
    }


    public override void DetectEnemy()
    {
        if (bugTask == BugTask.none) return;

        int layerId = 7; //bugs
        int layerMask = 1 << layerId;

        interraction_range = 1;

        hitColliders = Physics.OverlapSphere(transform.position, interraction_range, layerMask);
        hitColliders = hitColliders.OrderBy((d) => (d.transform.position -
        transform.position).sqrMagnitude).ToArray();

        foreach (var hitCollider in hitColliders)
        {
            CoreBug cb = hitCollider.GetComponent<CoreBug>();
            if (cb)
            {
                // we do not interract with ourself and we dont do anything if we are idle
                if (cb == this) continue;
                

                if (bugTask == BugTask.fight)
                {
                    // enemy is dead 
                    if (cb.GetState() == BugAnimation.dead) continue;
                    if (cb.coalition == coalition) continue;
                    
                    bug_action = Bug_action.fighting;
                    InteractWithEnemy(cb);
                    return;
                }

                /*
                // based on task 
                if (bugTask == BugTask.salvage)
                {
                    if (cb.GetState() == BugAnimation.dead)
                    {
                        this.target = cb.transform.position;
                        bug_action = Bug_action.salvaging;
                        InteractWithEnemy(cb);
                        return;
                    }
                    continue;
                }

                if (cb.GetState() == BugAnimation.dead)
                
                if (cb.IsValidState() == false) continue;

                if (cb.tag != this.tag)
                {
                    Debug.DrawLine(transform.position, cb.transform.position, Color.red);
                    this.target = cb.transform.position;
                    bug_action = Bug_action.fighting;
                    InteractWithEnemy(cb);
                    return;
                }
                */
            }
        }

        // if we are here we stop
        if (bug_action == Bug_action.fighting)
        {
            bug_action = Bug_action.idle;
            StopInteracitonWithEnemy();
        }      
    }


    public void SetColor(int coalition)
    { 
    
    
    }


}
