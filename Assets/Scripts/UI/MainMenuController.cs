using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject creditsPanel;

    private void Awake()
    {
        ToMenu();
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
