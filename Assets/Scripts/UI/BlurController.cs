using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
public class BlurController : MonoBehaviour
{
    private Volume volume;
    private void Start()
    {
        volume = GetComponent<Volume>();
        DisableBlur();
    }
    public void DisableBlur()
    {
        if(volume)
            volume.enabled = false;
        
    }
    public void EnableBlur()
    {
        if (volume)
            volume.enabled = true;
    }
}
