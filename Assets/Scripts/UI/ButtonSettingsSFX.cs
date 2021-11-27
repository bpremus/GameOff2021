using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSettingsSFX : MonoBehaviour
{
    public void onCLick()
    {
        AkSoundEngine.PostEvent("Play_Button_Settings", gameObject);
    }
}
