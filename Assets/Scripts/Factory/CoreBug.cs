using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CoreBug : BugMovement
{

    public Queue<HiveCell> path = new Queue<HiveCell>();
    public HiveCell current_cell = null;
    public HiveCell destination_cell = null;
    public HiveCell target_cell = null;
    public HiveCell asigned_cell = null;

    // stats 
    public float health = 10f;
    public float damage = 1f;
    public float interraction_range = 3f;

    // offset to camera above cells 
    // may not be needed once tiles are replaced with mesh
    protected Vector3 z_offset = new Vector3(0, 0, 1);

    public enum BugTask { none, fight, salvage }
    public BugTask bugTask = BugTask.none;

    public virtual void AssignToAroom(HiveCell cell)
    {

    }

    public virtual void OnInteract(CoreBug otherBug)
    {
        Debug.Log("we got interraced by " + otherBug.name);

        // override me with appropriate behaviour 
        if (GetState() == BugAnimation.dead)
        {
            Debug.Log("someone is interracting with a dead enemy");
            Destroy(this.gameObject);
            return;
        }

        // if other is enemy
        OnTakeDamage(otherBug);
        this.target = transform.position;
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
        Debug.Log("died");
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

    public void GoTo(HiveCell destination)
    {
        path.Clear();
        target_cell = destination;
        destination_cell = destination;

        List<HiveCell> move_path = AiController.GetPath(current_cell, destination);
        for (int i = 1; i < move_path.Count; i++)
        {
            path.Enqueue(move_path[i]);
        }

        this.target = current_cell.transform.position + z_offset;
    }

    public override void WalkPath()
    {
        if (path.Count > 0)
        {
            HiveCell c = path.Peek();
            float t = Vector3.Distance(c.transform.position + z_offset, transform.position);
            if (t <= 0.5f)
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
                    OnDestinationReach();
                }
                return;
            }
            this.target = c.transform.position + z_offset;
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
        // place dead bug 
        
    }

    [SerializeField]
    Collider[] hitColliders;

    public virtual void InteractWithEnemy(CoreBug otherBug)
    {     
        if (_interact_t > 0.1f)
        {
            _interact_t = 0;
        }
        else
            return;
        otherBug.OnInteract(this);
    }

    float _interact_t = 0;
    public override void SetTimers()
    {
        _interact_t += Time.deltaTime;
    }


    public override void DetectEnemy()
    {

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
                if (cb == this) continue;

                // based on task 
                if (bugTask == BugTask.salvage)
                {
                    if (cb.GetState() == BugAnimation.dead)
                    {
                        this.target = cb.transform.position;
                        InteractWithEnemy(cb);
                        return;
                    }
                    continue;
                }
                
                if (cb.IsValidState() == false) continue;

                if (cb.tag != this.tag)
                {
                    Debug.DrawLine(transform.position, cb.transform.position, Color.red);
                    this.target = cb.transform.position;
                    InteractWithEnemy(cb);
                    return;
                }
            }
        }
    }


}
