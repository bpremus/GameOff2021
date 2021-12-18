using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SettingsGraphicChange : MonoBehaviour
{
    [SerializeField] private GameObject[] qualityButtons;
    [SerializeField] private Color normalColor;
    [SerializeField] private Color activeColor;

   [SerializeField] private controlsSettings ControlsSettings;
    private void Start()
    { 
        OnUpdate();
    }
    public void SetDefaultSettings()
    {
        PlayerPref.Instance.SetDefaultSettings();
        Camera.main.GetComponent<CameraController>().UpdateCamPreferences();
        OnUpdate();
    }
    private void OnUpdate()
    {
        foreach(GameObject obj in qualityButtons)
        {
               obj.GetComponentInChildren<TextMeshProUGUI>().color = normalColor;
        }
        qualityButtons[currentPrefSavedQuality()].GetComponentInChildren<TextMeshProUGUI>().color = activeColor;
    }
    public int currentPrefSavedQuality()
    {
      return PlayerPref.Instance.GetQualitySettings();
    }
    public int currentQuality()
    {
        return UnityEngine.QualitySettings.GetQualityLevel();
    }
    public void SetHighest()
    {
        UnityEngine.QualitySettings.SetQualityLevel(3);
        PlayerPref.Instance.SaveQualitySettings(3);
        OnUpdate();
    }
    public void SetMedium()
    {
        UnityEngine.QualitySettings.SetQualityLevel(2);
        PlayerPref.Instance.SaveQualitySettings(2);
        OnUpdate();
    }
    public void SetLow()
    {
        UnityEngine.QualitySettings.SetQualityLevel(1);
        PlayerPref.Instance.SaveQualitySettings(1);
        OnUpdate();
    }
    public void SetPotato()
    {
        UnityEngine.QualitySettings.SetQualityLevel(0);
        PlayerPref.Instance.SaveQualitySettings(0);
        OnUpdate();
    }
}
