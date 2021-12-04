using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class MainMenuController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject leftPanel;
    [SerializeField] private GameObject rightPanel;
    [Space(10)]
    [SerializeField] private GameObject rp_Creators;
    [SerializeField] private GameObject rp_Settings;
    [SerializeField] private GameObject rp_Saved;
    [Space(10)]
    [SerializeField] private GameObject loadingScreen;

    [Header("Menu camera mover")]
    [SerializeField] private float scrollSpeed;
    [SerializeField] private Vector2 maxBoundaries;
    private Vector2 randomPoint;
    private Vector3 defaultPosition;
    Transform cam;
    private void Awake()
    {
        ToMenu();

        cam = Camera.main.transform;
        defaultPosition = cam.position;
        leftPanel.SetActive(true);
        rightPanel.SetActive(false);
    }

    private void SetRandomPoint()
    {
        Vector2 randomPoint = new Vector3(UnityEngine.Random.Range(-maxBoundaries.x, maxBoundaries.x), (UnityEngine.Random.Range(-maxBoundaries.y, maxBoundaries.y)));
        this.randomPoint = randomPoint;
    }
    private void Update()
    {

        if (rightPanel.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                rightPanel.SetActive(false);
            }
        }




        Vector3 target = new Vector3(randomPoint.x, randomPoint.y, defaultPosition.z);
        Debug.Log(target);
        cam.transform.position = Vector3.MoveTowards(cam.transform.position, target, scrollSpeed * Time.deltaTime);
        float dist = Vector3.Distance(cam.position, target);
        if(dist <= 1f)
        {
            SetRandomPoint();
        }
    }
    public void NewGame()
    {
        rightPanel.SetActive(false);
        loadingScreen.SetActive(true);
        StartCoroutine(StartCooldown());


    }
    private IEnumerator StartCooldown()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
    }
    public void LoadGame()
    {
        rightPanel.SetActive(true);
        rp_Settings.SetActive(false);
        rp_Creators.SetActive(true);
        rp_Saved.SetActive(true);
    }
    public void Credits()
    {
        //hide this panel and display credits panel
        rightPanel.SetActive(true);
        rp_Settings.SetActive(false);
        rp_Saved.SetActive(false);
        rp_Creators.SetActive(true);
    }
    public void Settings()
    {
        rightPanel.SetActive(true);
        rp_Creators.SetActive(false);
        rp_Saved.SetActive(false);
        rp_Settings.SetActive(true);
    }
    public void ToMenu()
    {

    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
