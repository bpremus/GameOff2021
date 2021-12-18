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
        vca = FMODUnity.RuntimeManager.GetVCA(VcaPath);
    }

    public void SetVolume(float volume)
    {
        vca.setVolume(volume);
    }

}
