using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiveRoom : CoreRoom
{
    public override void Start()
    {

    }
    public override void DetachBug(CoreBug bug)
    {
        if (assigned_bugs.Contains(bug))
            assigned_bugs.Remove(bug);
    }
    public override bool AssignBug(CoreBug bug)
    {
        if (assigned_bugs.Count > max_asigned_units) return false;
        assigned_bugs.Add(bug);
        return true;
    }
    [SerializeField] protected Collider[] hitColliders;
    public override void DetectEnemy() { }
    public override void SpreadBugs()
    {
        for (int i = 0; i < assigned_bugs.Count; i++)
        {
            Vector3 move_to = new Vector3(0, -1, 0);
            move_to = Quaternion.Euler(0, 0, 45 - (180 / max_asigned_units * i)) * move_to;
            move_to = move_to.normalized;

            CoreBug cb = assigned_bugs[i].GetComponent<CoreBug>();

            if (cb.current_cell == this.cell)
            {
                if (cb.GetAction == CoreBug.Bug_action.idle ||cb.GetAction == CoreBug.Bug_action.sleeping)
                {
                    cb.target = transform.position + move_to * 0.6f;
                }
            }
        }
    }
    public override void RecallBugs()
    {
        for (int i = 0; i < assigned_bugs.Count; i++)
        {
            CoreBug cb = assigned_bugs[i].GetComponent<CoreBug>();
            cb.GoTo(this.cell);
            cb.SetAction(CoreBug.Bug_action.idle);
        }
    }
    public virtual void SleepnigBugs()
    {
        for (int i = 0; i < assigned_bugs.Count; i++)
        {
            CoreBug cb = assigned_bugs[i].GetComponent<CoreBug>();
            cb.GoTo(this.cell);
            cb.SetAction(CoreBug.Bug_action.sleeping);
        }
    }

    public virtual void SendToCollect(WorldMapCell gathering_destination) { }
}
