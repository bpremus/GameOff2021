using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueenRoom : HiveRoom
{
    // this one is unique and we can have only one

    [SerializeField]
    public HiveCell gather_destination;
    public int gather_duration_time = 1;

    public int spawn_bug_interval = 1;
    float _spawn_t = 100;
    int name_index = 0;
    public override void Update()
    {
        base.Update();
        SpawnBug();
        SendGathering();
    }

    public override void DetachBug(CoreBug bug)
    {
        assigned_bugs.Remove(bug.gameObject);
      //  Debug.Log("deatching a bug from " + this.name);
        SpreadBugs();

    }
    public override bool AssignBug(CoreBug bug)
    {
        if (assigned_bugs.Count > max_asigned_units) return false;
        assigned_bugs.Add(bug.gameObject);
        return true;
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


    Queue<CoreBug> bugs_on_collect_task = new Queue<CoreBug>();
    public void SendToCollect()
    {
        Debug.Log("send hive to collect");

        int[] hive_size = cell.hiveGenerator.GetSize();
        gather_destination = cell.hiveGenerator.cells[hive_size[0] - 1][hive_size[1] - 1];
        
        for (int i = 0; i < assigned_bugs.Count - 1; i++)
        {
            WorkerBug wb = assigned_bugs[i].GetComponent<WorkerBug>();
            if (wb)
            {
                if (bugs_on_collect_task.Contains(wb) == false)
                    bugs_on_collect_task.Enqueue(wb);
            }
        }
    }

    public void SpawnBug()
    {
        _spawn_t += Time.deltaTime;
        if (_spawn_t > spawn_bug_interval)
        {
            _spawn_t = 0;

            if (assigned_bugs.Count < max_asigned_units)
            {

                // consume 1 food to build a new drone
                if (GameController.Instance.GetFood() > 0)
                {
                    GameController.Instance.OnConsumeFood();
                }

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
                    cb.asigned_cell = this.parent_cell;

                    cb.name = "Drone"; // "b_" + name_index;
                    name_index++;

                    GameController.Instance.OnNewBug();


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


    float _gather_t = 0;
    float _spread_timer = 0;
    protected virtual void SendGathering()
    {
        if (gather_destination)
        {
            // Debug.Log("sending gathering");

            _spread_timer += Time.deltaTime;

            // send gathering 
            // keep in the room
            if (bugs_on_collect_task.Count > 0)
            {
                CoreBug bug = bugs_on_collect_task.Dequeue();
                if (bug)
                {
                    // send on a task 
                    bug.GoTo(gather_destination);
                    bug.bugTask = CoreBug.BugTask.harvesting;
                    bug.SetAction(CoreBug.Bug_action.traveling);
                }
            }

            for (int i = 0; i < assigned_bugs.Count; i++)
            {
                WorkerBug bug = assigned_bugs[i].GetComponent<WorkerBug>();
                if (bug)
                {
                    if (bug.bugTask == CoreBug.BugTask.harvesting)
                    {
                        if (bug.underlaying_cell == gather_destination &&
                            bug.GetAction == CoreBug.Bug_action.traveling)
                        {
                            // we have reached the destination 
                            // bring back food 
                            OnBugReachGatheringSite(bug);
                            bug.GoTo(this.cell);
                            bug.SetAction(CoreBug.Bug_action.returning);
                        }
                    }
                    if (bug.GetAction == CoreBug.Bug_action.returning)
                    {
                        if (bug.underlaying_cell == this.cell)
                        {
                            bug.bugTask = CoreBug.BugTask.none;
                            bug.SetAction(CoreBug.Bug_action.idle);

                            // were we in gathering hunt?
                            OnGatherComlpete(bug);
                          
                        }
                    }
                    if (bug.GetAction == CoreBug.Bug_action.idle)
                    {
                        SpreadBugs();
                    }
                }
            }
        }
    }

 
    public void OnGatherComlpete(CoreBug bug)
    {
        if (bug.harvest_object == null) return;

        Debug.Log("bug has reached destination");

        Destroy(bug.harvest_object);
            bug.harvest_object = null;

        OnBugReachHomeCell(bug);
    }

    public void OnBugReachGatheringSite(CoreBug bug)
    {
        Debug.Log("bugs are gathering");
        if (bug.harvest_object != null)
        {
            Destroy(bug.harvest_object);
            bug.harvest_object = null;
        }

        int idx = Random.Range(0, ArtPrefabsInstance.Instance.FoodAndWoodPrefabs.Length);
        GameObject food_wood = ArtPrefabsInstance.Instance.FoodAndWoodPrefabs[idx];
        Vector3 food_pos = new Vector3(0, 0, -5);
        GameObject g = Instantiate(food_wood, food_pos, Quaternion.identity);
        bug.harvest_object = g;
    }


    public void OnBugReachHomeCell(CoreBug bug)
    {
        Debug.Log("bugs returned home");
        GameController.Instance.OnBrigResources();
    }

    public void OnBugDepart(CoreBug bug)
    {

    }

    public override void OnRoomSelect()
    {
        Debug.Log(this.name + " selected");
    }



}
