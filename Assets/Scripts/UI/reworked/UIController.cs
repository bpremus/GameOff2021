using System;
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
    public static UIController instance;

    public enum UIElements { none, build_corridor, build_harvester, build_salvage, build_war_room,
    assign_bug, send_gathering, recall_bug, evolve_drone};

    [SerializeField]
    private Card_BuildMenu[] menu_cards;

    private CanvasGroup canvasGroup;


    public void IntroFadeIN()
    {
        canvasGroup.alpha = 1;
    }
    public void IntroFadeOUT()
    {
        canvasGroup.alpha = 0;
    }

    public void RestrictBuilds(List<int> restrictedBuilds)
    {
        buildMenu.GetComponent<UIBuildMenu>().RestrictBuilds(restrictedBuilds);
    }
    public void RestrictUnits(List<int> restrictedUnits,bool restricted)
    {
        DBG_UnitUI.Instance.RestrictUnits(restrictedUnits,restricted);
        Debug.Log("unit ui restrict detected");
    }
    public void EnableUIElement(UIElements ui_elements)
    {

        if (menu_cards.Length == 0)
            menu_cards = buildMenu.GetComponentsInChildren<Card_BuildMenu>();

        if (ui_elements == UIElements.build_corridor)
            menu_cards[0].gameObject.SetActive(true);
        if (ui_elements == UIElements.build_harvester)
            menu_cards[1].gameObject.SetActive(true);

    }

    public void DisableUIElements(UIElements ui_elements)
    {
        if (menu_cards.Length == 0)
            menu_cards = buildMenu.GetComponentsInChildren<Card_BuildMenu>();
    }

    public void DisableBuildCards()
    { 
        if (menu_cards.Length == 0)
            menu_cards = buildMenu.GetComponentsInChildren<Card_BuildMenu>();
        foreach (Card_BuildMenu card in menu_cards)
        {
            card.gameObject.SetActive(false);
        }
    }


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
        canvasGroup = mainCanvas.GetComponent<CanvasGroup>();
        if (mainCanvas == null) Debug.LogError("MAIN CANVAS IS NOT ASSIGNED IN UICONTROLER");



        if (isBuildMenuActive()) CloseBuildMenu();

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

        //   if (GameController.Instance.OnFoodValueChanged()) { buildMenu.GetComponent<UIBuildMenu>().CheckIfCanAfford(); }   //for some reason function gets called but its not executed(?) 
        //using this instead now
        if (isBuildMenuActive()) 
        { 
            buildMenu.GetComponent<UIBuildMenu>().CheckIfCanAfford(); //very inefficient way of doing this
        } 


        if (Camera.main.gameObject.GetComponent<CameraController>().FollowingTarget) uiState = State.Following;
        if (isBuildMenuActive() || isSettingsMenuActive()) Camera.main.GetComponent<CameraController>().SetDraggingState(false);


        ControlUIStates();

    }
   
    private void ControlUIStates()
    {
        if (currentState != uiState)
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
                    overlayHandler.ShowIndicator("Following");
                    inBuildingMode = false;
                    overlayHandler.DisableBuildButton();
                    break;
                default:
                    break;
            }
        }

        currentState = uiState;
    }
    #region References Functions
    public void CreatePopup(int id, string header = default, string content = default,GameObject callbackObj = null) => popupsHandler.CreateNewPopup(id,mainCanvas,header,content,callbackObj);
    public void OpenBuildMenu() => overlayHandler.OpenBuildMenu();
    public void CloseBuildMenu() => overlayHandler.CloseBuildMenu();
    public void CloseSettingsMenu() => overlayHandler.CloseSettingsMenu();
    public void HideRoomUI() => Room_UI.Instance.Hide();
    public void HideBugUI() => DBG_UnitUI.Instance.Hide();
    public void EnableBlur() => blurController.EnableBlur();
    public void DisableBlur() => blurController.DisableBlur();
    public void HideAllUI() => CellSelectProto.Instance.CloseUI();
    public void HideOverlays() => overlayHandler.SwitchVisibility(false);
    public void ShowOverlays() => overlayHandler.SwitchVisibility(true);
    public void HideEverything() { HideAllUI();HideOverlays(); }
    public void ExitBuildingMode() => SetDefaultState();
    #endregion
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
        if (BuildManager.Instance.TryBuild(roomID))
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
