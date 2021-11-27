using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonExitSFX : MonoBehaviour
{

    public void onClick()
    {
        AkSoundEngine.PostEvent("Play_Button_Exit_Click", gameObject);
    }
}
