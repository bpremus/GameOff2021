using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayLightCOntroller : MonoBehaviour
{
    // Start is called before the first frame update
    GameController gc;
    float start_intensity = 0;
    float current_intesity = 0;
    Light light;

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
        }
        else
        {
            current_intesity += Time.deltaTime * 2f;
            if (current_intesity > start_intensity)
                current_intesity = start_intensity;
        }

        light.intensity = current_intesity;

    }
}
