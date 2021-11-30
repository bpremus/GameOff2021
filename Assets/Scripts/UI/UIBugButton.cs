using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIBugButton : MonoBehaviour
{
    public CoreBug bug;
    TextMeshProUGUI text;

    [SerializeField]
    Sprite[] icon_images;

    Image button_image;

    private bool startTimer;
    private float elapsedTime;
    public void Start()
    {
        
        button_image = GetComponent<Image>();

        TextMeshProUGUI t = GetComponentInChildren<TextMeshProUGUI>();
        if (t)
        {
            text = t;
        }
    }

    public void Update()
    {
        if (startTimer) CountTime();
        if (bug)
        {
            ArtPrefabsInstance.Instance.SetBugName(bug);
            if (bug.bug_evolution == CoreBug.BugEvolution.drone)
            {
                button_image.sprite = icon_images[0];
            }
            else if (bug.bug_evolution == CoreBug.BugEvolution.warrior)
            {
                button_image.sprite = icon_images[1];
            }
            else if (bug.bug_evolution == CoreBug.BugEvolution.claw)
            {
                button_image.sprite = icon_images[2];
            }
            else if (bug.bug_evolution == CoreBug.BugEvolution.range)
            {
                button_image.sprite = icon_images[3];
            }
            else if (bug.bug_evolution == CoreBug.BugEvolution.cc_bug)
            {
                button_image.sprite = icon_images[4];
            }
            text.text = bug.name;
        }
           
    }
    private void OnEnable()
    {
        startTimer = true;
        elapsedTime = 0;
    }
    private void OnDisable()
    {
        startTimer = false;
    }
    private void CountTime()
    {
        if (elapsedTime > 0.22f)
        {
            if(!GetComponent<ButtonAnim>().displayedCorrectly)
                    GetComponent<ButtonAnim>().ForceDisplay();
        }
      elapsedTime += Time.deltaTime;
    }
}
