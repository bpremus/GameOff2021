using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBuildMenu : MonoBehaviour
{
    [SerializeField] private float fadeTime = 0.1f;
    [SerializeField] private LeanTweenType ease = LeanTweenType.easeSpring;
    [SerializeField] private GameObject[] roomsPanels;
    private CanvasGroup canvasGroup;
    private UIController uiController;
    private float awakeTime;
    //  0 - corridor    1 - salvage    2 - barracks    3 - harvesting   4 - hatchery

    private void Awake()
    {
        uiController = FindObjectOfType<UIController>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        awakeTime = 0;


      CheckIfCanAfford();
    }
    public void CheckIfCanAfford()
    {
       
        foreach (GameObject room in roomsPanels)
        {

            Card_BuildMenu buildCard = room.GetComponent<Card_BuildMenu>();
            if (buildCard.isRestricted()) break;
            buildCard.SetAvailable(BuildManager.Instance.CanBuildRoom(buildCard.getID()));
        }
    }
    public void RestrictBuilds(List<int> restrictedBuilds)
    {
        if (roomsPanels.Length < 1) Debug.LogError("No roomPanels assigned in inspector!!");
        foreach (GameObject room in roomsPanels)
        {
            Card_BuildMenu buildCard = room.GetComponent<Card_BuildMenu>();

            if (restrictedBuilds.Contains(buildCard.getID()))
            {
                buildCard.SetRestricted(true);
            }
            else
            {
                buildCard.SetRestricted(false);
            }
        }
    }

    private void Update()
    {
        if (awakeTime > 2f && canvasGroup.alpha != 1) canvasGroup.alpha = 1;
        awakeTime += Time.deltaTime;
    }
    public void CheckForSelectedRoom(int roomID)
    {
        Debug.Log("clicked " + roomID + " room to build");
        uiController.Build_BuildRequest(roomID);
    }
    public void CloseMenu()
    {
        uiController.overlayHandler.CloseBuildMenu();
        HideWindow();
    }
    public void ShowWindow()
    {
        // LeanTween.alphaCanvas(canvasGroup, 1, fadeTime).setEase(ease);
        canvasGroup.alpha = 1;
        CheckIfCanAfford();
    }
    public void HideWindow()
    {
        // LeanTween.alphaCanvas(canvasGroup, 0, fadeTime).setEase(LeanTweenType.linear);
        canvasGroup.alpha = 0;
    }
    private void OnEnable()
    {
        awakeTime = 0;
        ShowWindow();
    }
    private void OnDisable()
    {
        CloseMenu();
    }
}
