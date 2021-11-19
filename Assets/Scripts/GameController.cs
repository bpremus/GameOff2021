using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class GameController : MonoBehaviour
{
    // Singleton class

    // Stats 
    // -----------------------------
    int food = 0;
    int population = 0;

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

    // Events 
    // ------------------------------



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
    }

    // Protected
    // ------------------------------
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
