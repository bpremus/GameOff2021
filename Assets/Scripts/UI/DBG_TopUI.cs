using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DBG_TopUI : MonoBehaviour
{
    // changed to singleton 
    private static DBG_TopUI _instance;
    public static DBG_TopUI Instance
    {
        get { return _instance; }
    }

    public int food = 20;
    public int population = 1;

    public void ConsumeFood()
    {
        food -= 1;
    }
    public void BringFood()
    {
        food += 20;
    }

    public void NewBug()
    {
        population += 1;
    }
    public void BugDied()
    {
        population -= 1;
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

    [SerializeField]
    Text food_Text;
    [SerializeField]
    Text population_Text;
    [SerializeField]
    Text time_Text;

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

    GameController gc;
    public void DayOrNight()
    {
        if (gc == null)
            gc = GameController.Instance;

        string str = "";

        str += " " + (100 - gc.GetTimePercent()) + "%";

        if (gc.ISDayCycle())
        {
            str += " (Day)";
        }
        else
        {
            str += " (Night)";
        }

       
        time_Text.text = str;
    }

    public void Update()
    {
        food_Text.text = food.ToString();
        population_Text.text = population.ToString();

        ConsumeFoodThick();
        DayOrNight();
    }

}
