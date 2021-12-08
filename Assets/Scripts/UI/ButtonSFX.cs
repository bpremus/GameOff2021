using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonSFX : MonoBehaviour
{

    public void onClick()
    {

        FMODUnity.RuntimeManager.PlayOneShotAttached("event:/UI/Button_Press_Default", gameObject);

    }

    public void onPointerEnter()

    {


    }
}
