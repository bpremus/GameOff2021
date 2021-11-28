using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UITopTimer : MonoBehaviour
{
    [SerializeField]
    Scrollbar scrollbar;

    [SerializeField]
    TextMeshProUGUI DayCounter;

    [SerializeField]
    Image day_night_image;

    [SerializeField]
    Sprite[] day_night_sprites;

    // Start is called before the first frame update
    void Start()
    {
        DayCounter = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        float p = GameController.Instance.GetTimePercent();
        scrollbar.value = p / 100;

        if (GameController.Instance.ISDayCycle())
        {
            day_night_image.sprite = day_night_sprites[0];
        }
        else
        {
            day_night_image.sprite = day_night_sprites[1];
        }

        DayCounter.text = "Day "+GameController.Instance.GetDayS();
    }
}
