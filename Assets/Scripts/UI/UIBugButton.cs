using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBugButton : MonoBehaviour
{
    public CoreBug bug;
    Text text;


    public void Start()
    {
        Text t = GetComponentInChildren<Text>();
        if (t)
        {
            text = t;
        }
    }

    public void Update()
    {
        if (bug)
            text.text = bug.name;
    }

}
