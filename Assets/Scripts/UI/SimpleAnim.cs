using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CanvasGroup))]
public class SimpleAnim : MonoBehaviour
{
    [Header("Basic")]
    [SerializeField] private float fadeInTime = 0.2f;
    [SerializeField] private float fadeOutTime = 0.1f;

    [Header("Optional scaling up when on")]
    [SerializeField] private bool enableScaleUp;
    [SerializeField] private Vector3 scaleFrom = new Vector3(1f, 1f, 1f);
    [SerializeField] private Vector3 scaleTo = new Vector3(1.1f, 1.1f, 1.1f);
    [SerializeField] private LeanTweenType ease;
    private CanvasGroup canvasGroup;
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        scaleFrom = transform.localScale;
    }
    private void Update()
    {
    }
    private void Show()
    {
        // LeanTween.alphaCanvas(canvasGroup, 1, fadeInTime).setOnComplete( () => Debug.Log("comp[leted"));

        canvasGroup.alpha = 1;
        if (enableScaleUp)
        {
            //LeanTween.scale(gameObject, scaleTo, fadeInTime).setEase(ease);
         
        }
    }
    private void Hide()
    {
        //  LeanTween.alphaCanvas(canvasGroup, 0, fadeOutTime).setOnComplete(ResetScale);
        canvasGroup.alpha = 0;
        ResetScale();
    }
    
    private void ResetScale() => transform.localScale = scaleFrom;
    
    private void OnEnable() => Show();

    private void OnDisable() => Hide();
}
