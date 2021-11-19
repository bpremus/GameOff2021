using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupsHandler : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> popupsList;
    [SerializeField] private GameObject activePopup;
    private BlurController blurController;

    private void Awake()
    {
        blurController = GetComponent<BlurController>();
    }
    public bool isPopupActive()
    {
        if (activePopup != null) return true;
        return false;
    }
    public void CreateNewPopup(int id, Transform parent,string header = default,string content = default)
    {
        if (activePopup == null)
        {
            if(id <= popupsList.Count)
            {
                activePopup = Instantiate(popupsList[id].gameObject, parent);
                if(blurController)blurController.EnableBlur();

                UIPopup uIPopup = activePopup.GetComponent<UIPopup>();
                if (uIPopup)
                {
                    if (header != default)
                    {
                        uIPopup.SetHeaderText(header);
                    }
                    if (content != default)
                    {
                        uIPopup.SetContentText(content);
                    }
                }
                else Debug.LogWarning("Popup with id " + id + " is missing UIPopup script!");
 
            }
            else
            {
                Debug.LogError("Popup with id " + id + "doesnt exist!");
            }
             
        }
        else
        {
            Debug.LogWarning("Disable active popup first before creating a new one!");
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

    public void SetCurrentPopupHeader(string text)
    {
        if (activePopup != null)
        {
            activePopup.GetComponent<UIPopup>().SetHeaderText(text);
        }
    }
    public void SetCurrentPopupContent(string text)
    {
        if (activePopup != null)
        {
            activePopup.GetComponent<UIPopup>().SetContentText(text);
        }
    }
}
