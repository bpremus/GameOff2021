using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarRoom : HiveRoom
{
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

    public GameObject GetBugFroTransfer(int index)
    {
        if (index < assigned_bugs.Count)
        {
            GameObject g = assigned_bugs[index];
            assigned_bugs.RemoveAt(index);
            SpreadBugs();
            return g;
        }
        return null;
    }


    public override void Update()
    {
        base.Update();
        GuardRoom();
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

            SpreadBugs();
        }
    }

}
