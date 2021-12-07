using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonExitSFX : MonoBehaviour
{

    public void onClick()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached("event:/UI/Button_Press_Exit", gameObject);
    }
}
