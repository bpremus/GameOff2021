using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] Scrollbar progress_bar;

    Vector3 eulter_angles = Vector3.zero;
    public void Start()
    {
        HideProgressBar();
        eulter_angles = transform.eulerAngles;
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
    
    public void LateUpdate()
    {
        eulter_angles.y = transform.eulerAngles.y;
        transform.eulerAngles = eulter_angles;
        Vector3 pos = transform.position;
        pos.x = transform.parent.position.x;
        transform.position = pos;
    }
}
