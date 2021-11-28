using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicVolumeSlider : MonoBehaviour
{
    public void MusicVolume (float volume)
    {
        AkSoundEngine.SetRTPCValue("Music_Volume", volume);
    }
}
