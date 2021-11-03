using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreBug : MonoBehaviour
{
    public float bug_movement_speed = 1f;

    [SerializeField]
    public HiveGenerator hiveGrid;

    [SerializeField]
    public HiveCell destionation;

    [SerializeField]
    public HiveCell current_cell;

    [SerializeField]
    List<HiveCell> path = new List<HiveCell>();

    public void Update()
    {
        MoveToCell();
    }


    // Prototype movent, need to be optimized

    public void MoveAround()
    {
        List<HiveCell> walkable_cells = new List<HiveCell>();
        for (int i = 0; i < hiveGrid.cells.Count; i++)
        {
            for (int j = 0; j < hiveGrid.cells[i].Count; j++)
            {
                HiveCell c = hiveGrid.cells[i][j];
                if (c.walkable == 1)
                {
                    walkable_cells.Add(c);
                }
            }
        }
     
        if (walkable_cells.Count > 0)
        {
            if (current_cell == null)
                current_cell = walkable_cells[Random.Range(0, walkable_cells.Count - 1 )];
                destionation = walkable_cells[Random.Range(0, walkable_cells.Count - 1)];
        }
    }

    public void MoveToCell()
    {
        if (destionation == null || current_cell == null)
        {
            Debug.Log("new path");
            MoveAround();
            return;
        }

        if (current_cell != destionation)
        {
            // move to
            Debug.Log("start to move");

            if (path.Count == 0)
            {
                path = AiController.GetPath(current_cell, destionation);
                Debug.Log("generating new path " + path.Count);

            }

            // move slowly 
            Debug.Log("p " + path.Count);
            if (path.Count > 1)
            {
                Debug.Log("path ok");

                float d = Vector3.Distance(transform.position, path[0].transform.position);
                if (d < 0.2f)
                {
                    Debug.Log("dist ok " + d);
                    current_cell = path[0];
                    path.RemoveAt(0);
                    return;
                }

                Debug.Log("move");

                Vector3 dir = path[0].transform.position - transform.position;
                dir = dir.normalized;

                transform.position += dir * bug_movement_speed * Time.deltaTime;
            }
            else
            {
                Debug.Log("path empty");
                destionation = null;
                path.Clear();
            }
        }
        else
        {
            Debug.Log("same");
            destionation = null;
        }


    }



}
