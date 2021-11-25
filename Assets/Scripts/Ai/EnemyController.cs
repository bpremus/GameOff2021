using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Start cell is location where bugs will spawn
    [SerializeField]
    HiveCell start_cell;

    // target is a target that we will attack, default like a hive
    [SerializeField]
    HiveCell target_cell;

    // Queue of bugs 
    [SerializeField]
    Queue<int> bugs_to_spawn = new Queue<int>();

    [SerializeField]
    float spawn_timer = 0;

    int coalition = 1;
    int day_attack_number = 1;
    int night_attack_number = 10;
    public int bug_separation = 1;
    GameController game_controller;

    public void SpawnBug(int bug_prefab_index)
    {
        bugs_to_spawn.Enqueue(bug_prefab_index);
    }

    public void Start()
    {
        game_controller = GameController.Instance;
    }

    public void Update()
    {
        StageAttack();
        SpawnBug();
    }

    bool is_day = false;
    public void StageAttack()
    {
        // day or night, therefore a different attacks 
        bool curret_state = game_controller.ISDayCycle();
        if (is_day != curret_state)
        {
            if (curret_state)
                OnDayAttacks();
            else
                OnNightAttacks();

            d_idx = 0;
        }
        is_day = curret_state;

        /*
        // player condition have to be met 
        // have a queen
        // have a harvester 
        // have one combat unit
        // cycle will start

        // day scout 
        // ------------------------------

        */
    }

    public void OnDayAttacks()
    {
        SetAttack();
       // Debug.Log("Day attack start");

        for (int i = 0; i < day_attack_number; i++)
        {
            bugs_to_spawn.Enqueue(0);
        }

    }

    public void OnNightAttacks()
    {
        SetAttack();
       // Debug.Log("Night attack start");

        for (int i = 0; i < night_attack_number; i++)
        {
            bugs_to_spawn.Enqueue(0);
           // bugs_to_spawn.Enqueue(3);
        }

    }

    public void SetAttack()
    {
        if (target_cell) return;

        HiveGenerator hc = FindObjectOfType<HiveGenerator>();
        if (hc)
        {
            if (hc.cells.Count > 0)
            {
                int[] size = hc.GetSize();
                start_cell = hc.cells[size[0] -1][size[1] -1];
                target_cell = hc.hive_cell;
            }
        }
    }


    public class AITask
    {
        public CoreBug bug;
        public AITask(CoreBug bug)
        {
            this.bug = bug;
        }
        
        public void OnTargetReach()
        {
            int idx = Random.Range(0, ArtPrefabsInstance.Instance.FoodAndWoodPrefabs.Length-1);
            GameObject food_wood = ArtPrefabsInstance.Instance.FoodAndWoodPrefabs[idx];
            Vector3 food_pos = bug.transform.position + bug.transform.up * 0.5f;
            GameObject g = Instantiate(food_wood, food_pos, Quaternion.identity);
            bug.harvest_object = g;

            GameController.Instance.OnStolenFood();
        }
    }


    int d_idx = 0;
    public void SpawnBug()
    {
        if (bugs_to_spawn.Count == 0) return;

        spawn_timer += Time.deltaTime;
        if (spawn_timer > bug_separation)
        {
            spawn_timer = 0;
        }
        else
            return;

       int bug_index = bugs_to_spawn.Dequeue();
       // Debug.Log("int " + bug_index);
       GameObject bug_prefab = ArtPrefabsInstance.Instance.BugsPrefabs[bug_index];
       CoreBug cb = Instantiate(bug_prefab, start_cell.transform.position, start_cell.transform.rotation).GetComponent<CoreBug>();
       if (cb != null)
       {
            cb.CurrentPositon(start_cell);
            cb.GoToAndBack(start_cell,target_cell);
            cb.tag = "Enemy";
            cb.coalition = coalition; // same as tag 

            AITask task = new AITask(cb);
            cb.SetAITask(task);

            cb.SetBugColor(0.2f,0.2f,0.2f);


            d_idx++;
            cb.name = "en_" + d_idx;

            // cb.stop_and_fight = false;
            //cb.GoTo(target_cell);
        }
    }
}

