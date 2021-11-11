using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform m_Transform; //camera tranform

    #region Movement

    public float keyboardMovementSpeed = 5f; //speed with keyboard movement
    public float screenEdgeMovementSpeed = 3f; //spee with screen edge movement
    public float followingSpeed = 5f; //speed when following a target

    #endregion

    #region Height

    public bool autoHeight = true;
    public LayerMask groundMask = -1; //layermask of ground or other objects that affect height

    public float maxHeight = 10f; //maximal height
    public float minHeight = 15f; //minimnal height
    public float heightDampening = 5f;
    public float keyboardZoomingSensitivity = 2f;
    public float scrollWheelZoomingSensitivity = 25f;

    private float zoomPos = 0; 

    #endregion

    #region MapLimits

    public bool limitMap = true;
    public float limitX = 50f; //x limit of map
    public float limitY = 50f; //y limit of map
    public float limitZ = 30f;

    #endregion

    #region Targeting

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

    public bool useScreenEdgeInput = true;
    public float screenEdgeBorder = 25f;

    public bool useKeyboardInput = true;
    public string horizontalAxis = "Horizontal";
    public string verticalAxis = "Vertical";


    public bool useKeyboardZooming = true;
    public KeyCode zoomInKey = KeyCode.E;
    public KeyCode zoomOutKey = KeyCode.Q;

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


    private void CameraUpdate()
    {
        if (FollowingTarget)
            FollowTarget();
        else
            Move();

        HeightCalculation();
        LimitPosition();

    }

    private void Move()
    {
        if (useKeyboardInput)
        {
            Vector3 desiredMove = new Vector3(KeyboardInput.x, 0, KeyboardInput.y);

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
        float distanceToGround = DistanceToGround();
        if (useScrollwheelZooming)
            zoomPos -= ScrollWheel * Time.deltaTime * scrollWheelZoomingSensitivity;
        if (useKeyboardZooming)
            zoomPos -= ZoomDirection * Time.deltaTime * keyboardZoomingSensitivity;

        zoomPos = Mathf.Clamp01(zoomPos);
       

        float targetHeight = Mathf.Lerp(minHeight, maxHeight, zoomPos);
        float difference = 0;

        if (distanceToGround != targetHeight)
            difference = targetHeight - distanceToGround;

     //   m_Transform.position = Vector3.Lerp(m_Transform.position,
       //    new Vector3(m_Transform.position.x, targetHeight + difference, m_Transform.position.z), Time.deltaTime * heightDampening);
        Camera.main.orthographicSize = targetHeight + difference;
    //    Camera.main.orthographicSize = zoomPos;
    }


    private void FollowTarget()
    {
        Vector3 targetPos = new Vector3(targetFollow.position.x, targetFollow.position.y, m_Transform.position.z) + targetOffset;
        m_Transform.position = Vector3.MoveTowards(m_Transform.position, targetPos, Time.deltaTime * followingSpeed);
    }

    private void LimitPosition()
    {
        if (!limitMap)
            return;

        m_Transform.position = new Vector3(Mathf.Clamp(m_Transform.position.x, -limitX, limitX), Mathf.Clamp(m_Transform.position.y, -limitY, limitY), limitZ);
    }
    private float DistanceToGround()
    {
        Ray ray = new Ray(m_Transform.position, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, groundMask.value))
            return (hit.point - m_Transform.position).magnitude;

        return 0f;
    }

    public void SetTarget(Transform target)
    {
        targetFollow = target;
    }


    public void ResetTarget()
    {
        targetFollow = null;
    }


}
