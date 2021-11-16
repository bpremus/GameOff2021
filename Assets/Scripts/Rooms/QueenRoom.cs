using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueenRoom : HiveRoom
{
    // this one is unique and we can have only one

    public int spawn_bug_interval = 1;
    float _spawn_t = 0;

    public override void Update()
    {
        base.Update();
        SpawnBug();
    }

    public void SpreadBugs()
    {
        for (int i = 0; i < assigned_bugs.Count; i++)
        {
            Vector3 move_to = new Vector3(0, -1, 0);
            move_to = Quaternion.Euler(0, 0, 45 - (180 / max_asigned_units * i)) * move_to;
            move_to = move_to.normalized;

            CoreBug cb = assigned_bugs[i].GetComponent<CoreBug>();
            if (cb.bug_action == CoreBug.Bug_action.idle)
            {
                cb.target = transform.position + move_to * 0.7f;
            }
           
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
    
    public void SpawnBug()
    {
        _spawn_t += Time.deltaTime;
        if (_spawn_t > spawn_bug_interval)
        {
            _spawn_t = 0;

            if (assigned_bugs.Count < max_asigned_units)
            {
                int i = assigned_bugs.Count;
                Vector3 move_to = new Vector3(0, -1, 0);
                move_to = Quaternion.Euler(0, 0, 45 - (180 / max_asigned_units * i)) * move_to;
                move_to = move_to.normalized;

                // queen consume food to build a drone 
                // drone is then evolved into higher tier 

                GameObject bug_prefab = ArtPrefabsInstance.Instance.BugsPrefabs[0];
                GameObject bug_instance = Instantiate(bug_prefab, transform.position, Quaternion.identity);
                if (bug_instance)
                {

                    assigned_bugs.Add(bug_instance);

                    CoreBug cb = bug_instance.GetComponent<CoreBug>();
                    cb.CurrentPositon(this.parent_cell);
                    cb.target = transform.position + move_to * 0.7f;
                    // HiveCell hc = this.parent_cell;
                    // HiveCell destination = hc.hiveGenerator.rooms[0];

                    //cb.target = move_to;
                    //cb.GoTo(destination);

                    // if we dont have food return 0
                    // Debug.Log("spawning bug");
                }
            }
        }
    }



}
