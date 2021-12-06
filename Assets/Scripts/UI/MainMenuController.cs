using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class MainMenuController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject loadingPanel;
    [Header("Menu camera mover")]
    [SerializeField] private float scrollSpeed;
    [SerializeField] private Vector2 maxBoundaries;
    private Vector2 randomPoint;
    private Vector3 startingCamPos;
    Transform cam;

    [Header("Tabs")]
    [SerializeField] private GameObject contentButtons;
     private List<GameObject> mainbuttons;
    [SerializeField] private GameObject contentGamesSaved;
    private List<GameObject> gamesSavedChilds;

    [SerializeField] private GameObject contentSettings;
    private List<GameObject> settingsChilds;

    private void Awake() 
    {
        cam = Camera.main.transform;
        mainbuttons = new List<GameObject>();
        gamesSavedChilds = new List<GameObject>();
        settingsChilds = new List<GameObject>();
        InitiateMenu();
    }
    private void InitiateMenu()
    {
        SetupChilds();

        startingCamPos = cam.position;
        loadingPanel.SetActive(false);
        SetRandomTargetPointForCamera();
        SwitchGamesSavedChilds(false);
        SwitchMainMenuButtons(true);
    }
    private void SetupChilds()
    {
        for (int i = 0; i < contentButtons.transform.childCount; i++)
        {
            GameObject child = contentButtons.transform.GetChild(i).gameObject;
            mainbuttons.Add(child);
            Debug.Log("Added:" + mainbuttons[i].name + " to list of main buttons");
        }
        for (int i = 0; i < contentGamesSaved.transform.childCount; i++)
        {
            GameObject child = contentGamesSaved.transform.GetChild(i).gameObject;
            gamesSavedChilds.Add(child);
            Debug.Log("Added:" + gamesSavedChilds[i].name + " to list of child (loadGamePanel)");
        }
        for (int i = 0; i < contentSettings.transform.childCount; i++)
        {
            GameObject child = contentSettings.transform.GetChild(i).gameObject;
            settingsChilds.Add(child);
            Debug.Log("Added:" + settingsChilds[i].name + " to list of child (settingsPanel)");
        }
    }
    private void Update()
    {
        MoveCamera();

        if (loadingPanel.activeInHierarchy)
            loadingPanel.GetComponent<CanvasGroup>().alpha += 0.03f;
    }
    #region Background camera movement
    private void MoveCamera()
    {
        Vector3 target = new Vector3(randomPoint.x, randomPoint.y, startingCamPos.z);
        cam.transform.position = Vector3.MoveTowards(cam.transform.position, target, scrollSpeed * Time.deltaTime);
        float dist = Vector3.Distance(cam.position, target);
        if (dist <= 1f)
        {
            SetRandomTargetPointForCamera();
        }
    }
    private void SetRandomTargetPointForCamera()
    {
        Vector2 randomPoint = new Vector3(UnityEngine.Random.Range(-maxBoundaries.x, maxBoundaries.x), (UnityEngine.Random.Range(-maxBoundaries.y, maxBoundaries.y)));
        this.randomPoint = randomPoint;
    }

    #endregion

    #region Buttons
    public void NewGame()
    {
        loadingPanel.SetActive(true);
        StartCoroutine(WaitToLoad());
    }
    public void OpenSavedGames()
    {
        DisplaySavedGames();
    }

    public void Credits()
    {

    }
    public void Settings()
    {
        DisplaySettingsMenu();
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void BackToMenu()
    {
        ResetAllPanels();
        SwitchMainMenuButtons(true);

    }
    #endregion

    #region Panel switching

    private void DisplaySavedGames()
    {
        ResetAllPanels();
        SwitchGamesSavedChilds(true);
    }
    private void DisplaySettingsMenu()
    {
        ResetAllPanels();
        SwitchSettingsChilds(true);
    }
    private void ResetAllPanels()
    {
        SwitchGamesSavedChilds(false);
        SwitchSettingsChilds(false);
        SwitchMainMenuButtons(false);
    }
    #endregion
    #region Others
    private IEnumerator WaitToLoad()
    {
        yield return new WaitForSeconds(0.9f);
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
    }

    private void SwitchMainMenuButtons(bool state)
    {
        foreach(GameObject button in mainbuttons)
        {
            button.SetActive(state);
        }
    }
    private void SwitchGamesSavedChilds(bool state)
    {
        foreach (GameObject child in gamesSavedChilds)
        {
            child.SetActive(state);
        }
    }
    private void SwitchSettingsChilds(bool state)
    {
        foreach (GameObject child in settingsChilds)
        {
            child.SetActive(state);
        }
    }
    private void HideAll()
    {

    }
    #endregion

}
