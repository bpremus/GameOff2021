using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DBG_TopUI : MonoBehaviour
{
    // changed to singleton 
    private static DBG_TopUI _instance;
    public static DBG_TopUI Instance
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

    [SerializeField]
    TextMeshProUGUI food_Text;
    [SerializeField]
    TextMeshProUGUI wood_text;
    [SerializeField]
    TextMeshProUGUI population_Text;
    [SerializeField]
    TextMeshProUGUI time_Text;



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

       if(time_Text)
        time_Text.text = str;
    }

    public void Update()
    {
        food_Text.text = GameController.Instance.GetFood().ToString();
        population_Text.text = GameController.Instance.GetPopulation().ToString();
        wood_text.text = 0.ToString();
        DayOrNight();
    }

}
