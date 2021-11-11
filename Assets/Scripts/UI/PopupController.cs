using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupController : MonoBehaviour
{
    [SerializeField] private bool showDebug = false;
    [SerializeField] private List<GameObject> popupsList;
    [SerializeField] private GameObject activePopup;

    private BlurController blurController;

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
                CreateNewPopup(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                CreateNewPopup(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                CreateNewPopup(2);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                CreateNewPopup(3);
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
                    CreateNewPopup(UnityEngine.Random.Range(0, popupsList.Count));
                    SetCurrentPopupHeader("New header text");
                    SetCurrentPopupContent("This is a changed content text");
                }
            }
            //*********************************************************
        }

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

    public void CreateNewPopup(int id)
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
        }
            
    }

}
