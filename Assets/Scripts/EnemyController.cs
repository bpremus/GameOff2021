using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    CoreBug[] bug_prefabs;

    [SerializeField]
    HiveCell start_cell;

    [SerializeField]
    HiveCell target_cell;


    public void Start()
    {
        SpawnBug();
    }

    public void SpawnBug()
    {
     //   CoreBug cb = Instantiate(bug_prefabs[0], start_cell.transform.position, start_cell.transform.rotation);
     //   if (cb != null)
     //   {
     //       cb.target = target_cell.transform;
     //   }

    }

}

