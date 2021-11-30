using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandCenter : WarRoom
{
    [SerializeField]
    public HiveCell gather_destination;
    public int gather_duration_time = 10;

    Queue<CoreBug> bugs_on_collect_task = new Queue<CoreBug>();
    public void SendToCollect()
    {
        Debug.Log("send hive to collect");

        int[] hive_size = cell.hiveGenerator.GetSize();
        gather_destination = cell.hiveGenerator.cells[hive_size[0] - 1][hive_size[1] - 1];

        for (int i = 0; i < assigned_bugs.Count; i++)
        {
            WarriorBug wb = assigned_bugs[i].GetComponent<WarriorBug>();
            if (wb)
            {
                if (bugs_on_collect_task.Contains(wb) == false)
                    bugs_on_collect_task.Enqueue(wb);
            }
        }
    }

    public override void Update()
    {
        base.Update();
        SendOnPillage();
    }


    float _pillage_t = 0;
    float _p_spread_timer = 0;
    public void SendOnPillage()
    {
        if (gather_destination)
        {

            _p_spread_timer += Time.deltaTime;

            // send gathering 
            // keep in the room
            if (bugs_on_collect_task.Count > 0)
            {

                if (_p_spread_timer > 0.5f)
                {
                    _p_spread_timer = 0;
                }
                else
                {
                    return;
                }


                CoreBug bug = bugs_on_collect_task.Dequeue();
                if (bug)
                {
                    // send on a task 
                    bug.GoTo(gather_destination);
                    bug.bugTask = CoreBug.BugTask.fight;
                    bug.SetAction(CoreBug.Bug_action.traveling);
                }
            }

            for (int i = 0; i < assigned_bugs.Count; i++)
            {
                WarriorBug bug = assigned_bugs[i].GetComponent<WarriorBug>();
                if (bug)
                {
                    if (bug.bugTask == CoreBug.BugTask.fight)
                    {
                        if (bug.underlaying_cell == gather_destination &&
                            bug.GetAction == CoreBug.Bug_action.traveling)
                        {
                            // we have reached the destination 
                            // bring back food 
                            // OnBugReachGatheringSite(bug);
                            bug.GoTo(this.cell);
                            bug.SetAction(CoreBug.Bug_action.returning);
                        }
                    }

                    if (bug.GetAction == CoreBug.Bug_action.returning)
                    {
                        if (bug.underlaying_cell == this.cell)
                        {
                            bug.bugTask = CoreBug.BugTask.none;
                            bug.SetAction(CoreBug.Bug_action.idle);

                            // were we in gathering hunt?
                            OnBugReachHomeCell(bug);
                            OnBugReachHomeCell(bug); // on purpose quick fix

                        }
                    }

                    if (bug.GetAction == CoreBug.Bug_action.idle)
                    {
                        SpreadBugs();
                    }
                }
            }
        }
    }

    public override void OnRoomSelect()
    {
        Debug.Log(this.name + " selected");

        AkSoundEngine.PostEvent("Play_Hatchery", gameObject);
    }


}
