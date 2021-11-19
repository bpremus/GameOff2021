using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
public class BlurController : MonoBehaviour
{
    [SerializeField]
    private Volume volume;
    private void Start()
    {
        if(volume == null)
              volume = GetComponent<Volume>();
    }
    public void DisableBlur()
    {
        if (volume)
        {
            if(volume.isActiveAndEnabled)
                 volume.enabled = false;
        }
           
        
    }
    public void EnableBlur()
    {
        if (volume)
        {
            if (!volume.isActiveAndEnabled)
                volume.enabled = true;
        }
    }
}
