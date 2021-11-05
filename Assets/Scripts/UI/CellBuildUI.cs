using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CellBuildUI : MonoBehaviour
{
    [SerializeField] private GameObject buildMenu;
    [SerializeField] private LeanTweenType ease;
    private CellSelection cellSelection;
    private BuildManager buildManager;
    private bool windowActive;
    private float targetPos;
    private float offScreenPos;
    private void Awake()
    {
        cellSelection = FindObjectOfType<CellSelection>();
        buildManager = FindObjectOfType<BuildManager>();
        if (buildManager == null) Debug.LogError("Build manager not found!");
      //  offScreenPos = Screen.width + 180;
     //   targetPos = offScreenPos - 50f;
    }
    private void Start()
    {
        buildMenu.GetComponent<CanvasGroup>().alpha = 0;
        buildMenu.SetActive(false);
    }
    private void Update()
    {
        if (cellSelection.selectedCell) DisplayUI();
        else HideUI();
    }
    public void CreateRoomButton()
    {
        buildManager.CreateNewRoom(1);
    }
    public void CreateCorridorButton()
    {
        buildManager.CreateCorridor(0);
    }
    public void DestroyRoomButton()
    {
        buildManager.DestroyRoom();
    }
    private void DisplayUI()
    {
        if (windowActive) return;
        windowActive = true;
        buildMenu.SetActive(true);
        LeanTween.alphaCanvas(buildMenu.GetComponent<CanvasGroup>(), 1, 0.35f);
     //   LeanTween.moveLocalX(buildMenu, targetPos, 0.2f).setEase(ease);
    }

    [ContextMenu("Manual UI Hide")]
    public void HideUI()
    {
        if (!windowActive) return;
        windowActive = false;
        cellSelection.selectedCell = null;
    //    LeanTween.moveLocalX(buildMenu, offScreenPos, 0.2f);
        LeanTween.alphaCanvas(buildMenu.GetComponent<CanvasGroup>(), 0, 0.15f).setOnComplete(DisableGO);
    }
    private void DisableGO() => buildMenu.SetActive(false);
}
