using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDbgMove : MonoBehaviour
{
    public int cameraDragSpeed = 50;
    public float scroll_scale = 1;

    public void Awake()
    {
        scroll_scale = Camera.main.orthographicSize;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            float speed = cameraDragSpeed * Time.deltaTime;
            Vector3 pos = new Vector3(-Input.GetAxis("Mouse X") * speed, Input.GetAxis("Mouse Y") * speed, 0);
            Camera.main.transform.position -= pos;
        }

        scroll_scale -= Input.mouseScrollDelta.y * 1;
        if (scroll_scale < 3) scroll_scale = 3;
        if (scroll_scale > 7) scroll_scale = 7;

        Camera.main.orthographicSize = scroll_scale;

        Vector3 pos2 = transform.position;
        if (pos2.y > 1.5) pos2.y = 1.5f;
        if (pos2.x > 8) pos2.x = 7f;
        if (pos2.x < -8) pos2.x = -7f;
        if (pos2.y < -10) pos2.y = -10f;
        
        transform.position = pos2;


    }
}
