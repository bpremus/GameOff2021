using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// [ExecuteInEditMode] <- turn on for running this in editor
public class GameController : MonoBehaviour
{
    // Singleton class

    // Stats 
    // -----------------------------
    [SerializeField]
    int food = 20;
    [SerializeField]
    int wood = 100;
    [SerializeField]
    int population = 2;

    // day and night cylces 
    [SerializeField]
    float day_night_duration = 1;
    [SerializeField]
    bool isDay = true;
    [SerializeField]
    float dayDuration = 0;

    [SerializeField]
    int day_count = 0;


    public void ResetGame()
    {
        food = 20;
        wood = 20;
        population = 2;
        day_count = 0;

        // we need to rest AI 

    }


    // costs                 wood, food 
    [SerializeField] int[] room_corridor_cost = { 5, 1 };
    [SerializeField] int[] room_harvester_cost = { 10, 3 };
    [SerializeField] int[] room_salvage_cost = { 10, 5 };
    [SerializeField] int[] room_war_cost = { 10, 5 };
    [SerializeField] int[] room_queen_cost = { 20, 10 };

    [SerializeField] int[] bug_return_resources = { 5, 10 };

    [SerializeField] int[] evolve_drone         = { 5, 0 };
    [SerializeField] int[] evolve_super_drone   = { 25, 10 };
    [SerializeField] int[] evolve_warrior       = { 25, 10 };
    [SerializeField] int[] evolve_claw          = { 100, 20 };
    [SerializeField] int[] evolve_spike         = { 200, 20 };
    [SerializeField] int[] evolve_slow          = { 150, 20 };

    // consume resources

    bool DoWeHaveEnoughResources(int[] cost)
    {

        // food 
        bool can_consume = true;
        if (food < cost[0])
        {
            can_consume = false;
        }

        // wood 
        if (wood < cost[1])
        {
            can_consume = false;
        }

        // we can add minimal population if needed


        return can_consume;
    }
    bool SpendResources(int[] cost)
    {
        food -= cost[0];
        wood -= cost[1];
        return true;
    }
    bool RetrieveResources(int[] cost)
    {
        food += cost[0];
        wood += cost[1];
        return true;
    }
    bool SellBuilding(int[] cost)
    {
        food += (int)(((float)(cost[0])) *0.3f);
        wood += (int)(((float)(cost[1])) *0.3f);
        return true;
    }
    public int[] GetRoomCost(int roomIndex)
    {
        switch (roomIndex)
        {
            case 0:
                return room_corridor_cost;
                break;
            case 1:
                return room_salvage_cost;
                break;
            case 2:
                return room_war_cost;
                break;
            case 3:
                return room_harvester_cost;
                break;
            case 4:
                return room_queen_cost;
                break;
            default:
                return room_corridor_cost;
        }
         

    }

    public void OnGameEnd()
    { 
    
    }

    public void OnGameRestart()
    { 
      
    }

    public void OnRooomBuild(HiveCell.RoomContext context)
    {
        // define cost of each buildning 
        if (context == HiveCell.RoomContext.corridor)
        {
            SpendResources(room_corridor_cost);
        }
        if (context == HiveCell.RoomContext.harvester)
        {
            SpendResources(room_harvester_cost);
        }
        if (context == HiveCell.RoomContext.salvage)
        {
            SpendResources(room_salvage_cost);
        }
        if (context == HiveCell.RoomContext.war)
        {
            SpendResources(room_war_cost);
        }
        if(context == HiveCell.RoomContext.queen)
        {
            SpendResources(room_queen_cost);
        }
    }
    // bring back some resources
    public void OnRoomDestroyed(HiveCell.RoomContext context)
    {
        // define cost of each buildning 
        if (context == HiveCell.RoomContext.corridor)
        {
            RetrieveResources(room_corridor_cost);
        }
        if (context == HiveCell.RoomContext.harvester)
        {
            RetrieveResources(room_harvester_cost);
        }
        if (context == HiveCell.RoomContext.salvage)
        {
            RetrieveResources(room_salvage_cost);
        }
        if (context == HiveCell.RoomContext.war)
        {
            RetrieveResources(room_war_cost);
        }
    }
    public bool CanBuild(HiveCell.RoomContext context)
    {
        // define cost of each buildning 
        if (context == HiveCell.RoomContext.corridor)
        {
            return DoWeHaveEnoughResources(room_corridor_cost);
        }
        if (context == HiveCell.RoomContext.harvester)
        {
            return DoWeHaveEnoughResources(room_harvester_cost);
        }
        if (context == HiveCell.RoomContext.salvage)
        {
            return DoWeHaveEnoughResources(room_salvage_cost);
        }
        if (context == HiveCell.RoomContext.war)
        {
            return DoWeHaveEnoughResources(room_war_cost);
        }
        return false;
    }

    // Events 
    // ------------------------------

    public void OnDayStart() 
    {
        AkSoundEngine.PostEvent("Stop_Ambient_Night", gameObject);

        AkSoundEngine.PostEvent("Play_Ambient_Day", gameObject);

        AkSoundEngine.PostEvent("Play_Day_Music", gameObject);

        AkSoundEngine.PostEvent("Stop_Night_Music", gameObject);

        day_count++;
    }
    public void OnNightStart()
    {
        AkSoundEngine.PostEvent("Stop_Ambient_Day", gameObject);

        AkSoundEngine.PostEvent("Play_Ambient_Night", gameObject);

        AkSoundEngine.PostEvent("Play_Night_Music", gameObject);

        AkSoundEngine.PostEvent("Stop_Day_Music", gameObject);
    }
    public void OnAttackStart()
    { 
    
    }
    public void OnConsumeFood()
    {
        food -= 1;
    }
    public void OnBrigResources()
    {
        food += bug_return_resources[0];
        wood += bug_return_resources[1];
    }
    public void OnStolenFood()
    {
        food -= 1;
    }
    public void OnNewBug()
    {
        population += 1;
    }
    public void OnBugDied()
    {
        population -= 1;
    }

    public bool EvolveBug(CoreBug.BugEvolution bugEvolution) 
    {
        if (bugEvolution == CoreBug.BugEvolution.warrior)
        {
            if (food >= evolve_super_drone[0] && wood >= evolve_super_drone[1])
            {
                food -= evolve_super_drone[0];
                wood -= evolve_super_drone[1];
                return true;
            }   
        }
        else if (bugEvolution == CoreBug.BugEvolution.claw)
        {
            if (food >= evolve_claw[0] && wood >=  evolve_claw[1])
            {
                food -= evolve_claw[0];
                wood -= evolve_claw[1];
                return true;
            }
        }
        else if (bugEvolution == CoreBug.BugEvolution.range)
        {
            if (food >= evolve_spike[0] && wood >= evolve_spike[1])
            {
                food -= evolve_spike[0];
                wood -= evolve_spike[1];
                return true;
            }
        }
        else if (bugEvolution == CoreBug.BugEvolution.cc_bug)
        {
            if (food >= evolve_slow[0] && wood >= evolve_slow[1])
            {
                food -= evolve_slow[0];
                wood -= evolve_slow[1];
                return true;
            }
        }

        return false;
    }

    public bool IsLevelUpOnly(CoreBug bug)
    {
        if (bug.bug_evolution == CoreBug.BugEvolution.claw)
        {
            return true;
        }
        if (bug.bug_evolution == CoreBug.BugEvolution.range)
        {
            return true;
        }
        if (bug.bug_evolution == CoreBug.BugEvolution.cc_bug)
        {
            return true;
        }

        return false;
    }


    // methods 
    // ------------------------------
    public int GetFood()
    {
        return food;
    }
    public int GetWood()
    {
        return wood;
    }
    public int GetPopulation()
    {
        return population;
    }
    public float GetTimePercent()
    {
        return Mathf.Floor((_t_day_duration / day_night_duration) * 100);
    }
    public bool ISDayCycle()
    {
        return isDay;
    }
    public int GetDayS()
    {
        return day_count;
    }

    public void OnLoadOverride(int food, int wood, int population, int day_count)
    {
        this.food = food;
        this.wood = wood;
        this.population = population;
        this.day_count = day_count;
        isDay = true;
    }

    protected void Update()
    {
        TimeCycle();
        dayDuration = GetTimePercent();
        ConsumeFoodThick();
    }

    // Protected
    // ------------------------------

    float _food_t = 0;
    float food_conusme_thick = 5;
    public void ConsumeFoodThick()
    {
        _food_t += Time.deltaTime;
        if (_food_t > food_conusme_thick)
        {
            _food_t = 0;
        }
        else
        {
            return;
        }

        if (population < 10)
        {
            food -= 1;
            return;
        }
        food -= population;
    }
    private float _t_day_duration = 0;
    protected void TimeCycle()
    {
        _t_day_duration += Time.deltaTime;
        if (_t_day_duration > day_night_duration)
        {
            if (isDay == true)
            {
                isDay = false;
                OnNightStart();
            }
            else
            {
                isDay = true;
                OnDayStart();
            }
            _t_day_duration = 0;
        }
    }
    private static GameController _instance;
    public static GameController Instance
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

    private void Start()
    {

         // fire first day or night 
         if (isDay)
             OnDayStart();
         else
             OnNightStart();

        day_count = 0; // set day to one




        //playerprefs inits
        PlayerPref.Instance.IncreaseGamesPlayed();
        PlayerPref.Instance.UpdatePlayerSoundSettings();
    }

}
