using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonSFX : MonoBehaviour
{

    public void onClick()
    {

        AkSoundEngine.PostEvent("Play_Button_Click", gameObject);

    }

    public void onPointerEnter()

    {
        AkSoundEngine.PostEvent("Play_Button_Hover", gameObject);

    }
}
