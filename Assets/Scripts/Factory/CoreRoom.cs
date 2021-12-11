using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreRoom : MonoBehaviour
{
    // Save and Load 
    // ----------------------------
    [System.Serializable]
    public class SaveCoreRoom
    {
        public int   max_asigned_units;
        public float room_detect_distance;
        public int   coalition;

        // assigned bugs
        public CoreBug.SaveCoreBug[] assigned_bugs;

        // special rooms 
        public QueenRoom.SaveQueenRoom save_queen_data;
        public HarversterRoom.SaveHarversterData save_harvester_data;
    }

    public SaveCoreRoom GetSaveData()
    {
        SaveCoreRoom data = new SaveCoreRoom();
        data.max_asigned_units = this.max_asigned_units;
        data.room_detect_distance = this.room_detect_distance;
        data.coalition = this.coalition;

        data.assigned_bugs = new CoreBug.SaveCoreBug[assigned_bugs.Count];
        for (int i = 0; i < assigned_bugs.Count; i++)
        {
            data.assigned_bugs[i] = assigned_bugs[i].GetSaveData();
        }

        QueenRoom qr = GetComponent<QueenRoom>();
        if (qr)
        {
            data.save_queen_data = qr.GetSaveQueenData();
        }

        HarversterRoom hr = GetComponent<HarversterRoom>();
        if (hr)
        {
            data.save_harvester_data = hr.GetSaveHarversterData();
        }

        return data;
    }

    public void SetRoomData(SaveCoreRoom save)
    {
        this.max_asigned_units = save.max_asigned_units;
        this.room_detect_distance = save.room_detect_distance;
        this.coalition = save.coalition;

        // restore bugs
        for (int i = 0; i < save.assigned_bugs.Length; i++)
        {
             CoreBug.SaveCoreBug bug_data = save.assigned_bugs[i];
             CoreBug bug = ArtPrefabsInstance.Instance.SpawnBug(bug_data.bug_evolution, this.cell);
             bug.SetSaveData(bug_data);
        }

        // special rooms 
        QueenRoom qr = GetComponent<QueenRoom>();
        if (qr)
        {
            qr.SetSaveQueenData(save.save_queen_data);
        }

        HarversterRoom hr = GetComponent<HarversterRoom>();
        if (hr)
        {
            hr.SetSaveHarversterData(save.save_harvester_data);
        }
    }

    [SerializeField] protected HiveCell parent_cell;
    [SerializeField] protected List<CoreBug> assigned_bugs = new List<CoreBug>();
    [SerializeField] protected int max_asigned_units = 3;
    [SerializeField] protected float room_detect_distance = 2;
    public HiveCell cell
    {
        get { return this.parent_cell; }
        set { this.parent_cell = value; }
    }
    public int coalition = 0;
    public int GetMAxAssignUnits() { return max_asigned_units; }
    public void SetMaxUnits(int max_units)
    {
        max_asigned_units = max_units;
    }
    public float GetRomRange() { return room_detect_distance; }
    public void SetRoomRange(float max_range)
    {
        room_detect_distance = max_range;
    }
    public virtual float GetRoomBonusDmg() 
    {
        return 0;
    }
    public virtual float GetRoomBonusHealth()
    {
        return 0;
    }
    public List<CoreBug> GetAssignedBugs()
    {
        List<CoreBug> cbgus = new List<CoreBug>();
        for (int i = 0; i < assigned_bugs.Count; i++)
        {
            cbgus.Add(assigned_bugs[i].GetComponent<CoreBug>());
        }
        return cbgus;
    }
    public bool IsInTheRoomRange(Vector3 target)
    {
        float dist = Vector3.Distance(transform.position, target);
        if (dist < room_detect_distance)
            return true;
        return false;
    }
    public virtual void DetachBug(CoreBug bug)
    {
        assigned_bugs.Remove(bug);
    }
    public virtual void SpreadBugs() 
    {
        Debug.LogError("Why are you calling the base class?");
    }
    public virtual void RecallBugs()
    {
        Debug.LogError("Why are you calling the base class?");
    }
    public virtual bool AssignBug(CoreBug bug)
    {
        if (assigned_bugs.Count > max_asigned_units) return false;
        assigned_bugs.Add(bug);
        return true;
    }
    public virtual void DetectEnemy()
    {

    }
    public virtual void OnRoomSelect()
    {
        Debug.Log("core room selected");
    }
    public virtual void Start()
    {

    }
    public virtual void Update()
    {
        DetectEnemy();
    }
}
