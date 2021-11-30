using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ArtPrefabsInstance : MonoBehaviour
{
    // changed to singleton 
    private static ArtPrefabsInstance _instance;
    public static ArtPrefabsInstance Instance
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

    public GameObject[] RoomPrefabs;
    public GameObject[] BugsPrefabs;
    public GameObject[] FoodAndWoodPrefabs;


    // ment for loading 
    public CoreBug SpawnBug(CoreBug.BugEvolution bug_type, HiveCell cell)
    {
        int prefab_index = 0;
        if (bug_type == CoreBug.BugEvolution.drone)
        {
            prefab_index = 0;
        }
        if (bug_type == CoreBug.BugEvolution.warrior)
        {
            prefab_index = 3;
        }
        if (bug_type == CoreBug.BugEvolution.claw)
        {
            prefab_index = 2;
        }
        if (bug_type == CoreBug.BugEvolution.range)
        {
            prefab_index = 4;
        }
        if (bug_type == CoreBug.BugEvolution.cc_bug)
        {
            prefab_index = 5;
        }

        Vector3 new_pos = cell.transform.position + cell.transform.up * 1f;
        GameObject g = Instantiate(BugsPrefabs[prefab_index], new_pos, Quaternion.identity); 
        if (g)
        {
            CoreBug evolved_bug = g.GetComponent<CoreBug>();
            evolved_bug.asigned_cell = cell;
            evolved_bug.current_cell = cell;
            evolved_bug.SiegeBug(false);
            CoreRoom room = cell.GetRoom();
            room.AssignBug(evolved_bug);
            evolved_bug.GoTo(cell);
            SetBugName(evolved_bug);

            return evolved_bug;
        }

        return null;
    }

    public void EvolveBug(CoreBug bug, int prefab_index)
    {
        // detach bug from rooms 
        HiveCell cell = bug.asigned_cell;
        cell.DetachDrone(bug);
        // evolve bug

        GameObject g = Instantiate(BugsPrefabs[prefab_index], bug.transform.position, bug.transform.rotation);
        if (g)
        {     
            CoreBug evolved_bug = g.GetComponent<CoreBug>();
            
            evolved_bug.asigned_cell = cell;
            CoreRoom room = cell.GetRoom();
            room.AssignBug(evolved_bug);
            // evolved_bug.target = bug.target;
            evolved_bug.current_cell = bug.current_cell;
            evolved_bug.GoTo(cell);

            //cell.AssignDrone(bug); <-- something is wrong 
            SetBugName(evolved_bug);

            Destroy(bug.gameObject);
        }
    }

    public void SetBugName(CoreBug bug)
    {

        string bug_name = bug.name;
        if (bug.bug_evolution == CoreBug.BugEvolution.drone)
        {
            bug_name = "Drone";
        }
        else if (bug.bug_evolution == CoreBug.BugEvolution.warrior)
        {
            bug_name = "Warrior";
        }
        else if (bug.bug_evolution == CoreBug.BugEvolution.claw)
        {
            bug_name = "Claw";
        }
        else if (bug.bug_evolution == CoreBug.BugEvolution.range)
        {
            bug_name = "Spit";
        }
        else if (bug.bug_evolution == CoreBug.BugEvolution.cc_bug)
        {
            bug_name = "Slow";
        }

        bug.name = bug_name;

    }

}
