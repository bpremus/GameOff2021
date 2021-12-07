using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MasterVolumeSlider : MonoBehaviour
{
    private Slider slider;
    private void Awake()
    {
        slider = GetComponent<Slider>();
        SetSavedLevel();
    }
    public float GetCurentLevel()
    {
       return  PlayerPref.Instance.GetMainVolume();
    }
    public void MasterVolume (float volume)
    {
  
        PlayerPref.Instance.SaveMainVolume(volume);
    }
    public void SetSavedLevel()
    {
        if (slider)
        {
            slider.value = GetCurentLevel();
            MasterVolume(GetCurentLevel());
        }
        MasterVolume(GetCurentLevel());
    }

    private void OnEnable()
    {
        SetSavedLevel();
    }
}
