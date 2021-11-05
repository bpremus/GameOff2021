using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CellSelection : MonoBehaviour
{
    public GameObject selectedCell;

    [SerializeField] private GameObject cellSelectionImg;
    [SerializeField] private Color hoverColor;
    [SerializeField] private Color clickColor;
    [SerializeField] private Sprite hoverImage;
    [SerializeField] private Sprite selectionImage;
    private CanvasGroup cgroup;
    private bool inSelection;
    private void Awake() => cgroup = cellSelectionImg.GetComponent<CanvasGroup>();
    private void Start()
    {
        selectedCell = null;
        cgroup.alpha = 0;
    }
    private void Update()
    {
        //needs to be optimized


        //Check if player is aiming at cell
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            GameObject objectHit = hit.transform.gameObject;
            if(objectHit.layer == 6) //cell
            {
                //aiming at cell and clicked on it
                if (Input.GetMouseButtonDown(0))
                {
                    SwitchSelection(objectHit);
                }
                //no cell selected, hover over current one
                if (!inSelection) 
                {
                    HoverMode(objectHit);
                }


                cellSelectionImg.SetActive(true);
                cgroup.alpha = 1;

            }
            else
            {
                //no cell selected and not aiming at any
                if(!inSelection)
                {
                    cgroup.alpha = 0;
                    cellSelectionImg.SetActive(false);
                }
                 
            }
        }

        if (selectedCell == null && inSelection) 
        { 
            inSelection = false;
            cgroup.alpha = 0;
            cellSelectionImg.SetActive(false);
        }
    }
    private void SwitchSelection(GameObject objHit)
    {
        //*Aiming at cell and clicked*


        //if not selected anything, select this
        if (selectedCell == null && !inSelection)
        {
            SelectionMode(objHit);
            inSelection = true;
        }
        //if already selected, unselect current one
        else if (inSelection && selectedCell != null)
        {
            selectedCell = null;
            inSelection = false;
        }

    }
    private void HoverMode(GameObject objHit)
    {
        transform.position = objHit.transform.position;
        cellSelectionImg.GetComponent<Image>().color = hoverColor;
        cellSelectionImg.GetComponent<Image>().sprite = hoverImage;
        cellSelectionImg.GetComponent<RectTransform>().sizeDelta = new Vector2(1.55f, 1.55f);

    }
    private void SelectionMode(GameObject objHit)
    {
        selectedCell = objHit;
        transform.position = objHit.transform.position;
        cellSelectionImg.GetComponent<Image>().color = clickColor;
        cellSelectionImg.GetComponent<Image>().sprite = selectionImage;
        cellSelectionImg.GetComponent<RectTransform>().sizeDelta = new Vector2(1.3f, 1.3f);

    }



}
