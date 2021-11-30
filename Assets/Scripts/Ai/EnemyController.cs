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
    HiveCell hive_cell = null;
    [SerializeField]
    List<HiveCell> harvester_cells = new List<HiveCell>();
    [SerializeField]
    List<HiveCell> salvage_cells = new List<HiveCell>();

    // hive generator 
    protected HiveGenerator hc = null;

    // Queue of bugs 
    [SerializeField]
    Queue<BugRaid> bugs_to_spawn = new Queue<BugRaid>();

    [SerializeField]
    float spawn_timer = 0;

    int coalition = 1;
    [SerializeField]
    int day_attack_number = 1;
    [SerializeField]
    int night_attack_number = 10;
    public int bug_separation = 1;
    GameController game_controller;


    // day logic 
    public void OnDayAttacks()
    {
        Debug.Log("Day scout started");

        // day is just a scout;
        SetAttack();
        int elapsed_day = game_controller.GetDayS();

        // every day send 1 more scut and increase stats slightly, there is a stats cap also
        float speed_boost  = elapsed_day * 0.001f;
        float health_boost = elapsed_day * 0.005f;

        int h_room = harvester_cells.Count;
        int s_room = salvage_cells.Count;


        for (int i = 0; i < day_attack_number + elapsed_day ; i++)
        {
            HiveCell target = hive_cell;
            if (i == 0)
            {
                // first scout the hive 
            }
            else if (i < h_room + 1)
            {
                // then scout the storrage rooms 
                target = harvester_cells[i - 1];
            }
            else if (i < h_room + s_room + 1)
            {
                // then scout the salvage rooms 
                target = salvage_cells[i - h_room -1];
            }
            else
            {
                return;
            }

            BugRaid raider = new BugRaid(0, speed_boost, health_boost, 0, target);
            Debug.Log("Bug scouting > " + target.GetRoom().name);
            bugs_to_spawn.Enqueue(raider);

        }
    }

    public void OnNightAttacks()
    {
        Debug.Log("Night raid started");

        // night is for a raid
        int elapsed_day = game_controller.GetDayS();

        // we gout our asses kicked 

        // stats boost 
        float speed_boost = elapsed_day * 0.1f;
        float health_boost = elapsed_day * 0.05f;

        HiveCell target = hive_cell;
        int total_attack_count = night_attack_number + elapsed_day;
        if (total_attack_count > 50) total_attack_count = game_controller.GetPopulation();

        int add_mid_bugs = 0;
        int add_big_bugs = 0;

        // start adding bigger bugs
        add_mid_bugs += (int) (elapsed_day * 0.5f);
        add_big_bugs += (int) (elapsed_day * 0.1f);
        Debug.Log("m>" + add_mid_bugs);

        int split_count = 0;
        int split = total_attack_count / (GoodPillagePoints.Count + 1);
        for (int i = 0; i < total_attack_count; i++)
        {
            // spawning raiders 
            BugRaid raider = new BugRaid(0, speed_boost, health_boost, 0, target);
            bugs_to_spawn.Enqueue(raider);

            if (add_mid_bugs > 0)
            {
                BugRaid pillager = new BugRaid(6, speed_boost, health_boost, 0, target);
                bugs_to_spawn.Enqueue(pillager);
            }

            if (add_mid_bugs > 0)
            {
                BugRaid spike = new BugRaid(7, speed_boost, health_boost, 0, target);
                bugs_to_spawn.Enqueue(spike);
            }

            if (split_count > split)
            {
                split_count = 0;
                if (GoodPillagePoints.Count > 0)
                {
                    target = GoodPillagePoints.Dequeue();
                }
            }
            else
                split_count++;
        }
    }


    public void Start()
    {
        game_controller = GameController.Instance;
    }

    public void Update()
    {
        SetAttack();
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


    public void SetAttack()
    {
        if (hc != null)
        {
            salvage_cells.Clear();
            harvester_cells.Clear();
            for (int i = 0; i < hc.rooms.Count; i++)
            {
                HiveCell room = hc.rooms[i];
                if (room.GetRoom().GetComponent<SalvageRoom>())
                {
                    salvage_cells.Add(room);
                }
                else if (room.GetRoom().GetComponent<HarversterRoom>())
                {
                    harvester_cells.Add(room);
                }
            }
            return;
        }
        hc = FindObjectOfType<HiveGenerator>();
        if (hc)
        {
            if (hc.cells.Count > 0)
            {
                int[] size = hc.GetSize();
                start_cell = hc.cells[size[0] -1][size[1] -1];
                hive_cell = hc.hive_cell;
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

        public void OnDestinationReach()
        {
            // bugs have found a weak spot in our defense
            EnemyController.Instance.AssignGoodPillagePoint(bug.target_cell);
            Debug.Log("bug has reached destination");
            OnDestroyBug();
        }

        public void OnDestroyBug()
        {
            bug.OnAIEndDestroy();
        }

    }

    Queue<HiveCell> GoodPillagePoints = new Queue<HiveCell>();
    public void AssignGoodPillagePoint(HiveCell destination)
    {
        if (GoodPillagePoints.Contains(destination) == false)
            GoodPillagePoints.Enqueue(destination);
    }

    struct BugRaid
    {

        float speed_boost;
        float health_boost;
        int bug_prefab_index;
        int attack_other_bugs;
        HiveCell target;
        int coalition;
        float max_speed_boost;

        public BugRaid(int prefab_index, float speed_boost, float health_boost, int attack, HiveCell target)
        {
            this.bug_prefab_index = prefab_index;
            this.speed_boost = speed_boost;
            this.health_boost = health_boost;
            this.attack_other_bugs = attack;
            this.target = target;
            this.coalition = 1;

            // const 
            max_speed_boost = 2.5f;
        }

        public int GetIndex() { return bug_prefab_index; }
        public HiveCell GetTarget() { return target; }

        public float SpeedBoost() { return speed_boost; }
        public float HealthBoost() { return health_boost; }

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

        BugRaid bug_rider = bugs_to_spawn.Dequeue();
        int idx = bug_rider.GetIndex();
        GameObject bug_prefab = ArtPrefabsInstance.Instance.BugsPrefabs[idx];
        CoreBug cb = Instantiate(bug_prefab, start_cell.transform.position, start_cell.transform.rotation).GetComponent<CoreBug>();
        if (cb != null)
        {

            // colors 
            cb.tag = "Enemy";
            cb.coalition = coalition; // same as tag 
            //cb.SetBugColor(0.2f, 0.2f, 0.2f);

            float r = Random.Range(0.2f, 0.5f);
            float g = Random.Range(0.2f, 0.5f);
            float b = 0.2f;
            cb.SetBugColor(r,g,b);

            cb.CurrentPositon(start_cell);
            cb.GoToAndBack(start_cell, bug_rider.GetTarget());

            // stats boost 
            cb.AIBoost(bug_rider.SpeedBoost(), bug_rider.HealthBoost());


            // generic task for now 
            AITask task = new AITask(cb);
            cb.SetAITask(task);

            d_idx++;
            cb.name = "en_" + d_idx;
        }
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

