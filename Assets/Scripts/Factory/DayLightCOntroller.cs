using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayLightCOntroller : MonoBehaviour
{
    // Start is called before the first frame update
    GameController gc;
    float start_intensity = 0;
    float current_intesity = 0;
    Light light;

    [SerializeField]
    SpriteRenderer bg_image;

    void Start()
    {
        light = GetComponent<Light>();
        gc = GameController.Instance;
        start_intensity = light.intensity;
        current_intesity = start_intensity;
    }

    // Update is called once per frame
    void Update()
    {
        if (gc.ISDayCycle() == false)
        {
            current_intesity -= Time.deltaTime * 2f;
            if (current_intesity < 2)
                current_intesity = 2;

            if (bg_image) // test 
            {
                Color c = bg_image.color;
                float clr = c.r - Time.deltaTime * 2;
                if (clr < 0) clr = 0;
                c.r = clr;
                c.g = clr;
                c.b = clr;
                bg_image.color = c;
            }

        }
        else
        {
            current_intesity += Time.deltaTime * 2f;
            if (current_intesity > start_intensity)
                current_intesity = start_intensity;


            if (bg_image) // test 
            {
                Color c = bg_image.color;
                float clr = c.r + Time.deltaTime * 2;
                if (clr > 1) clr = 1;
                c.r = clr;
                c.g = clr;
                c.b = clr;
                bg_image.color = c;
            }

        }

        light.intensity = current_intesity;

    }
}
