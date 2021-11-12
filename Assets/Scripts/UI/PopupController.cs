using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PopupController : MonoBehaviour
{
    [SerializeField] private bool showDebug = false;
    [SerializeField] private List<GameObject> popupsList;
    [SerializeField] private GameObject activePopup;
    [SerializeField] private GameObject buildMenuButton;
    [SerializeField] private GameObject buildingIndicator;
    private BlurController blurController;
    public bool isPopupActive()
    {
        if (activePopup != null) return true;
        return false;
    }
    private void Awake()
    {
        blurController = FindObjectOfType<BlurController>();
    }
    private void Update()
    {
        if (showDebug)
        {
            //****************************** Debug setup ******************************
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                CreateNewWindow(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                CreateNewWindow(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                CreateNewWindow(2);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                CreateNewWindow(3);
            }

            if (Input.GetKeyDown(KeyCode.T))
            {
                if (activePopup)
                {
                    SetCurrentPopupHeader("New header text");
                    SetCurrentPopupContent("This is a changed content text");
                }
                else
                {
                    CreateNewWindow(UnityEngine.Random.Range(0, popupsList.Count));
                    SetCurrentPopupHeader("New header text");
                    SetCurrentPopupContent("This is a changed content text");
                }
            }
            //*********************************************************
        }

    }
    public void ChangeCanvasGroup(float to)
    {
        if(buildMenuButton != null)
        {
            CanvasGroup canvasGroup =  buildMenuButton.GetComponent<CanvasGroup>();
            canvasGroup.alpha = to;
            if (to > 0)
                buildMenuButton.GetComponent<Button>().interactable = true;
            else
                buildMenuButton.GetComponent<Button>().interactable = false;
        }
    }
    public void DisplayBuildingIndicator()
    {
        buildingIndicator.SetActive(true);
    }
    public void HideBuildingIndicator()
    {
        buildingIndicator.SetActive(false);
    }
    //Set custom header text for current popup
    public void SetCurrentPopupHeader(string text)
    {
        if(activePopup != null)
        {
            activePopup.GetComponent<UIPopup>().SetHeaderText(text);
        }
    }

    //Set custom content text for current popup
    public void SetCurrentPopupContent(string text)
    {
        if (activePopup != null)
        {
            activePopup.GetComponent<UIPopup>().SetContentText(text);
        }
    }

    public void CreateNewWindow(int id)
    {
        if(activePopup == null)
        {
            if (blurController) blurController.EnableBlur();
            activePopup = Instantiate(popupsList[id].gameObject, transform);
        }
        else
        {
            Debug.LogError("Cant create a new popup becouse theres already one");
        }
    }
    public void DeleteCurrentPopup()
    {
        if (activePopup)
        {
            if (blurController) blurController.DisableBlur();
            activePopup = null;
            ChangeCanvasGroup(1);
        }
            
    }

}
