using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class OverlayHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject buildButton;

    [SerializeField]
    private GameObject optionsListButton;

    [SerializeField]
    private GameObject tutorialButton;

    [SerializeField]
    private GameObject buildMenu;

    [SerializeField]
    private GameObject settingsMenu;

    [SerializeField]
    private GameObject topIndicator;

    [SerializeField]
    private TextMeshProUGUI topIndicatorText;
    [SerializeField]
    private Button topIndicatorButton;

    private UIController uiController;

    public bool isIndicatorActive()
    {
        if (topIndicator.activeInHierarchy) return true;
        return false;
    }
    private void Awake()
    {
        uiController = GetComponent<UIController>();
    }
    public void DisableBuildButton() => ButtonInteractable(buildButton, false);
    public void EnableBuildButton() =>  ButtonInteractable(buildButton, true);
    public void OpenBuildMenu()
    {
        if (!uiController.isBuildMenuActive())
        {
            buildMenu.SetActive(true);
            ButtonInteractable(buildButton, false);
        }
    }
    public void CloseBuildMenu()
    {
        if (uiController)
        {
            if (uiController.isBuildMenuActive())
            {
                Debug.Log("closing menu");
                buildMenu.SetActive(false);
                ButtonInteractable(buildButton, true);
            }
        }


    }
    public void OpenSettingsMenu()
    {
        uiController.HideAllUI();
        settingsMenu.SetActive(true);
        uiController.gameObject.GetComponent<BlurController>().EnableBlur();
    }
    public void CloseSettingsMenu()
    {
        settingsMenu.SetActive(false);
        uiController.gameObject.GetComponent<BlurController>().DisableBlur();
    }
    public bool IsSettingsMenuActive()
    {
        return settingsMenu.activeInHierarchy;
    }
    public void BuildingMode(bool building)
    {
        if (building)
        {
            CloseBuildMenu();
            ShowIndicator("Building mode");
            ButtonInteractable(buildButton, false);
        }
        else
        {
            ButtonInteractable(buildButton, true);
            HideIndicator();
            return;
        }
    }
    public void ShowIndicator(string operationType = default)
    {
        topIndicator.SetActive(true);
        if (operationType != default)
            topIndicatorText.text = operationType;
    }
    public void HideIndicator() //after placing room (?)
    {
        topIndicator.SetActive(false);
    }
    private void ButtonInteractable(GameObject button_go,bool f)
    {
        Button button = button_go.GetComponent<Button>();
        if(button)
            button.GetComponent<Button>().interactable = f;
    }
}
