using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiveCorridor : HiveRoom
{
  
    public void GetBugsFromWarRoom()
    {

    }

    public override void Update()
    {
        base.Update();
        GuardRoom();
    }

    public void SiegeBug()
    {
        Debug.Log("SIEGE MODE");
        for (int i = 0; i < assigned_bugs.Count; i++)
        {
            CoreBug bug = assigned_bugs[i].GetComponent<CoreBug>();
            if (bug)
            {
                bug.SiegeBug(true);
            }
        }
    }

    public void UnsiegeBug()
    {
        Debug.Log("move mode");
        for (int i = 0; i < assigned_bugs.Count; i++)
        {
            CoreBug bug = assigned_bugs[i].GetComponent<CoreBug>();
            if (bug)
            {
                bug.SiegeBug(false);
            }
        }
    }


    public void GuardRoom()
    {
        // keep in the room
        for (int i = 0; i < assigned_bugs.Count; i++)
        {
            CoreBug cb = assigned_bugs[i].GetComponent<CoreBug>();
            if (cb)
            {
                if (cb.current_cell != this.cell)
                {
                 //   cb.bug_action = CoreBug.Bug_action.traveling;
                    cb.GoToAndIdle(this.cell);
                }
                else
                {
                  //  cb.bug_action = CoreBug.Bug_action.idle;
                }
            }
        }
    }

    public override void OnRoomSelect()
    {
        Debug.Log(this.name + " selected");


    }
}
