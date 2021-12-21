using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{

    public static CameraShake instance;


    [SerializeField] private Vector2 cameraShake;
    Transform cameraT;
    Vector3 initialPos;

    private float xrand;
    private float yrand;
    private float xTime;
    private float yTime;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(this);
        }
    }
    private void Start()
    {
        cameraT = Camera.main.transform;
        initialPos = cameraT.position;
    }
    [ContextMenu("Manual camera shake")]
    public void ShakeCamera()
    {
        initialPos = cameraT.position;
        xrand = UnityEngine.Random.Range(-cameraShake.x, cameraShake.x); 
        yrand = UnityEngine.Random.Range(-cameraShake.y, cameraShake.y);
        xTime = 0.01f;
        yTime = 0.04f;
        HorizontalCameraShake();
    }
    [ContextMenu("Manual huge camera shake")]
    public void HugeShakeCamera()
    {
        initialPos = cameraT.position;
        xrand = -0.35f;
        yrand = 0.25f;
        xTime = 0.03f;
        yTime = 0.08f;
        HorizontalCameraShake();
    }
    private void HorizontalCameraShake()
    {
        Debug.Log("xrand: " + xrand);
        LeanTween.moveX(cameraT.gameObject, xrand, xTime).setOnComplete(VerticalCameraShake);
    }
    private void VerticalCameraShake()
    {
        Debug.Log("yrand: " + yrand);
        LeanTween.moveY(cameraT.gameObject, yrand, yTime).setOnComplete(DefaultCamPos);
    }
    private void DefaultCamPos()
    {
        LeanTween.move(cameraT.gameObject, initialPos, 0.1f);
    }

}