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
    [SerializeField] WorldMapCell gathering_destination;

    // we can have it for each separate bug
    // or we handle all bugs ate same time
    // we will go with version 2 for now 
    // later in polish phase we can adjust
    float _gather_t = 0;
    public override void Update()
    {
        base.Update();
        SendGathering();
        DayAndNightCycle();
    }

    bool _isNight = true;
    public void DayAndNightCycle()
    {
        bool day = GameController.Instance.ISDayCycle();
        if (day == false)
        {
            SleepnigBugs(); // put bugs to sleep over night 
            _isNight = true;
        }
        else
        {
            if (_isNight == day)
            {
                _isNight = false;
                WakeBugsUp(); // wake them up
            }
        }
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
    public override void SendToCollect(WorldMapCell gathering_destination)
    {
        this.gathering_destination = gathering_destination;
        gather_destination = cell.hiveGenerator.GetGatheringCell();
    }

    public override void RecallBugs()
    {
        base.RecallBugs();
        gather_destination = null;
    }

    public void WakeBugsUp()
    {
        base.RecallBugs();
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
                    cb.gathering_cell = gathering_destination;
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

                    if (cb.GetAction == CoreBug.Bug_action.sleeping)
                    {
                        SpreadBugs();
                    }

                    // reached location
                    if (cb.GetAction == CoreBug.Bug_action.traveling)
                    {
                        if (cb.current_cell == gather_destination)
                        {
                            cb.NextAction();
                            cb.OnBugReachGatheringSite();

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

                            WorldMapGenerator.Instance.ReportVisitedCell(cb.gathering_cell);
                            cb.OnBugReachHomeCell();
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
