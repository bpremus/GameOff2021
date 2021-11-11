using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiveRoom : CoreRoom
{
    public float room_detect_distance = 3;

    [SerializeField]
    protected GameObject[] assigned_bugs;

    [SerializeField]
    GameObject Dbg_destination;


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, room_detect_distance);
        Gizmos.color = Color.red;
    }

    public override void Start()
    {
        for (int i = 0; i < assigned_bugs.Length; i++)
        {
            CoreBug b = assigned_bugs[i].GetComponent<CoreBug>();
            b.CurrentPositon(this.parent_cell);
            // b.GoTo(Dbg_destination.GetComponent<HiveCell>());

        }
    }

    public void SendBugToIntercept(HiveCell cell)
    {
        Debug.DrawLine(transform.position, cell.transform.position);

        if (assigned_bugs[0] == null) return;

        CoreBug b = assigned_bugs[0].GetComponent<CoreBug>();
        if (b.destination_cell != cell)
        {
            b.GoTo(cell);
        }
    }

    public void ReturnHome()
    {
        CoreBug b = assigned_bugs[0].GetComponent<CoreBug>();
        if (b.destination_cell == this.parent_cell)
            return;

        b.GoTo(this.parent_cell);
    }


    [SerializeField]
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
