using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controlsSettings : MonoBehaviour
{
    [SerializeField] private GameObject settingActive;
    CameraController settings;

    public enum controlTypes
    {
        rmbDrag,
        keyboardzoom,
        edgescroll
    }
    public controlTypes controlType;
    private void Start()
    {
       settings = Camera.main.GetComponent<CameraController>();
    }
    private void Awake()
    {
    }
    private void Update()
    {
        switch (controlType)
        {
            case controlTypes.rmbDrag:
                UpdateStateDrag();
                break;
            case controlTypes.keyboardzoom:
                UpdateStateZoom();
                break;
            case controlTypes.edgescroll:
                UpdateStateScrolling();
                break;
            default:
                break;
        }
    }
    private void UpdateStateDrag()
    {
        if (settings.isRMBDragOn()) settingActive.SetActive(true);
        else settingActive.SetActive(false);
    }
    private void UpdateStateZoom()
    {
        if (settings.isKeyboardZoomingOn()) settingActive.SetActive(true);
        else settingActive.SetActive(false);
    }
    private void UpdateStateScrolling()
    {

        if (settings.isEdgeScreenScrollingOn()) settingActive.SetActive(true);
        else settingActive.SetActive(false);
    }
}
