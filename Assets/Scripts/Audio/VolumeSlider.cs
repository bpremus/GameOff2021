using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{

    private FMOD.Studio.VCA vca;
    public string VcaPath;
    private void Start()
    {
        InitializeSound();
    }
    public void InitializeSound()
    {
        vca = FMODUnity.RuntimeManager.GetVCA(VcaPath);
        vca.setVolume(GetSavedVolume());
    }
    public void SetVolume(float volume)
    {
        vca.setVolume(volume);

        SaveVolume(volume);
    }
    public float GetSavedVolume()
    {
        float value = 0;
        if (VcaPath == "vca:/Master")
        {
            value = PlayerPref.Instance.GetMainVolume();
        }
        else if (VcaPath == "vca:/SFX")
        {
            value = PlayerPref.Instance.GetSFXVolume();
        }
        else if (VcaPath == "vca:/Music")
        {
            value = PlayerPref.Instance.GetMusicVolume();
        }
        else
        {
            Debug.LogError("VcaPath error on get");
        }
        GetComponent<Slider>().value = value;
        return value;
    }
    public void SaveVolume(float volume)
    {
       if(VcaPath == "vca:/Master")
        {
            PlayerPref.Instance.SaveMainVolume(volume);
        }
       else if(VcaPath== "vca:/SFX")
        {
            PlayerPref.Instance.SaveSFXVolume(volume);
        }
       else if(VcaPath == "vca:/Music")
        {
            PlayerPref.Instance.SaveMusicVolume(volume);
        }
        else
        {
            Debug.LogError("VcaPath error on set");
        }
    }
}
