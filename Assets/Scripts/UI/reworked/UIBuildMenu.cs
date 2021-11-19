using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBuildMenu : MonoBehaviour
{
    [SerializeField] private float fadeTime = 0.1f;
    [SerializeField] private LeanTweenType ease = LeanTweenType.easeSpring;
    private CanvasGroup canvasGroup;
    private UIController uiController;
    //  0 - corridor    1 - storage    2 - barracks    3 - resource room    4 - queen room
    private void Awake()
    {
        uiController = FindObjectOfType<UIController>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
    }
    public void CheckForSelectedRoom(int roomID)
    {
        uiController.Build_BuildRequest(roomID);
    }

    public void CloseMenu()
    {
        uiController.overlayHandler.CloseBuildMenu();
        HideWindow();
    }
    public void ShowWindow()
    {
        LeanTween.alphaCanvas(canvasGroup, 1, fadeTime).setEase(ease);
    }
    public void HideWindow()
    {
        LeanTween.alphaCanvas(canvasGroup, 0, fadeTime).setEase(LeanTweenType.linear);
    }
    private void OnEnable()
    {
        ShowWindow();
    }
    private void OnDisable()
    {
        CloseMenu();
    }
}
