using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIBugButton : MonoBehaviour
{
    public CoreBug bug;
    TextMeshProUGUI text;

    [SerializeField]
    Sprite[] icon_images;

    Image button_image;


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
        if (bug)
        {
            string bug_name = bug.name;
            if (bug.bug_evolution == CoreBug.BugEvolution.drone)
            {
                button_image.sprite = icon_images[0];
                bug_name = "Drone";
            }
            else if (bug.bug_evolution == CoreBug.BugEvolution.warrior)
            {
                button_image.sprite = icon_images[1];
                bug_name = "Warrior";
            }
            else if (bug.bug_evolution == CoreBug.BugEvolution.claw)
            {
                button_image.sprite = icon_images[2];
                bug_name = "Claw";
            }
            else if (bug.bug_evolution == CoreBug.BugEvolution.range)
            {
                button_image.sprite = icon_images[3];
                bug_name = "Spit";
            }
            else if (bug.bug_evolution == CoreBug.BugEvolution.cc_bug)
            {
                button_image.sprite = icon_images[4];
                bug_name = "Slow";
            }

            text.text = bug_name;
        }
           
    }

}
