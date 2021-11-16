using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiveRoom : CoreRoom
{
    public float room_detect_distance = 3;

    [SerializeField]
    protected List<GameObject> assigned_bugs = new List<GameObject>();
    [SerializeField]
    protected int max_asigned_units = 3;

    public void AssignBug(GameObject bug)
    {
        assigned_bugs.Add(bug);
    }

    private void OnDrawGizmos()
    {
      //  Gizmos.DrawWireSphere(transform.position, room_detect_distance);
      //  Gizmos.color = Color.red;
    }

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

    public void SendBugToIntercept(HiveCell cell)
    {
        Debug.DrawLine(transform.position, cell.transform.position);

        if (assigned_bugs.Count == 0) return;

        CoreBug b = assigned_bugs[0].GetComponent<CoreBug>();
        if (b.destination_cell != cell)
        {
            b.GoTo(cell);
        }
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


    protected Collider[] hitColliders;
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
                if (cb.IsValidState() == false) continue;
                if (cb.tag == "Enemy")
                {
                    SendBugToIntercept(cb.current_cell);
                    return;
                }
            }
        }
        ReturnHome();
    }

}
