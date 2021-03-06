using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// [ExecuteInEditMode] <- turn on for running this in editor
public class GameController : MonoBehaviour
{
    #region Save and Load
    // Save and Load 
    // ----------------------------
    [System.Serializable]
    public class SaveGameController
    {
        public int  food;
        public int  wood;
        public int  population;
        public bool isDay;
        public int  day_count;
    }

    public SaveGameController GetSaveData()
    {
        SaveGameController data = new SaveGameController();
        data.food = this.food;
        data.wood = this.wood;
        data.population = this.population;
        data.isDay = this.isDay;
        data.day_count = this.day_count;
        return data;
    }

    public void SetSaveData(SaveGameController data)
    {
        this.food        = data.food;
        this.wood        = data.wood;
        this.population  = data.population;
        this.isDay       = data.isDay;
        this.day_count   = data.day_count;

        // reset counters 
        _t_day_duration = 0;
        _food_t = 0;
    }

    #endregion

    #region Stats,variables
    // Stats 
    // -----------------------------
    [SerializeField]
    int food = 20;
    [SerializeField]
    int wood = 100;
    [SerializeField]
    int population = 2;

    // day and night cycles 
    [SerializeField]
    float day_night_duration = 1;
    [SerializeField]
    bool isDay = true;
    [SerializeField]
    float dayDuration = 0;

    [SerializeField]
    int day_count = 0;

    private int lastFoodValue;
    private int lastWoodValue;
    private int lastPopulationValue;


    [SerializeField] private VolumeSlider[] volumeSliders;
    #endregion
  
    #region Room and evolution costs

    // costs                 wood, food 
    [SerializeField] int[] room_corridor_cost = { 5, 1 };
    [SerializeField] int[] room_harvester_cost = { 10, 3 };
    [SerializeField] int[] room_salvage_cost = { 10, 5 };
    [SerializeField] int[] room_war_cost = { 10, 5 };
    [SerializeField] int[] room_queen_cost = { 20, 10 };

    [SerializeField] int[] bug_return_resources = { 5, 10 };



    //costs               food wood
    [SerializeField] int[] evolve_drone         = { 5, 0 };
    [SerializeField] int[] evolve_super_drone   = { 25, 10 };
    [SerializeField] int[] evolve_warrior       = { 25, 10 };
    [SerializeField] int[] evolve_claw          = { 100, 20 };
    [SerializeField] int[] evolve_spike         = { 200, 20 };
    [SerializeField] int[] evolve_slow          = { 150, 20 };

    #endregion

    #region Spending and retrieving resources
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

    #endregion

    #region Check if value of resource changed
    public bool OnFoodValueChanged()
    {
        //this gets called only in one frame
        if(lastFoodValue != food)
        {
            int difference = food - lastFoodValue;
            lastFoodValue = food;
            //here we can play animation of food added (+difference)
            ValueChangedDisplay.Instance.OnNewValue(difference, 0);
            return true;
        }
        return false;
    }
    public bool OnWoodValueChanged()
    {
        if (lastWoodValue != wood)
        {
            int difference = wood - lastWoodValue;
            lastWoodValue = wood;
            //here we can play animation of wood added (+difference)
            ValueChangedDisplay.Instance.OnNewValue(difference, 1);

            return true;
        }
        return false;
    }
    public bool OnPopulationValueChanged()
    {
        if(lastPopulationValue != population)
        {
            int difference = population - lastPopulationValue;
            lastPopulationValue = population;
            
            ValueChangedDisplay.Instance.OnNewValue(difference, 2);
            return true;
        }
        return false;
    }
    #endregion

    #region Get costs of rooms/evolutions and check if  player has enough
    public bool EnoughResources(int[] cost)
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
    public int[] GetRoomCost(int roomIndex)
    {
        switch (roomIndex)
        {
            case 0:
                return room_corridor_cost;
            case 1:
                return room_salvage_cost;
            case 2:
                return room_war_cost;
            case 3:
                return room_harvester_cost;
            case 4:
                return room_queen_cost;
            default:
                return room_corridor_cost;
        }
         

    }
    public bool CanBuild(HiveCell.RoomContext room)
    {
        // define cost of each buildning 
        if (room == HiveCell.RoomContext.corridor)
        {
            return EnoughResources(room_corridor_cost);
        }
        if (room == HiveCell.RoomContext.harvester)
        {
            return EnoughResources(room_harvester_cost);
        }
        if (room == HiveCell.RoomContext.salvage)
        {
            return EnoughResources(room_salvage_cost);
        }
        if (room == HiveCell.RoomContext.war)
        {
            return EnoughResources(room_war_cost);
        }
        return false;
    }
    public int[] GetUpgradeCost(CoreBug.BugEvolution bugEvolution)
    {
        switch (bugEvolution)
        {
            case CoreBug.BugEvolution.drone:
                return evolve_drone;
            case CoreBug.BugEvolution.super_drone:
                return evolve_super_drone;
            case CoreBug.BugEvolution.warrior:
                return evolve_warrior;
            case CoreBug.BugEvolution.claw:
                return evolve_claw;
            case CoreBug.BugEvolution.range:
                return evolve_spike;
            case CoreBug.BugEvolution.cc_bug:
                return evolve_slow;
            default:
                return evolve_drone;
        }

    } 
    public bool CanAffordEvolution(CoreBug.BugEvolution bugEvolution)
    {
        return EnoughResources(GetUpgradeCost(bugEvolution));
    }
    #endregion

    public void ResetGame()
    {
        food = 20;
        wood = 20;
        population = 2;
        day_count = 0;

        // we need to rest AI 

    }
    public void OnGameEnd()
    { 
    
    }

    public void OnGameRestart()
    {

    }

    #region On Room  Build / Destroy
    public void OnRooomBuild(HiveCell.RoomContext room)
    {
        if (room == HiveCell.RoomContext.corridor)
        {
            SpendResources(room_corridor_cost);
        }
        if (room == HiveCell.RoomContext.harvester)
        {
            SpendResources(room_harvester_cost);
        }
        if (room == HiveCell.RoomContext.salvage)
        {
            SpendResources(room_salvage_cost);
        }
        if (room == HiveCell.RoomContext.war)
        {
            SpendResources(room_war_cost);
        }
        if(room == HiveCell.RoomContext.queen)
        {
            SpendResources(room_queen_cost);
        }
    }
    // bring back some resources
    public void OnRoomDestroyed(HiveCell.RoomContext room)
    {
        // define cost of each buildning 
        if (room == HiveCell.RoomContext.corridor)
        {
            SellBuilding(room_corridor_cost);
        }
        if (room == HiveCell.RoomContext.harvester)
        {
            SellBuilding(room_harvester_cost);
        }
        if (room == HiveCell.RoomContext.salvage)
        {
            SellBuilding(room_salvage_cost);
        }
        if (room == HiveCell.RoomContext.war)
        {
           SellBuilding(room_war_cost);
        }
    }
    #endregion

    #region Events
    // Events 
    // ------------------------------

    private FMOD.Studio.EventInstance DayAmbience;

    private FMOD.Studio.EventInstance NightAmbience;

    private FMOD.Studio.EventInstance DayMusic;

    private FMOD.Studio.EventInstance NightMusic;

    public void OnDayStart() 
    {
        DayAmbience = FMODUnity.RuntimeManager.CreateInstance("event:/Ambience/Ambience_Day");
        DayAmbience.start();
        NightAmbience.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        NightAmbience.release();


        DayMusic = FMODUnity.RuntimeManager.CreateInstance("event:/Music/Day_Music");
        DayMusic.start();
        NightMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        NightMusic.release();

        day_count++;
    }
    public void OnNightStart()
    {
        NightAmbience = FMODUnity.RuntimeManager.CreateInstance("event:/Ambience/Ambience_Night");
        NightAmbience.start();
        DayAmbience.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        DayAmbience.release();


        NightMusic = FMODUnity.RuntimeManager.CreateInstance("event:/Music/Night_Music");
        NightMusic.start();
        DayMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        DayMusic.release();
    }
    public void OnAttackStart()
    { 
    
    }
    public void SetResources(int food, int wood)
    {
        this.food = food;
        this.wood = wood;
    }

    public void OnConsumeFood(int food = 1)
    {
        this.food -= food;
    }
    public void OnBringResources()
    {
        food += bug_return_resources[0];
        wood += bug_return_resources[1];
    }
    public void OnBringResources(WorldMapCell.Cell_type cell_type)
    {
        if (cell_type == WorldMapCell.Cell_type.food)
            food += bug_return_resources[0];
        if (cell_type == WorldMapCell.Cell_type.wood)
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
    #endregion

    #region Methods
    // methods 
    // ------------------------------

    public int[] GetResources()
    {
        int[] resources = { food, wood };
        return resources;
    }
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
        OnFoodValueChanged();
        OnWoodValueChanged();
        OnPopulationValueChanged();
    }
    #endregion

    #region Protected
    // Protected
    // ------------------------------

    float _food_t = 0;
    float food_conusme_thick = 5;

    public void StopFoodTick()
    {

    }

    public void SetFoodTick(int food_tick)
    {
        food_conusme_thick = food_tick;
    }

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

        OnConsumeFood(population);
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




        //player's inits
        PlayerPref.Instance.IncreaseGamesPlayed();
        PlayerPref.Instance.UpdatePlayerSoundSettings();
        PlayerPref.Instance.ApplySavedQualitySettings();

        //set sound 
        foreach(VolumeSlider volume in volumeSliders)
        {
            volume.InitializeSound();
        }
    }
    #endregion
}
