using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class GameController : MonoBehaviour
{
    // Singleton class

    // Stats 
    // -----------------------------
    [SerializeField]
    int food = 20;
    int population = 2;

    // day and night cylces 
    [SerializeField]
    float day_night_duration = 1;
    [SerializeField]
    bool isDay = true;
    [SerializeField]
    float dayDuration = 0;
    

    // Events 
    // ------------------------------

    public void OnDayStart() 
    {

    }

    public void OnNightStart()
    {

    }

    public void OnAttackStart()
    { 
    
    }

    public void OnConsumeFood()
    {
        food -= 1;
    }
    public void OnBringFood()
    {
        food += 20;
    }

    public void OnNewBug()
    {
        population += 1;
    }
    public void OnBugDied()
    {
        population -= 1;
    }

    // methods 
    // ------------------------------
    public int GetFood()
    {
        return food;
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

}
