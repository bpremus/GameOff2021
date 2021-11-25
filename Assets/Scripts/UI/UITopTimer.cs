using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITopTimer : MonoBehaviour
{
    [SerializeField]
    Scrollbar scrollbar;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float p = GameController.Instance.GetTimePercent();
        scrollbar.size = p / 100;
    }
}
