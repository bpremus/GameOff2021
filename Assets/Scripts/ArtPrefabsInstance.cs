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


    public enum BugEvolution { drone, warrior, claw, spike };
    public void EvolveBug(CoreBug bug, BugEvolution evolve)
    {
        // bug category
    
        // detach bug from rooms 
        HiveCell cell = bug.asigned_cell;
        cell.DetachDrone(bug);
        // evolve bug

        GameObject g = Instantiate(BugsPrefabs[2], bug.transform.position, bug.transform.rotation);
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

            Destroy(bug.gameObject);

        }
    }



}
