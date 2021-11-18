using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarversterRoom : HiveRoom
{
    [SerializeField]
    public HiveCell gather_destination;
    public int gather_duration_time = 1;

    // we can have it for each separate bug
    // or we handle all bugs ate same time
    // we will go with version 2 for now 
    // later in polish phase we can ajust
    float _gather_t = 0;

    public override void Update()
    {
        base.Update();
        SendGathering();
    }

    public void GetBugsFromHive()
    {
        QueenRoom qr = FindObjectOfType<QueenRoom>();
        if (qr)
        {
            GameObject bug = qr.GetBugFroTransfer(0);
            assigned_bugs.Add(bug);
            Debug.Log("moving a bug");
        }
    }
    public void SendToCollect()
    {
        gather_destination = cell.hiveGenerator.cells[9][9];
    }
    public void OnBugReachGatheringSite(CoreBug bug)
    {
        Debug.Log("bugs are gathering");
    }

    public void OnBugReachHomeCell(CoreBug bug)
    {
        Debug.Log("bugs returned home");

        DBG_TopUI.Instance.BringFood();
    }

    public void OnBugDepart(CoreBug bug)
    { 
    
    }

    float _spread_timer = 0;
    protected void SendGathering()
    {
        if (gather_destination)
        {
            Debug.Log("sending gathering");

            _spread_timer += Time.deltaTime;
            
            // send gathering 
            // keep in the room
            for (int i = 0; i < assigned_bugs.Count; i++)
            {
                CoreBug cb = assigned_bugs[i].GetComponent<CoreBug>();
                if (cb)
                {
                    // moving to location 
                    if (cb.GetAction == CoreBug.Bug_action.idle)
                    {
                        if (_spread_timer < 0.5f) continue;
                        _spread_timer = 0;

                        cb.GoTo(gather_destination);
                        cb.NextAction();
                        OnBugDepart(cb);
                        continue;                
                    }

                    // reached location
                    if (cb.GetAction == CoreBug.Bug_action.traveling)
                    {
                        if (cb.current_cell == gather_destination)
                        {
                            cb.NextAction();
                            OnBugReachGatheringSite(cb);

                            // start gathering 
                            _gather_t = 0;
                            continue;
                        }
                    }

                    // gathering and return back
                    if (cb.GetAction == CoreBug.Bug_action.gathering)
                    {
                        if (cb.current_cell == gather_destination)
                        {
                            // finished gathering 
                            _gather_t += Time.deltaTime;
                            if (_gather_t > gather_duration_time)
                            {
                                if (_spread_timer < 0.5f) continue;
                                _spread_timer = 0;

                                cb.GoTo(this.cell);
                                cb.NextAction();
                                _spread_timer = 0;
                                continue;
                            }
                        }
                    }

                    // reched cell
                    if (cb.GetAction == CoreBug.Bug_action.returning)
                    {
                        if (cb.current_cell == this.cell)
                        {
                            OnBugReachHomeCell(cb);
                            cb.NextAction();
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

   

}
