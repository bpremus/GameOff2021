using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarversterRoom : HiveRoom
{

    // Save and Load 
    // ----------------------------
    [System.Serializable]
    public class SaveHarversterData
    {
        public int[] gather_destination;
        public int   gather_duration_time;
        public float _gather_t;
    }

    public SaveHarversterData GetSaveHarversterData()
    {
        SaveHarversterData data   = new SaveHarversterData();
        data.gather_destination   = new int[2] { cell.x, cell.y };
        data.gather_duration_time = this.gather_duration_time;
        data._gather_t            = this._gather_t;
        return data;
    }

    public void SetSaveHarversterData(SaveHarversterData data)
    {
        if (this.gather_destination != null)
        {
            gather_destination = cell.hiveGenerator.GetCell(data.gather_destination[0], data.gather_destination[1]);
        }
        this.gather_duration_time = data.gather_duration_time;
        this._gather_t = data._gather_t;
    }

    [SerializeField]
    public HiveCell gather_destination;
    public int gather_duration_time = 1;

    // we can have it for each separate bug
    // or we handle all bugs ate same time
    // we will go with version 2 for now 
    // later in polish phase we can adjust
    float _gather_t = 0;
    public override void Update()
    {
        base.Update();
        SendGathering();
    }
    public void GetBugsFromHive()
    {
        QueenRoom qr = FindObjectOfType<QueenRoom>();
        if (qr)
        {
            CoreBug bug = qr.GetBugFroTransfer(0);
            assigned_bugs.Add(bug);
            Debug.Log("moving a bug");
        }
    }
    public void SendToCollect()
    {
        int[] hive_size = cell.hiveGenerator.GetSize();
        gather_destination = cell.hiveGenerator.GetGatheringCell();
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
        GameObject g =  Instantiate(food_wood, food_pos, Quaternion.identity);
        bug.harvest_object = g;
    }

    public void OnBugReachHomeCell(CoreBug bug)
    {
        Debug.Log("bugs returned home");
        GameController.Instance.OnBringResources();
    }

    public override void RecallBugs()
    {
        base.RecallBugs();
        gather_destination = null;
    }

    public void OnBugDepart(CoreBug bug)
    {

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
            for (int i = 0; i < assigned_bugs.Count; i++)
            {
                WorkerBug cb = assigned_bugs[i].GetComponent<WorkerBug>();
                if (cb)
                {

                    // set task
                    cb.bugTask = CoreBug.BugTask.harvesting;


                    // moving to location 
                    if (cb.GetAction == CoreBug.Bug_action.idle)
                    {
                        if (_spread_timer < 0.5f) continue;
                        _spread_timer = 0;

                        cb.GoTo(gather_destination);
                        cb.NextAction();
                        OnBugDepart(cb);
                        continue;
                    }

                    // reached location
                    if (cb.GetAction == CoreBug.Bug_action.traveling)
                    {
                        if (cb.current_cell == gather_destination)
                        {
                            cb.NextAction();
                            OnBugReachGatheringSite(cb);

                            // start gathering 
                            _gather_t = 0;
                            continue;
                        }
                    }

                    // gathering and return back
                    if (cb.GetAction == CoreBug.Bug_action.gathering)
                    {
                        if (cb.current_cell == gather_destination)
                        {
                            // finished gathering 
                            _gather_t += Time.deltaTime;
                            if (_gather_t > gather_duration_time)
                            {
                                if (_spread_timer < 0.5f) continue;
                                _spread_timer = 0;

                                cb.GoTo(this.cell);
                                cb.NextAction();
                                _spread_timer = 0;
                                continue;
                            }
                        }
                    }

                    // reached cell
                    if (cb.GetAction == CoreBug.Bug_action.returning)
                    {
                     
                        if (cb.underlaying_cell == this.cell)
                        {
                            OnBugReachHomeCell(cb);
                            cb.NextAction();


                            Destroy(cb.harvest_object);
                            cb.harvest_object = null;

                            continue;
                        }
                    }
                }
                else
                {
                    SpreadBugs();
                }
            }
        }
        else
        {
            SpreadBugs();
        }
    }

    public override void OnRoomSelect()
    {
        Debug.Log(this.name + " selected");
    }

}
