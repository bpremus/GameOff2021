using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamera : MonoBehaviour
{
    [SerializeField]
    Transform menu_camera_pos;

    [SerializeField]
    Transform game_camera_pos;

    [SerializeField]
    Transform game_hive;

    [SerializeField]
    GameObject[] cover_entities_cover;

    public bool is_in_menu = false;

    Vector3 _camera_menu_position;
    Quaternion _camera_menu_rotation;
    float _camera_ortho_size;

    [SerializeField]
    Camera _camera;

    CameraController _cameraController;

    // Start is called before the first frame update
    void Start()
    {
        _cameraController = _camera.GetComponent<CameraController>();
        GetCameraLastInGamePosition();
    }

    public void GetCameraLastInGamePosition()
    {
        _camera_menu_position = _camera.transform.position;
        _camera_menu_rotation = _camera.transform.rotation;
        _camera_ortho_size = _camera.orthographicSize;
    }

    // Update is called once per frame
    [Range(0,1)]
    public float move_index = 0;

    public float move_speed = 20;
    void Update()
    {
        if (is_in_menu == false && move_index < 1)
        {
            move_index += move_speed * Time.deltaTime;
            if (move_index >= 1) move_index = 1;
        }
        if (is_in_menu == true && move_index > 0)
        {
            move_index -= move_speed * 3 * Time.deltaTime;
            if (move_index <= 0) move_index = 0;
        }

        MoveOnPath();
       // Dbg_path();
    }

    public void Dbg_path()
    {

        Vector3 p1 = menu_camera_pos.transform.position;
        Vector3 p2 = menu_camera_pos.transform.position + menu_camera_pos.transform.forward * 30;
        Vector3 p3 = game_camera_pos.transform.position - game_camera_pos.transform.forward * 500;
        Vector3 p4 = game_camera_pos.transform.position;

        Vector3 last_pos = p1;
        int steps = 10;
        for (int i = 0; i < steps; i++)
        {
            float step = (float)i / steps;
            Vector3 new_pos = CalculateCubicBezierPoint(step, p1, p2, p3, p4);
            Debug.DrawLine(last_pos, new_pos, Color.white);
            last_pos = new_pos;
        }

    }

    void MoveOnPath()
    {

        // this is fairly simplified
        // correct way it so to have a view matrix

        if (_cameraController)
        {
            if (move_index == 1)
            {
                _cameraController.is_in_menu = false;
                for (int i = 0; i < cover_entities_cover.Length; i++)
                    cover_entities_cover[i].SetActive(false);
                return; // nothing to do 
            }
            else
                _cameraController.is_in_menu = true;
        }

        for (int i = 0; i < cover_entities_cover.Length; i++)
             cover_entities_cover[i].SetActive(true);


        Vector3 p1 = menu_camera_pos.transform.position;
        Vector3 p2 = menu_camera_pos.transform.position + menu_camera_pos.transform.forward * 30;
        Vector3 p3 = game_camera_pos.transform.position - game_camera_pos.transform.forward * 500;
        Vector3 p4 = game_camera_pos.transform.position;

        Vector3 new_pos = CalculateCubicBezierPoint(move_index, p1, p2, p3, p4);
        _camera.transform.position = new_pos;

        Vector3 look_dir = game_hive.transform.position - _camera.transform.position;
        _camera.transform.rotation = Quaternion.LookRotation(look_dir);

        if (move_index > 0.65f)
        {
            if (_camera.fieldOfView > 10)
            {
                _camera.fieldOfView -= Time.deltaTime * 100;
            }
            if (move_index >= 0.90)
            {
                _camera.orthographic = true;
                _camera.orthographicSize = _camera_ortho_size;
                _camera.transform.position = _camera_menu_position;
                _camera.transform.rotation = _camera_menu_rotation;
            }

            cover_entities_cover[1].SetActive(false);
        }
        else
        {
            if (_camera.fieldOfView < 60)
            {
                _camera.fieldOfView += Time.deltaTime * 50;
                _camera.orthographic = false;
            }
        }
    }


    Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * p0;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += ttt * p3;

        return p;
    }

}
