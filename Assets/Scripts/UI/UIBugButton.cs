using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIBugButton : MonoBehaviour
{
    public CoreBug bug;
    TextMeshProUGUI text;


    public void Start()
    {
        TextMeshProUGUI t = GetComponentInChildren<TextMeshProUGUI>();
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
