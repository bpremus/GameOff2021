using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject creditsPanel;

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
    }

    private void SetRandomPoint()
    {
        Vector2 randomPoint = new Vector3(UnityEngine.Random.Range(-maxBoundaries.x, maxBoundaries.x), (UnityEngine.Random.Range(-maxBoundaries.y, maxBoundaries.y)));
        this.randomPoint = randomPoint;
    }
    private void Update()
    {
        Vector3 target = new Vector3(randomPoint.x, randomPoint.y, defaultPosition.z);
        Debug.Log(target);
        cam.transform.position = Vector3.MoveTowards(cam.transform.position, target, scrollSpeed * Time.deltaTime);
        float dist = Vector3.Distance(cam.position, target);
        if(dist <= 5f)
        {
            SetRandomPoint();
        }
    }
    public void NewGame()
    {
        SceneManager.LoadScene(1);
    }
    public void LoadGame()
    {
        //???
    }
    public void Credits()
    {
        //hide this panel and display credits panel
        mainPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }
    public void ToMenu()
    {
        mainPanel.SetActive(true);
        creditsPanel.SetActive(false);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
