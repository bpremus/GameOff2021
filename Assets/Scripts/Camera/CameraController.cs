using UnityEngine;

public class CameraController : MonoBehaviour
{
    //
    // Use SetTarget() and ResetTarget() to make camera follow passed Transform
    // Target is reset when player moves camera or uses stopFollowingKey
    //
    #region Variables
    private Transform m_Transform; //camera tranform
    private PopupController popupController;


    [Header("Main Settings")]
    public bool useScreenEdgeInput = false;
    public bool useKeyboardInput = true;
    public bool useDragInput = true;
    public bool useKeyboardZooming = true;
    public bool useScrollwheelZooming = true;
    public bool useRightMouseButtonToDrag = false;
    

    #region Movement
    [Header("Movement")]
    public float keyboardMovementSpeed = 25f; //speed with keyboard movement
    public float screenEdgeMovementSpeed = 25f; //spee with screen edge movement
    public float followingSpeed = 15f; //speed when following a target
    public float focusingSpeed = 40f; //speed when setting a focus on specific object (for objectives)
    public float dragMovementSpeed = 35f; // speed when drag and click

    #endregion

    #region Height
    [Header("Height")]
    public LayerMask groundMask = -1; //layermask of ground or other objects that affect height

    public float maxHeight = 40f; //max height
    public float minHeight = 12f; //min height
    public float keyboardZoomingSensitivity = 2f;
    public float scrollWheelZoomingSensitivity = 50f;

    private float zoomPos = 1; 

    #endregion

    #region MapLimits
    [Header("Limits")]
    public bool limitEnabled = true;
    public float leftlimitX = 5f; //x limit of map
    public float rightlimitX = 0f; //x limit of map
    public float uplimitY = 10f; //y limit of map
    public float downlimitY = 0f; //y limit of map
    public float limitZ = 10f; //z limit

    #endregion

    #region Targeting
    [Header("Targeting")]
    public Transform targetFollow;
    public Vector3 targetOffset;

    public bool FollowingTarget
    {
        get
        {
            return targetFollow != null;
        }
    }

    #endregion
    #region Focusing
    [Header("Focusing camera")]
    public Transform focusTarget;
    public bool FocusingTarget
    {
        get
        {
            return focusTarget != null;
        }
    }
    #endregion
    #region Input
    [Header("Inputs")]
    public float screenEdgeBorder = 25f;
    public string horizontalAxis = "Horizontal";
    public string verticalAxis = "Vertical";
    public KeyCode zoomOutKey = KeyCode.E;
    public KeyCode zoomInKey = KeyCode.Q;
    public KeyCode stopFollowingKey = KeyCode.Escape;
    public string zoomingAxis = "Mouse ScrollWheel";

    private bool isDragging = false;
    public void SetDraggingState(bool state) { isDragging = state; }
    private Vector2 KeyboardInput
    {
        // get { return useKeyboardInput ? new Vector2(Input.GetAxis(horizontalAxis), Input.GetAxis(verticalAxis)) : Vector2.zero; }
        get {

            int _x = 0;
            int _y = 0;
            if (Input.GetKey(KeyCode.W)) _y = 1;
            if (Input.GetKey(KeyCode.S)) _y = -1;
            if (Input.GetKey(KeyCode.A)) _x = 1;
            if (Input.GetKey(KeyCode.D)) _y = -1;
            return useKeyboardInput ? new Vector2(_x,_y) : Vector2.zero;
        }
    }

    private Vector2 MouseInput
    {
        get { return Input.mousePosition; }
    }

    private float ScrollWheel
    {
        get { return Input.GetAxis(zoomingAxis); }
    }

    private int ZoomDirection
    {
        get
        {
            bool zoomIn = Input.GetKey(zoomOutKey);
            bool zoomOut = Input.GetKey(zoomInKey);
            if (zoomIn && zoomOut)
                return 0;
            else if (!zoomIn && zoomOut)
                return 1;
            else if (zoomIn && !zoomOut)
                return -1;
            else
                return 0;
        }
    }


    #endregion
    #endregion
    #region Monobehaviours

    private void Start()
    {
        m_Transform = transform;
        popupController = FindObjectOfType<PopupController>();
    }

    private void Update()
    {
      CameraUpdate();
        float y = transform.position.y;

        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Camera_Y", y);
    }

    #endregion

    #region Public methods
    public void SetTarget(Transform target)
    {
        targetFollow = target;
        Debug.Log("target assigned");
    }

    public void SetFocus(HiveCell hc)
    {
        Debug.Log("Camera focusing on:  " + hc.name);
        zoomPos = 0.5f;
        focusTarget = hc.transform;

    }
    public bool IsDragging()
    {
        return isDragging;
    }
    public void ResetTarget()
    {
        targetFollow = null;
        Debug.Log("target reset");
    }
    public float GetZoomLevel()
    {
        return zoomPos;
    }
    #endregion

    #region Private methods
    private void CameraUpdate()
    {
        if (FollowingTarget)
        {
            if (Input.GetKeyDown(stopFollowingKey)) UIController.instance.SetDefaultState();
            else
              FollowTarget();
        }   
        else if (FocusingTarget)
        {
            if (Input.GetKeyDown(stopFollowingKey)) { UIController.instance.SetDefaultState();focusTarget = null; }
            SetFocus();
        }
        else
            Move();

        HeightCalculation();
        LimitPosition();
    }

    private void Move()
    {
     
        if(popupController != null)
        {
            if (popupController.isPopupActive()) return;
        }

        if (useKeyboardInput)
        {
            Vector3 desiredMove = new Vector3(Input.GetAxis(horizontalAxis), Input.GetAxis(verticalAxis), 0);

            desiredMove *= keyboardMovementSpeed;
            desiredMove *= Time.deltaTime;
            desiredMove = Quaternion.Euler(new Vector3(0f, transform.eulerAngles.y, 0f)) * desiredMove;
            desiredMove = m_Transform.InverseTransformDirection(desiredMove);

            m_Transform.Translate(desiredMove, Space.Self);
        }

        if (useScreenEdgeInput)
        {
            Vector3 desiredMove = new Vector3();

            Rect leftRect = new Rect(0, 0, screenEdgeBorder, Screen.height);
            Rect rightRect = new Rect(Screen.width - screenEdgeBorder, 0, screenEdgeBorder, Screen.height);
            Rect upRect = new Rect(0, Screen.height - screenEdgeBorder, Screen.width, screenEdgeBorder);
            Rect downRect = new Rect(0, 0, Screen.width, screenEdgeBorder);

            desiredMove.x = leftRect.Contains(MouseInput) ? -1 : rightRect.Contains(MouseInput) ? 1 : 0;
            desiredMove.y = upRect.Contains(MouseInput) ? 1 : downRect.Contains(MouseInput) ? -1 : 0;

            desiredMove *= screenEdgeMovementSpeed;
            desiredMove *= Time.deltaTime;
            desiredMove = Quaternion.Euler(new Vector3(0f, transform.eulerAngles.y, 0f)) * desiredMove;
            desiredMove = m_Transform.InverseTransformDirection(desiredMove);

            m_Transform.Translate(desiredMove, Space.Self);
        }
        if (useDragInput)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            LayerMask selection_mask = 0;
            //selection_mask = 1 << 6 | 1 << 7;
            bool isOverUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
            if (isOverUI)
            {
              
            }
            if (Physics.Raycast(ray, out hit) && !isOverUI)
            {

                if (useRightMouseButtonToDrag)
                {
                    if (Input.GetMouseButton(1)) isDragging = true;
                    else isDragging = false;
                }
                else
                {
                    if (Input.GetMouseButton(0)) isDragging = true;
                    else isDragging = false;
                }
            }

            if (isDragging)
            {
                Vector3 desiredMove = new Vector3(-Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"), 0f);
                desiredMove *= dragMovementSpeed;
                desiredMove *= Time.deltaTime;
                m_Transform.Translate(desiredMove, Space.Self);
            }
        }
    }

    private void HeightCalculation()
    {
        if (useScrollwheelZooming)
            zoomPos -= ScrollWheel * Time.deltaTime * scrollWheelZoomingSensitivity;
        if (useKeyboardZooming)
            zoomPos -= ZoomDirection * Time.deltaTime * keyboardZoomingSensitivity;

        zoomPos = Mathf.Clamp01(zoomPos);
        float targetHeight = Mathf.Lerp(minHeight, maxHeight, zoomPos);
        Camera.main.orthographicSize = targetHeight;
    }


    private void FollowTarget()
    {
        Vector3 targetPos = new Vector3(targetFollow.position.x, targetFollow.position.y, m_Transform.position.z) + targetOffset;
        m_Transform.position = Vector3.MoveTowards(m_Transform.position, targetPos, Time.deltaTime * followingSpeed);
    }
    private void SetFocus()
    {
        Vector3 targetPos = new Vector3(focusTarget.position.x, focusTarget.position.y, m_Transform.position.z) + targetOffset;
        m_Transform.position = Vector3.MoveTowards(m_Transform.position, targetPos, Time.deltaTime * followingSpeed);

        if (Vector3.Distance(m_Transform.position, targetPos) < 0.2f) focusTarget = null;
    }
    private void LimitPosition()
    {
        if (!limitEnabled)
            return;

        m_Transform.position = new Vector3(Mathf.Clamp(m_Transform.position.x, leftlimitX, rightlimitX), Mathf.Clamp(m_Transform.position.y, downlimitY, uplimitY), limitZ);
    }

    #endregion

}
