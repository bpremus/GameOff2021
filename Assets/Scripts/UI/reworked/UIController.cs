using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    //Use this class to call all the possible ui functions and uicontroller will take care of it (hopefully)

    [SerializeField] private Transform mainCanvas;
    [SerializeField] private GameObject buildMenu;
    private BlurController blurController;
    private PopupsHandler popupsHandler;
    [HideInInspector] public OverlayHandler overlayHandler;
    private UIBuildMenu uiBuildMenu;
    public static UIController instance;

    public enum State 
    {
        Default,
        Building,
        Following
    }
    public State uiState;
    private State currentState;
  [HideInInspector] public bool inBuildingMode = false;
    private void Awake()
    {
        /////
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(this);
        }
        //////


        blurController = GetComponent<BlurController>();
        popupsHandler = GetComponent<PopupsHandler>();
        overlayHandler = GetComponent<OverlayHandler>();
        uiBuildMenu = GetComponent<UIBuildMenu>();
        if (mainCanvas == null) Debug.LogError("MAIN CANVAS IS NOT ASSIGNED IN UICONTROLER");

    }

    public State GetUIState() { return uiState; }
    public bool isBuildMenuActive()
    {
        if (buildMenu.activeInHierarchy) return true;
        return false;
    }
    public bool isIndicatorActive() { return overlayHandler.isIndicatorActive(); }
    private void Update()
    {
        if (Camera.main.gameObject.GetComponent<CameraController>().FollowingTarget) uiState = State.Following;

        if(currentState != uiState)
        {
            switch (uiState)
            {
                case State.Default:
                   overlayHandler.HideIndicator();
                    break;
                case State.Building:
                    overlayHandler.ShowIndicator("Building mode");
                    if (isBuildMenuActive()) overlayHandler.CloseBuildMenu();
                    break;
                case State.Following:
                    overlayHandler.ShowIndicator("Following bug");

                    break;
                default:
                    break;
            }
        }

        currentState = uiState;
    }
    public void CreatePopup(int id, string header = default, string content = default,GameObject callbackObj = null) => popupsHandler.CreateNewPopup(id,mainCanvas,header,content,callbackObj);
    public void OpenBuildMenu() => overlayHandler.OpenBuildMenu();
    public void CloseBuildMenu() => overlayHandler.CloseBuildMenu();
    public void HideRoomUI() => Room_UI.Instance.Hide();
    public void HideBugUI() => DBG_UnitUI.Instance.Hide();

    public void ExitBuildingMode() => SetDefaultState();
    public void SetDefaultState()
    {
        if (uiState == State.Following) Camera.main.gameObject.GetComponent<CameraController>().ResetTarget();

        SetState(State.Default);
    }
    //this will be called once player selects room to build
    public void Build_BuildRequest(int roomID)
    {
        //ask game/build manager if player can build currently selected room (id of room)

        //then call Build_CanBuildRoom from build manager with either true or false

        //for debug i call it here with true
        UI_SetBuildMode(true);
    }
    public void UI_SetBuildMode(bool canBuild) 
    {
        if (canBuild)
            SetState(State.Building);
        else
            SetState(State.Default);
    }

    public void SetState(State state) => uiState = state;




}
