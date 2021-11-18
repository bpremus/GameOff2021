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

    int coalition = 1;

    public void Start()
    {
        
    }

    public void SetAttack()
    {
        HiveGenerator hc = FindObjectOfType<HiveGenerator>();
        if (hc)
        {
            if (hc.cells.Count > 0)
            {
                int[] size = hc.GetSize();
                start_cell = hc.cells[size[0] -1][size[1] -1];
                target_cell = hc.hive_cell;
            }
        }
    }

    [SerializeField]
    int max_bug = 2;
    float spawn_timer = 0;
    public void Update()
    {

        SetAttack();

        spawn_timer += Time.deltaTime;
        if (spawn_timer > 2)
        {
            spawn_timer = 0;
        }
        else
            return;

        if (max_bug <= 0)
            return;

        max_bug--;
        SpawnBug();
    }

    public void SpawnBug()
    {
       CoreBug cb = Instantiate(bug_prefabs[0], start_cell.transform.position, start_cell.transform.rotation);
       if (cb != null)
       {
            cb.CurrentPositon(start_cell);
            cb.GoToAndBack(start_cell,target_cell);
            cb.tag = "Enemy";
            cb.coalition = coalition; // same as tag 

            // cb.stop_and_fight = false;
            //cb.GoTo(target_cell);
        }
    }
}

