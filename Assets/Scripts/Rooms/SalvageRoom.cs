using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalvageRoom : HiveRoom
{
    // how this room works
    // you assign units to it
    // if dead bugs are within the range of room, bugs will start collecting these 
    [SerializeField]
    Queue<CoreBug> dead_bugs = new Queue<CoreBug>();

    public override void Start()
    {
        //  for (int i = 0; i < assigned_bugs.Count; i++)
        //  {
        //      CoreBug b = assigned_bugs[i].GetComponent<CoreBug>();
        //      b.CurrentPositon(this.parent_cell);
        //      b.bugTask = CoreBug.BugTask.salvage;
        //  }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, room_detect_distance);
        Gizmos.color = Color.red;
    }

    public override void Update()
    {
        base.Update();
        SendGathering();
    }


    public void OnBugDepart(CoreBug bug)
    {

    }

    public void OnBugReachGatheringSite(CoreBug bug)
    {
        Debug.Log("bugs are gathering");
    }

    public void OnBugReachHomeCell(CoreBug bug)
    {
        Debug.Log("bugs returned home");

        GameController.Instance.OnBrigResources();
    }


    float _spread_timer = 0;
    public void SendGathering()
    {
        _spread_timer += Time.deltaTime;
        for (int i = 0; i < assigned_bugs.Count; i++)
        {
            CoreBug cb = assigned_bugs[i].GetComponent<CoreBug>();
            if (cb)
            {
                // set task
                cb.bugTask = CoreBug.BugTask.salvage;

                if (dead_bugs.Count > 0)
                {
                    // moving to location 
                    if (cb.GetAction == CoreBug.Bug_action.idle)
                    {
                        if (cb.salvage_object != null) continue;
                        if (_spread_timer < 0.5f) continue;
                        _spread_timer = 0;


                        CoreBug dead = dead_bugs.Dequeue();
                        if (dead != null)
                        {
                            Debug.Log(cb.name + " is going to salvage " + dead.name);
                            dead.SetState(BugMovement.BugAnimation.dragged);
                            cb.salvage_object = dead;
                            dead.current_cell = dead.GetUndelayingCell();

                            cb.GoTo(dead.current_cell);
                            cb.SetAction(CoreBug.Bug_action.traveling);
                            OnBugDepart(cb);
                        }
                    }
                }
                else
                {
                    if (cb.GetAction == CoreBug.Bug_action.idle)
                    {
                        if (cb.salvage_object != null) continue;

                        // spread bug
                        SpreadBugs();
                    }
                }

                // reached location
                if (cb.GetAction == CoreBug.Bug_action.traveling)
                {

                    Debug.Log("traveling");
                    if (cb.current_cell == cb.salvage_object.current_cell)
                    {
                        Debug.Log("ready to salvage");
                        cb.SetAction(CoreBug.Bug_action.salvaging);
                        OnBugReachGatheringSite(cb);

                        // start gathering 
                        continue;
                    }
                    if (cb.salvage_object == null)
                    {
                        cb.SetAction(CoreBug.Bug_action.returning);
                        cb.GoTo(this.cell);
                    }

                }


                if (cb.GetAction == CoreBug.Bug_action.salvaging)
                {
                    if (cb.salvage_object == null)
                    {
                        cb.SetAction(CoreBug.Bug_action.returning);
                        cb.GoTo(this.cell);
                        continue;
                    }

                  // if (cb.current_cell != cb.salvage_object.current_cell)
                  // { 
                        //  float d = Vector3.Distance(cb.transform.position, cb.salvage_object.transform.position);
                        //  if (d < 0.2f)
                        //  {
                        cb.SetAction(CoreBug.Bug_action.returning);
                        cb.GoTo(this.cell);
                        // }
                        // else
                        // {
                        //     cb.target = cb.salvage_object.transform.position;
                        // }
                   // }

                }

                if (cb.GetAction == CoreBug.Bug_action.returning)
                {

                    if (cb.salvage_object != null)
                        cb.salvage_object.transform.position = cb.transform.position + cb.transform.up * 0.5f;

                    if (cb.underlaying_cell == this.cell)
                    {
                        OnBugReachHomeCell(cb);
                        cb.SetAction(CoreBug.Bug_action.idle);
                        if (cb.salvage_object != null)
                        {
                            cb.salvage_object.OnLateDecay();
                            cb.salvage_object = null;
                        }
                        continue;
                    }
                }
            }
        }


        
        // {
        //     SpreadBugs();
        // }

    }



    public override void DetectEnemy()
    {
        int layerId = 7; //bugs
        int layerMask = 1 << layerId;
        hitColliders = Physics.OverlapSphere(transform.position, room_detect_distance, layerMask);
        foreach (var hitCollider in hitColliders)
        {
            CoreBug cb = hitCollider.GetComponent<CoreBug>();
            if (cb)
            {
                if (cb.GetState() == BugMovement.BugAnimation.dead)
                {
                    if (dead_bugs.Contains(cb) == false)
                    {
                        dead_bugs.Enqueue(cb);
                    //    Debug.Log("Dead bug in range " + cb.name);
                    }
                    
                    return;
                }
            }
        }
    }
}
