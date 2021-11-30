using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPref : MonoBehaviour
{
    [Header("Audio settings")]
    [Range(0,100)]
    [SerializeField] private float defaultMasterSoundLevel = 80f;
    [Range(0, 100)]
    [SerializeField] private float defaultMusicSoundLeel = 80f;
    [Range(0, 100)]
    [SerializeField] private float defaultSFXSoundLevel = 80f;


    [SerializeField]  private MasterVolumeSlider masterVolumeSlider;
    [SerializeField] private MusicVolumeSlider musicVolumeSlider;
    [SerializeField] private SfxVolumeSlider sfxVolumeSlider;
    private static PlayerPref _instance;

    public static PlayerPref Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {

        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;


        UpdatePlayerSoundSettings();
    }

    [ContextMenu("Reset Sound Settings")]
    public void ResetPlayerSettings()
    {
        PlayerPrefs.DeleteKey("mainVolume");
        PlayerPrefs.DeleteKey("musicVolume");
        PlayerPrefs.DeleteKey("sfxVolume");
        Debug.Log("Deleted player Sound Settings");
        UpdatePlayerSoundSettings();
    }

    public void UpdatePlayerSoundSettings()
    {
        if (!masterVolumeSlider || !musicVolumeSlider || !sfxVolumeSlider) { Debug.LogError("ASSIGN VOLUME SLIDERS IN PLAYERPREFS!"); return; }

        CheckIfDefaultVolumeSettings();
        ApplySavedVolumesToSlider();
    }

    private void ApplySavedVolumesToSlider()
    {
        masterVolumeSlider.SetSavedLevel();
        musicVolumeSlider.SetSavedLevel();
        sfxVolumeSlider.SetSavedLevel();
    }
    private void CheckIfDefaultVolumeSettings()
    {
        if (!PlayerPrefs.HasKey("mainVolume")) SaveMainVolume(defaultMasterSoundLevel);
        if (!PlayerPrefs.HasKey("musicVolume")) SaveMusicVolume(defaultMusicSoundLeel);
        if (!PlayerPrefs.HasKey("sfxVolume")) SaveSFXVolume(defaultSFXSoundLevel);
    }
    #region Volume settings - Setters and Getters

    public void SaveMainVolume(float value) 
    {
        if (value > 100) value = 100;
        else if (value < 0) value = 0;

        PlayerPrefs.SetFloat("mainVolume", value);
    }
    public float GetMainVolume() { return PlayerPrefs.GetFloat("mainVolume"); }

    public void SaveMusicVolume(float value) 
    {
        if (value > 100) value = 100;
        else if (value < 0) value = 0;

        PlayerPrefs.SetFloat("musicVolume", value); 
    }
    public float GetMusicVolume() { return PlayerPrefs.GetFloat("musicVolume"); }

    public void SaveSFXVolume(float value) 
    {
        Debug.Log("SFX changed!");
        if (value > 100) value = 100;
        else if (value < 0) value = 0;
        
        PlayerPrefs.SetFloat("sfxVolume", value); 
    }
    public float GetSFXVolume() { return PlayerPrefs.GetFloat("sfxVolume");}


    #endregion


    #region Preferences

    public void SetRightDrag(bool value) 
    {
        string str;
        if (value) str = "true";
        else str = "false";
        PlayerPrefs.SetString("rightDrag", str);
           
    }
    public string GetRightDrag()
    {
        return PlayerPrefs.GetString("rightDrag");
    }



    public void SetScreenEdgeMovement(bool value)
    {
        string str;
        if (value) str = "true";
        else str = "false";
        PlayerPrefs.SetString("edgeMovement", str);
    }
    public string GetScreenEdgeMovement()
    {
        return PlayerPrefs.GetString("edgeMovment");
    }


    #endregion
}
