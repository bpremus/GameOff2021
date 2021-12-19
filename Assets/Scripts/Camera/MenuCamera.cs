using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamera : MonoBehaviour
{
    // changed to singleton 
    private static MenuCamera _instance;
    public static MenuCamera Instance
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
    }

    [SerializeField]
    Transform menu_camera_pos;

    [SerializeField]
    Transform game_camera_pos;

    [SerializeField]
    Transform game_hive;

    [SerializeField]
    UIController ui_controller;

    [SerializeField]
    GameObject[] cover_entities_cover;

    protected bool is_in_menu = false;

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

        StartGameZoomIn(); // auto zoom in on game start
    }

    public void GetCameraLastInGamePosition()
    {
        _camera_menu_position = _camera.transform.position;
        _camera_menu_rotation = _camera.transform.rotation;
        _camera_ortho_size    = _camera.orthographicSize;
    }

    // Update is called once per frame
    [Range(0,1)]
    public float move_index = 0;

    public float move_speed = 20;

    public void StartGameZoomIn()
    {
        move_index = 0;
        easy_in_multiply = 0.2f;
        is_in_menu = false;
    }

    public void CloseMenu()
    {
        easy_in_multiply = 1.5f;
        is_in_menu = false;
    }

    public void OpenMenu()
    {
        ui_controller.EnableBlur();
        easy_in_multiply = 1f;
        is_in_menu = true;
    }


    protected float easy_in_multiply = 3; // speed of zoom in, on first game start is slow like 0.1f
    void LateUpdate()
    {
        if (is_in_menu == false && move_index < 1)
        {
            move_index += move_speed * easy_in_multiply *  Time.deltaTime;
            if (move_index >= 1) move_index = 1;
        }
        if (is_in_menu == true && move_index > 0)
        {
            move_index -= move_speed  * Time.deltaTime;
            if (move_index <= 0) move_index = 0;
        }

        MoveOnPath();
      //  Dbg_path();
    }

    public void Dbg_path()
    {

        Vector3 p1 = menu_camera_pos.transform.position;
        Vector3 p2 = menu_camera_pos.transform.position + menu_camera_pos.transform.forward * 30;
        Vector3 p3 = game_camera_pos.transform.position - game_camera_pos.transform.forward * 500;
        Vector3 p4 = game_camera_pos.transform.position;

        Vector3 last_pos = p1;
        int steps = 100;
        for (int i = 0; i < steps; i++)
        {
            float step = (float)i / steps;
            Vector3 new_pos = CalculateCubicBezierPoint(step, p1, p2, p3, p4);
            Debug.DrawLine(last_pos, new_pos, Color.white);
            last_pos = new_pos;
        }
        Debug.DrawLine(last_pos, _camera.transform.position, Color.green);


    }
    public void OnCameraInGame()
    {
        Debug.Log("in game");
        _camera.orthographicSize    = _camera_ortho_size;
        //_camera.transform.position  = _camera_menu_position;
        _camera.transform.rotation  = _camera_menu_rotation;

    }

    public void OnCameraInmenu()
    {
        Debug.Log("in menu");
    }

    bool is_moving = true;
    void MoveOnPath()
    {

        // this is fairly simplified
        // correct way it so to have a view matrix

        if (_cameraController)
        {
            if (move_index == 1) // we are in final game position
            {
                // enable game movement 
                _cameraController.is_in_menu = false;

                // hide all scene objects we do not need for gameplay
                for (int i = 0; i < cover_entities_cover.Length; i++)
                    cover_entities_cover[i].SetActive(false);

                // remove blur 
                ui_controller.DisableBlur();

                if (is_moving == true)
                {
                    easy_in_multiply = 1; // speed of zoom in
                    OnCameraInGame();
                    is_moving = false;
                }
                return; // nothing to do 
            }
            else
            {
                _cameraController.is_in_menu = true;
                if (move_index == 0)
                {
                    if (is_moving == true)
                    {
                        OnCameraInmenu();
                        is_moving = false;
                    }
                }
            }
        }
                

        for (int i = 0; i < cover_entities_cover.Length; i++)
             cover_entities_cover[i].SetActive(true);

       

        Vector3 p1 = menu_camera_pos.transform.position;
        Vector3 p2 = menu_camera_pos.transform.position + menu_camera_pos.transform.forward * 30;
        Vector3 p3 = game_camera_pos.transform.position - game_camera_pos.transform.forward * 500;
        Vector3 p4 = game_camera_pos.transform.position;

        Vector3 new_pos = CalculateCubicBezierPoint(move_index, p1, p2, p3, p4);
        Vector3 look_dir = game_hive.transform.position - _camera.transform.position;

        _camera.transform.position = new_pos;
        _camera.transform.rotation = Quaternion.Slerp(_camera.transform.rotation, Quaternion.LookRotation(look_dir), 20f * Time.deltaTime);

        if (move_index > 0.65f && is_in_menu == false)
        {
           // if (_camera.fieldOfView > 72.5f)
           // {
           //     _camera.fieldOfView -= Time.deltaTime * 20;
           // }
           if (move_index >= 0.95)
           {
                // nothing for now
                cover_entities_cover[1].SetActive(false);
            }      
        }
        else
        {
            // if (_camera.fieldOfView < 72.5f)
            // {
            //     _camera.fieldOfView += Time.deltaTime * 50;
            //     _camera.orthographic = false;
            // }
            _camera.orthographic = false;
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
