using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueenRoom : HiveRoom
{
    // this one is unique and we can have only one

    // Save and Load 
    // ----------------------------
    [System.Serializable]
    public class SaveQueenRoom
    {
        public int gather_duration_time;
        public int spawn_bug_interval;
        public float _spawn_t;
    }

    public SaveQueenRoom GetSaveQueenData()
    {
        SaveQueenRoom data = new SaveQueenRoom();
        data.gather_duration_time = this.gather_duration_time;
        data.spawn_bug_interval = this.spawn_bug_interval;
        data._spawn_t = this._spawn_t;
        return data;
    }

    public void SetSaveQueenData(SaveQueenRoom data)
    {
        this.gather_duration_time = data.gather_duration_time;
        this.spawn_bug_interval = data.spawn_bug_interval;
        this._spawn_t = data._spawn_t;

        SpreadBugs();
    }
    
    [SerializeField]
    public HiveCell gather_destination;
    public int gather_duration_time = 1;

    public int spawn_bug_interval = 1;
    float _spawn_t = 0;
    int name_index = 0;
    public override void Update()
    {
        base.Update();
       // SpawnBug();
        SendGathering();
        BugSpawner();
    }
    public override void DetachBug(CoreBug bug)
    {
        assigned_bugs.Remove(bug);
        SpreadBugs();
    }
    public override bool AssignBug(CoreBug bug)
    {
        if (assigned_bugs.Count > max_asigned_units) return false;
        assigned_bugs.Add(bug);
        return true;
    }

    public CoreBug GetBugFroTransfer(int index)
    {
        if (index < assigned_bugs.Count)
        {
            CoreBug g = assigned_bugs[index];
            assigned_bugs.RemoveAt(index);
            SpreadBugs();
            return g;
        }
        return null;
    }
    
    Queue<CoreBug> bugs_on_collect_task = new Queue<CoreBug>();
    public override void SendToCollect(WorldMapCell gathering_destination)
    {
        Debug.Log("send hive to collect");

        int[] hive_size = cell.hiveGenerator.GetSize();
        gather_destination = cell.hiveGenerator.GetGatheringCell();
        
        for (int i = 0; i < assigned_bugs.Count; i++)
        {
            WorkerBug wb = assigned_bugs[i].GetComponent<WorkerBug>();
            if (wb)
            {
                if (bugs_on_collect_task.Contains(wb) == false)
                {
                    wb.gathering_cell = gathering_destination;
                    bugs_on_collect_task.Enqueue(wb);
                }
            }
        }
    }

    [SerializeField]
    float bug_spawn_perc = 0;
    Queue<int> bugs_to_spawn = new Queue<int>();

    [SerializeField]
    ProgressBar bug_spawn_bar;
    protected void BugSpawner()
    {
        if (bugs_to_spawn.Count > 0)
        {
            // get current bug to be spawned 
            _spawn_t += Time.deltaTime;
            if (_spawn_t > spawn_bug_interval)
            {
                int bug_type = bugs_to_spawn.Dequeue();
                PlaceBugOnMap();
                bug_spawn_bar.HideProgressBar();
                _spawn_t = 0;

                OnDroneSpawn();
                if (assigned_bugs.Count == max_asigned_units)
                {
                    OnMaxDronesInRoom();
                }
            }
            else
            {
                // progress bar;
                bug_spawn_perc = _spawn_t / spawn_bug_interval;
                bug_spawn_bar.SetProgress(bug_spawn_perc);
            }
        }
    }

    protected void PlaceBugOnMap()
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

            CoreBug cb = bug_instance.GetComponent<CoreBug>();
            assigned_bugs.Add(cb);
            cb.CurrentPositon(this.parent_cell);
            cb.target = transform.position + move_to * 0.7f;
            cb.asigned_cell = this.parent_cell;

            cb.name = "Worker"; // "b_" + name_index;
            name_index++;

            GameController.Instance.OnNewBug();
        }
    }

    public void OnDroneSpawn()
    {
        ActionLogger.Instance.AddLog("A new worker has been born, yay!", 1);
    }

    public void OnMaxDronesInRoom()
    {
        ActionLogger.Instance.AddLog("Queen room is full!", 2);
    }

    public void OnNotEnoughResrouces()
    { 
        
    }

    public void OnGatherComlpete(CoreBug bug)
    {
        if (bug.harvest_object == null) return;

        ActionLogger.Instance.AddLog(bug.name + " has return to harvester",0);
        Destroy(bug.harvest_object);
        bug.harvest_object = null;
        WorldMapGenerator.Instance.ReportVisitedCell(bug.gathering_cell);
        bug.OnBugReachHomeCell();
    }

 
  

    public void OnBugDepart(CoreBug bug)
    {
        ActionLogger.Instance.AddLog(Formatter_BugName.Instance.GetBugName(bug.bug_evolution) + " is on its way for resources", 0);
    }

    public bool SpawnBug()
    {
        // this is going to work a bit differently 
        // we can queue the task of creating bug 
        int total_bugs = assigned_bugs.Count + bugs_to_spawn.Count;
        if (total_bugs < max_asigned_units)
        {
            // consume 1 food to build a new drone
            if (GameController.Instance.GetFood() > 0)
            {
                GameController.Instance.OnConsumeFood();
            }
            else
            {
                // not enough food 
                ActionLogger.Instance.AddLog("Not enough resources!", 2);
                return false;
            }

            bugs_to_spawn.Enqueue(0); // drone 
            ActionLogger.Instance.AddLog("Producing worker:  " + bugs_to_spawn.Count + " of " + max_asigned_units, 0);
            return true;
        }

        return false;
    }

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

                if (_spread_timer > 0.5f)
                {
                    _spread_timer = 0;
                }
                else
                {
                    return;
                }

                CoreBug bug = bugs_on_collect_task.Dequeue();
                if (bug)
                {
                    // send on a task 
                    bug.GoTo(gather_destination);
                    bug.bugTask = CoreBug.BugTask.harvesting;
                    bug.SetAction(CoreBug.Bug_action.traveling);

                    OnBugDepart(bug);
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
                            bug.OnBugReachGatheringSite();
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

    public override void OnRoomSelect()
    {
        Debug.Log(this.name + " selected");


    }
}
