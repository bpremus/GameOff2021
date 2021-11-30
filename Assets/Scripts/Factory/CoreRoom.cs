using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreRoom : MonoBehaviour
{
    [SerializeField]
    protected HiveCell parent_cell;

    [SerializeField]
    protected List<GameObject> assigned_bugs = new List<GameObject>();
    
    [SerializeField]
    protected int max_asigned_units = 3;
    [SerializeField]
    protected float room_detect_distance = 2;

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

    public virtual void Start()
    {
        
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
        assigned_bugs.Remove(bug.gameObject);
    }
    public virtual bool AssignBug(CoreBug bug)
    {
        if (assigned_bugs.Count > max_asigned_units) return false;
        assigned_bugs.Add(bug.gameObject);
        return true;
    }

    public HiveCell cell{
        get { return this.parent_cell;  }
        set { this.parent_cell = value; }
    }

    public virtual void DetectEnemy()
    {

    }

    public virtual void Update()
    {
        DetectEnemy();
    }

    public virtual void OnRoomSelect()
    {
        Debug.Log("core room selected");
    }

}
