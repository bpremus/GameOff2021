using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxVolumeSlider : MonoBehaviour
{
    public void SFXVolume(float volume)
    {
        AkSoundEngine.SetRTPCValue("SFX_Volume", volume);
    }
}
