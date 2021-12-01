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
    private void Start()
    {
      
    }
    public State GetUIState() { return uiState; }
    public bool isBuildMenuActive()
    {
        if (buildMenu.activeInHierarchy) return true;
        return false;
    }
    public bool isSettingsMenuActive()
    {
       return  overlayHandler.IsSettingsMenuActive();
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
                    CellSelectProto.Instance.ClearSelectionState();
                    GhostRoomDisplayer.instance.HideGhostRoom();
                    overlayHandler.BuildingMode(false);
                    inBuildingMode = false;
                    break;
                case State.Building:
                    overlayHandler.BuildingMode(true);
                    inBuildingMode = true;
                    break;
                case State.Following:
                    overlayHandler.ShowIndicator("Following bug");
                    inBuildingMode = false;
                    overlayHandler.DisableBuildButton();
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
    public void CloseSettingsMenu() => overlayHandler.CloseSettingsMenu();
    public void HideRoomUI() => Room_UI.Instance.Hide();
    public void HideBugUI() => DBG_UnitUI.Instance.Hide();
    public void HideAllUI() => CellSelectProto.Instance.CloseUI();
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
        if (BuildManager.Instance.CanBuildRoom(roomID))
        {
            UI_SetBuildMode(true);
            GhostRoomDisplayer.instance.DisplayGhostRoom(roomID);
        }


      //  if (roomID > 0) BuildManager.Instance.CreateCorridor(0);
      //  else BuildManager.Instance.CreateNewRoom(roomID);
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
