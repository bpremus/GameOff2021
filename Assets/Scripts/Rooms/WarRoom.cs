using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarRoom : HiveRoom
{
    // if bugs are close to room, we will intercept these 

    [SerializeField]
    HiveCell last_detected_cell;
    public string roomName { get; private set; } = "Salvage room";

    public override void Start()
    {

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

    public override void OnRoomSelect()
    {
        Debug.Log(this.name + " selected");

        AkSoundEngine.PostEvent("Play_WarRoom", gameObject);
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
    public virtual void SendGathering()
    {
        _spread_timer += Time.deltaTime;
        for (int i = 0; i < assigned_bugs.Count; i++)
        {
            WarriorBug cb = assigned_bugs[i].GetComponent<WarriorBug>();
            if (cb)
            {
                // set task if combat capable bug        
                cb.bugTask = CoreBug.BugTask.fight;

                //if (_spread_timer < 0.5f) continue;
                //   _spread_timer = 0;

                if (last_detected_cell != null)
                {
                    Debug.DrawLine(cb.transform.position, last_detected_cell.transform.position);
                    if (cb.GetAction == CoreBug.Bug_action.idle)
                    {
                        cb.GoTo(last_detected_cell);
                        cb.SetAction(CoreBug.Bug_action.traveling);
                    }
                    if (cb.GetAction == CoreBug.Bug_action.fighting)
                    {
                        // cb.StopPath();
                    }
                }
                else
                {
                    //  cb.GoTo(cb.asigned_cell);
                    //  cb.SetState(BugMovement.BugAnimation.walk);
                    Debug.DrawLine(cb.transform.position, cb.asigned_cell.transform.position);
                    if (cb.GetAction == CoreBug.Bug_action.idle)
                    {

                        if (cb.underlaying_cell != cb.asigned_cell)
                            cb.GoTo(cb.asigned_cell);
                        else
                            SpreadBugs();
                    }
                }
            }
            else
            {
                SpreadBugs();
            }
        }
    }


    public override void DetectEnemy()
    {
        if (assigned_bugs.Count == 0) return;

        int layerId = 7; //bugs
        int layerMask = 1 << layerId;
        hitColliders = Physics.OverlapSphere(transform.position, room_detect_distance, layerMask);
        foreach (var hitCollider in hitColliders)
        {
            CoreBug cb = hitCollider.GetComponent<CoreBug>();
            if (cb)
            {
                if (cb.IsDead()) continue;
                if (coalition == cb.coalition) continue;

                // keep the fight inside hive
                if (cb.underlaying_cell.cell_Type == CellMesh.Cell_type.corridor ||
                    cb.underlaying_cell.cell_Type == CellMesh.Cell_type.room ||
                    cb.underlaying_cell.cell_Type == CellMesh.Cell_type.entrance)
                {
                    Debug.DrawLine(transform.position, cb.underlaying_cell.transform.position, Color.yellow);
                    last_detected_cell = cb.underlaying_cell;
                    return;
                }
               
            }
        }

        last_detected_cell = null;
    }
  
}
