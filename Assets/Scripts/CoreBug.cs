using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreBug : BugMovement
{

    public Queue<HiveCell> path = new Queue<HiveCell>();
    public HiveCell current_cell = null;
    public HiveCell destination_cell = null;
    public HiveCell target_cell = null;
    public HiveCell asigned_cell = null;

    Vector3 z_offset = new Vector3(0, 0, 1);

    public virtual void AssignToAroom(HiveCell cell)
    {

    }

    public void CurrentPositon(HiveCell position)
    {
        current_cell = position;
        transform.position = current_cell.transform.position + z_offset;
        this.target = current_cell.transform.position;
    }

    public void GoToAndBack(HiveCell start, HiveCell destination, float wait_timer = 0)
    {
        target_cell = destination;
        destination_cell = start;

        List<HiveCell> move_path = AiController.GetPath(start, destination);
        for (int i = 0; i < move_path.Count; i++)
        {
            path.Enqueue(move_path[i]);
        }

        path.Enqueue(destination);
        move_path.Reverse();

        for (int i = 0; i < move_path.Count; i++)
        {
            path.Enqueue(move_path[i]);
        }

        this.target = current_cell.transform.position + z_offset;
    }

    public void GoTo(HiveCell destination)
    {
        target_cell = destination;
        destination_cell = destination;

        List<HiveCell> move_path = AiController.GetPath(current_cell, destination);
        for (int i = 0; i < move_path.Count; i++)
        {
            path.Enqueue(move_path[i]);
        }

        this.target = current_cell.transform.position + z_offset;
    }

    public override void WalkPath()
    {
        if (path.Count > 0)
        {
            HiveCell c = path.Peek();    
            float t = Vector3.Distance(c.transform.position + z_offset, transform.position);
            if (t <= 0.5f)
            {
                current_cell = path.Dequeue();
                if (path.Count > 0)
                {
                    if (target_cell == current_cell)
                    {
                        OnTargetReach();
                    }
                }
                else
                {
                    OnDestinationReach();
                }
                return;
            }
            this.target = c.transform.position + z_offset;
        }
        else
        {
            if (destination_cell != null)
            this.target = destination_cell.transform.position + z_offset;
        }
    }

    public void OnDestinationReach()
    {

        Debug.Log("destination reached");
        
    }

    public void OnTargetReach()
    {
        Debug.Log("target reached");
    }


}
