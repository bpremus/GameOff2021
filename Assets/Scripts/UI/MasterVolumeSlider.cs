using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterVolumeSlider : MonoBehaviour
{

    public void MasterVolume (float volume)
    {
        AkSoundEngine.SetRTPCValue("Master_Volume", volume);
    }
}
