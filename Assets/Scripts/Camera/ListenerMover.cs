using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListenerMover : MonoBehaviour
{
     private CameraController cameraController;
    Vector3 startPos;
    private float maxZoomedOut = 0;
    private float maxZoomedIn = 9f;
    private void Awake()
    {
        cameraController = GetComponentInParent<CameraController>();
    }
    private void Start()
    {
        startPos = cameraController.transform.position;
        maxZoomedOut = startPos.z;
        maxZoomedIn = startPos.z - 9f;
    }

    private void Update()
    {
        float zoomLevel = cameraController.GetZoomLevel();
        float targetZ = Mathf.Lerp(maxZoomedIn, maxZoomedOut, zoomLevel);
        transform.position = new Vector3(cameraController.transform.position.x, cameraController.transform.position.y, targetZ);
    }
}
