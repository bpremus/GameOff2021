using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private GameObject[] tutorialPanels;
    [SerializeField] private GameObject raycastBlocker;
    public static TutorialUI instance;
    private GameObject currentPanel;
    private int currentIndex;
    private int maxIndex;
    private void Start()
    {
        currentIndex = 0;
        maxIndex = tutorialPanels.Length -1;



        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(this);
        }


        //  EnableFirst();
    }
    public void EnableFirst()
    {

        Activate(currentIndex);
    }
    public void SwitchRight()
    {
        if(currentIndex < maxIndex)
        {
            currentIndex++;
            SwitchToActive();
        }
        else
        {
            currentIndex = 0;
            SwitchToActive();
        }
    }
    public void SwitchLeft()
    {
        if(currentIndex > 0)
        {
            currentIndex--;
            SwitchToActive();
        }
        else
        {
            currentIndex = maxIndex;
            SwitchToActive();
        }
    }

    public void Activate(int index)
    {
        tutorialPanels[index].SetActive(true);
        raycastBlocker.SetActive(true);
    }
    public void HideAll()
    {
        foreach (GameObject panel in tutorialPanels)
        {
            panel.SetActive(false);
        }
        raycastBlocker.SetActive(false);
    }
    public void SwitchToActive()
    {
        HideAll();
        Activate(currentIndex);
    }
}
