using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPref : MonoBehaviour
{
    [Header("Audio settings")]
    [Range(0,1)]
    [SerializeField] private float defaultMasterSoundLevel = .7f;
    [Range(0, 1)]
    [SerializeField] private float defaultMusicSoundLeel = .7f;
    [Range(0, 1)]
    [SerializeField] private float defaultSFXSoundLevel = .7f;

    [SerializeField] private int gamesPlayed = 0;
    [SerializeField]  private VolumeSlider masterVolumeSlider;
    [SerializeField] private VolumeSlider musicVolumeSlider;
    [SerializeField] private VolumeSlider sfxVolumeSlider;
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

        gamesPlayed = GetGamesPlayed();
        UpdatePlayerSoundSettings();
    }
    [ContextMenu("Reset games played")]
    public void ResetGamesPlayed()
    {
        PlayerPrefs.DeleteKey("gamesPlayed");
        gamesPlayed = GetGamesPlayed();
    }
    [ContextMenu("Reset All Settings")]
    public void ResetPlayerSettings()
    {
        PlayerPrefs.DeleteKey("mainVolume");
        PlayerPrefs.DeleteKey("musicVolume");
        PlayerPrefs.DeleteKey("sfxVolume");
        PlayerPrefs.DeleteKey("rightDrag");
        PlayerPrefs.DeleteKey("qeZooming");
        PlayerPrefs.DeleteKey("edgeMovement");
        PlayerPrefs.DeleteKey("qualitySettings");
        Debug.Log("Deleted player Settings");
        UpdatePlayerSoundSettings();


    }
    public void IncreaseGamesPlayed()
    {
        gamesPlayed++;
        PlayerPrefs.SetInt("gamesPlayed", gamesPlayed);
    }
    public int GetGamesPlayed()
    {
        return PlayerPrefs.GetInt("gamesPlayed",0);
    }
    public void UpdatePlayerSoundSettings()
    {
        if (!masterVolumeSlider || !musicVolumeSlider || !sfxVolumeSlider) { Debug.LogError("ASSIGN VOLUME SLIDERS IN PLAYERPREFS!"); return; }

        CheckIfDefaultVolumeSettings();
        ResetVolumeSettingsSlider();
    }

    private void ResetVolumeSettingsSlider()
    {
        masterVolumeSlider.GetSavedVolume();
        musicVolumeSlider.GetSavedVolume();
        sfxVolumeSlider.GetSavedVolume();
    }
    private void CheckIfDefaultVolumeSettings()
    {
        if (!PlayerPrefs.HasKey("mainVolume")) SaveMainVolume(defaultMasterSoundLevel);
        if (!PlayerPrefs.HasKey("musicVolume")) SaveMusicVolume(defaultMusicSoundLeel);
        if (!PlayerPrefs.HasKey("sfxVolume")) SaveSFXVolume(defaultSFXSoundLevel);
    }

    public void SetDefaultSettings()
    {
        ResetPlayerSettings();
        SaveQualitySettings(3);
        ApplySavedQualitySettings();
        SetQEZooming(true);
        SetRightDrag(false);
        SetScreenEdgeMovement(false);


    }
    #region Volume settings - Setters and Getters

    public void SaveMainVolume(float value) 
    {
        PlayerPrefs.SetFloat("mainVolume", value);
    }
    public float GetMainVolume() { return PlayerPrefs.GetFloat("mainVolume"); }

    public void SaveMusicVolume(float value) 
    {
        PlayerPrefs.SetFloat("musicVolume", value); 
    }
    public float GetMusicVolume() { return PlayerPrefs.GetFloat("musicVolume"); }

    public void SaveSFXVolume(float value) 
    {
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
        return PlayerPrefs.GetString("rightDrag","false");
    }
    public void SetQEZooming(bool value)
    {
        string str;
        if (value) str = "true";
        else str = "false";
        PlayerPrefs.SetString("qeZooming", str);
    }
    public string GetQEZooming()
    {
        return PlayerPrefs.GetString("qeZooming","true");
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
        return PlayerPrefs.GetString("edgeMovment","false");
    }


    #endregion

    #region Quality Settings
    public void ApplySavedQualitySettings()
    {
        UnityEngine.QualitySettings.SetQualityLevel(GetQualitySettings());
    }
    public void SaveQualitySettings(int level)
    {
       PlayerPrefs.SetInt("qualitySettings", level);
    }
    public int GetQualitySettings()
    {
        return PlayerPrefs.GetInt("qualitySettings", 3);
    }

    #endregion
}
