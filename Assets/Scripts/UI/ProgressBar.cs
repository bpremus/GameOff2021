using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] Scrollbar progress_bar;

    public void Start()
    {
        HideProgressBar();
    }

    public void SetProgress(float percent)
    {
        progress_bar.gameObject.SetActive(true);
        progress_bar.size = percent;
    }

    public void HideProgressBar()
    {
        progress_bar.gameObject.SetActive(false);
    }


}
