using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    HiveGenerator hiveGenerator;

    class BugTask
    {
        public CoreBug.BugEvolution evolution_type;
        public HiveCell destination;
    }
    Queue<BugTask> bugs_to_create = new Queue<BugTask>();

    private RunnerBug PlaceBug(BugTask task)
    {
        // we spawn only runner bugs and classes that inherit runner bugs
        HiveCell spawn_cell = hiveGenerator.GetGatheringCell();
        RunnerBug rb = ArtPrefabsInstance.Instance.SpawnAI(task.evolution_type, spawn_cell);
        rb.SetBugColor(0.1f, 0.1f, 0.1f);
        rb.GoToAndBack(spawn_cell, task.destination);
        return rb;
    }

    float _t_create_separator = 0;
    public void Update()
    {
        _t_create_separator += Time.deltaTime;
        if (bugs_to_create.Count > 0)
        {
            if (_t_create_separator > 0.5f)
            {
                _t_create_separator = 0;
                BugTask bt = bugs_to_create.Dequeue();
                PlaceBug(bt);
            }
        }
    }


    // scout single room
    public void SapwnScount(CoreBug.BugEvolution evolution_type, HiveCell desitnation)
    {

        BugTask bugTask = new BugTask();
        bugTask.evolution_type = evolution_type;
        bugTask.destination = desitnation;

        bugs_to_create.Enqueue(bugTask);
    }

    // scout all player rooms 
    public void SpawnFullScout(CoreBug.BugEvolution evolution_type)
    { 
    
    }

    public void SpawnRaidGroup(CoreBug.BugEvolution evolution_type)
    { 
        
    }

    public void RecallEnemyBugs()
    { 
    
    }

    // report if bug managed to steal something from player
    public void ReportSucessfulScout(HiveCell target)
    {
        Debug.Log("bug successfully scouted player's " + target.GetRoom().name);
    }

    // report if we managed to kill some players bugs 
    public void ReportSucessfulRaid(HiveCell target)
    { 
    
    
    }

    // report where bugs are dieing the most 
    public void ReportStrongPoint(HiveCell target)
    { 
        
    
    }

    private static EnemyController _instance;
    public static EnemyController Instance
    {
        get { return _instance; }
    }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        _instance = this;
    }

   
}

