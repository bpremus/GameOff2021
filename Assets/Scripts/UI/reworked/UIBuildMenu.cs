using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBuildMenu : MonoBehaviour
{
    [SerializeField] private float fadeTime = 0.1f;
    [SerializeField] private LeanTweenType ease = LeanTweenType.easeSpring;
    private CanvasGroup canvasGroup;
    private UIController uiController;
    private float awakeTime;
    //  0 - corridor    1 - storage    2 - barracks    3 - resource room    4 - queen room
    private void Awake()
    {
        uiController = FindObjectOfType<UIController>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        awakeTime = 0;
    }
    private void Update()
    {

        if (awakeTime > 2f && canvasGroup.alpha != 1) canvasGroup.alpha = 1;
        awakeTime += Time.deltaTime;
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
        // LeanTween.alphaCanvas(canvasGroup, 1, fadeTime).setEase(ease);
        canvasGroup.alpha = 1;
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
