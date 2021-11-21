using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalvageRoom : HiveRoom
{
    // how this room works
    // you assign units to it
    // if dead bugs are within the range of room, bugs will start collecting these 
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
        if (dead_bugs.Count > 0)
        {
            //Debug.DrawLine(transform.position, cell.transform.position);
            _spread_timer += Time.deltaTime;
            for (int i = 0; i < assigned_bugs.Count; i++)
            {
                CoreBug cb = assigned_bugs[i].GetComponent<CoreBug>();
                if (cb)
                {
                    // set task
                    cb.bugTask = CoreBug.BugTask.salvage;

                    // moving to location 
                    if (cb.GetAction == CoreBug.Bug_action.idle)
                    {
                        if (cb.SalvageTask != null) continue;

                        if (_spread_timer < 0.5f) continue;
                        _spread_timer = 0;

                        CoreBug dead = dead_bugs.Dequeue();
                        dead.SetState(BugMovement.BugAnimation.dragged);
                        cb.SalvageTask = dead;
                        cb.GoTo(dead.current_cell);
                        cb.SetAction(CoreBug.Bug_action.traveling);
                        OnBugDepart(cb);
                    }

                    // reached location
                    if (cb.GetAction == CoreBug.Bug_action.traveling)
                    {
                        if (cb.current_cell == cb.SalvageTask.current_cell)
                        {
                            cb.SetAction(CoreBug.Bug_action.salvaging);
                            OnBugReachGatheringSite(cb);

                            // start gathering 
                            continue;
                        }
                        if (cb.SalvageTask == null)
                        {
                            cb.SetAction(CoreBug.Bug_action.returning);
                            cb.GoTo(this.cell);
                        }
                    }


                    if (cb.GetAction == CoreBug.Bug_action.salvaging)
                    {
                        if (cb.SalvageTask == null)
                        {
                            cb.SetAction(CoreBug.Bug_action.returning);
                            cb.GoTo(this.cell);
                            continue;
                        }
                       
                        float d = Vector3.Distance(cb.transform.position, cb.SalvageTask.transform.position);
                        if (d < 0.2f)
                        {
                            cb.SetAction(CoreBug.Bug_action.returning);
                            cb.GoTo(this.cell);
                        }
                        else
                        {
                            cb.target = cb.SalvageTask.transform.position;
                        }

                    }

                    if (cb.GetAction == CoreBug.Bug_action.returning)
                    {

                        if (cb.SalvageTask != null)
                            cb.SalvageTask.transform.position = cb.transform.position;


                        if (cb.current_cell == this.cell)
                        {
                            OnBugReachHomeCell(cb);
                            cb.SetAction(CoreBug.Bug_action.idle);
                            if (cb.SalvageTask != null)
                            {
                                Destroy(cb.SalvageTask.gameObject);
                                cb.SalvageTask = null;
                            }
                            continue;
                        }
                    }
                }
            }
        }
        else
        {
            SpreadBugs();
        }
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
                        dead_bugs.Enqueue(cb);
                    return;
                }
            }
        }
    }
}
