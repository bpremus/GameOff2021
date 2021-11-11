using UnityEngine;

public class CameraController : MonoBehaviour
{
    //
    // Use SetTarget() and ResetTarget() to make camera follow passed Transform
    // Target is reset when player moves camera or uses stopFollowingKey
    //
    #region Variables
    private Transform m_Transform; //camera tranform

    #region Movement
    [Header("Movement")]
    public float keyboardMovementSpeed = 25f; //speed with keyboard movement
    public float screenEdgeMovementSpeed = 25f; //spee with screen edge movement
    public float followingSpeed = 15f; //speed when following a target

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
    public float limitX = 50f; //x limit of map
    public float limitY = 50f; //y limit of map
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

    #region Input
    [Header("Inputs")]
    public bool useScreenEdgeInput = true;
    public float screenEdgeBorder = 25f;

    public bool useKeyboardInput = true;
    public string horizontalAxis = "Horizontal";
    public string verticalAxis = "Vertical";


    public bool useKeyboardZooming = true;
    public KeyCode zoomInKey = KeyCode.E;
    public KeyCode zoomOutKey = KeyCode.Q;
    public KeyCode stopFollowingKey = KeyCode.Escape;
    public bool useScrollwheelZooming = true;
    public string zoomingAxis = "Mouse ScrollWheel";


    private Vector2 KeyboardInput
    {
        get { return useKeyboardInput ? new Vector2(Input.GetAxis(horizontalAxis), Input.GetAxis(verticalAxis)) : Vector2.zero; }
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
            bool zoomIn = Input.GetKey(zoomInKey);
            bool zoomOut = Input.GetKey(zoomOutKey);
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
    }

    private void Update()
    {
      CameraUpdate();
    }


    #endregion

    #region Public methods
    public void SetTarget(Transform target)
    {
        targetFollow = target;
    }


    public void ResetTarget()
    {
        targetFollow = null;
    }

    #endregion

    #region Private methods
    private void CameraUpdate()
    {
        if (FollowingTarget)
        {
            if (Input.GetKeyDown(stopFollowingKey) || KeyboardInput.x != 0 || KeyboardInput.y != 0) ResetTarget();
            else
              FollowTarget();
        }
          
        else
            Move();

        HeightCalculation();
        LimitPosition();

    }

    private void Move()
    {
        if (useKeyboardInput)
        {
            Vector3 desiredMove = new Vector3(KeyboardInput.x, KeyboardInput.y, 0);

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

    private void LimitPosition()
    {
        if (!limitEnabled)
            return;

        m_Transform.position = new Vector3(Mathf.Clamp(m_Transform.position.x, -limitX, limitX), Mathf.Clamp(m_Transform.position.y, -limitY, limitY), limitZ);
    }

    #endregion

}