using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slow_rotate : MonoBehaviour
{
    [SerializeField]
    GameObject rotation_elem;

    [SerializeField] private Color baseColor;
    [SerializeField] private Color fadeToColor;
    [SerializeField] private float animTime;
    // Start is called before the first frame update
    [SerializeField]
    float rot_speed = 20;
    private void Start()
    {
        FadeOut();
        
    }
    private void Update()
    {
        transform.RotateAround(rotation_elem.transform.position, new Vector3(0, 0, 1), rot_speed * Time.deltaTime);

    }
    private void FadeOut()
    {
        LeanTween.color(rotation_elem, baseColor, animTime).setOnComplete(FadeIn);
    }
    private void FadeIn()
    {
        LeanTween.color(rotation_elem, fadeToColor, animTime).setOnComplete(FadeOut);
    }
}
