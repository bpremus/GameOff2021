using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiveRoom : CoreRoom
{
    public float room_detect_distance = 3;



    public override void Start()
    {
        for (int i = 0; i < max_asigned_units; i++)
        {
            //  CoreBug b = assigned_bugs[i].GetComponent<CoreBug>();
            //  b.CurrentPositon(this.parent_cell);
            //  // b.GoTo(Dbg_destination.GetComponent<HiveCell>());
           // assigned_bugs.Add(null);
        }
    }

    public override void DetachBug(CoreBug bug)
    {
        assigned_bugs.Remove(bug.gameObject);
    }
    public override bool AssignBug(CoreBug bug)
    {
        if (assigned_bugs.Count > max_asigned_units) return false;
        assigned_bugs.Add(bug.gameObject);
        return true;
    }

  

    public virtual void SendBugToIntercept(HiveCell cell)
    {
    }

    public void ReturnHome()
    {
      //  if (assigned_bugs.Count == 0) return;
      //
      //  CoreBug b = assigned_bugs[0].GetComponent<CoreBug>();
      //  if (b.destination_cell == this.parent_cell)
      //      return;
      //
      //  b.GoTo(this.parent_cell);
    }

    [SerializeField]
    protected Collider[] hitColliders;
    public override void DetectEnemy()
    {
    }

    public virtual void SpreadBugs()
    {
        for (int i = 0; i < assigned_bugs.Count; i++)
        {
            Vector3 move_to = new Vector3(0, -1, 0);
            move_to = Quaternion.Euler(0, 0, 45 - (180 / max_asigned_units * i)) * move_to;
            move_to = move_to.normalized;

            CoreBug cb = assigned_bugs[i].GetComponent<CoreBug>();

            if (cb.current_cell == this.cell)
            {
                if (cb.GetAction == CoreBug.Bug_action.idle)
                {
                    cb.target = transform.position + move_to * 0.6f;
                }
            }
        }
    }

}
