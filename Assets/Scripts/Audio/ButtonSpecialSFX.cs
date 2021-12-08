using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSpecialSFX : MonoBehaviour
{
    public void onCLick()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached("event:/UI/Button_Press_Special", gameObject);
    }
}
