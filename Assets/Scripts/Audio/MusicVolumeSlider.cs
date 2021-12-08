using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MusicVolumeSlider : MonoBehaviour
{

    private Slider slider;
    private void Awake()
    {
        slider = GetComponent<Slider>();
        SetSavedLevel();
    }
    public float  GetCurentLevel()
    {
       return PlayerPref.Instance.GetMusicVolume();
    }
    public void MusicVolume (float volume)
    {

        PlayerPref.Instance.SaveMusicVolume(volume);
    }


    public void SetSavedLevel()
    {
        if (slider)
        {
            slider.value = GetCurentLevel();
            
        }
        MusicVolume(GetCurentLevel());
    }
    private void OnEnable()
    {
        SetSavedLevel();
    }
}
