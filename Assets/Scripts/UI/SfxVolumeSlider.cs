using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SfxVolumeSlider : MonoBehaviour
{

    private Slider slider;
    private void Awake()
    {
        slider = GetComponent<Slider>();
        SetSavedLevel();
    }
    public float GetCurentLevel()
    {
       return  PlayerPref.Instance.GetSFXVolume();
    }
    public void SFXVolume(float volume)
    {
  
        PlayerPref.Instance.SaveSFXVolume(volume);
    }
    public void ResetAll()
    {
        PlayerPref.Instance.ResetPlayerSettings();
    }



    public void SetSavedLevel()
    {
        if (slider)
        {
            slider.value = GetCurentLevel();
        }
        SFXVolume(GetCurentLevel());
    }
    private void OnEnable()
    {
        SetSavedLevel();
    }
}
