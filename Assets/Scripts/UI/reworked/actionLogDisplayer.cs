using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class actionLogDisplayer : MonoBehaviour
{
    [SerializeField] private LeanTweenType easeType;
    [SerializeField] private Vector3 scaleVector;
    [SerializeField] private float timeToDestroy;
    [SerializeField] private float scaleTime;
    [SerializeField] private float appearTime;
    [SerializeField] private float disapearTime;

    private LTDescr scaleanim;
    private CanvasGroup canvasGroup;
    private Vector3 baseScale;
    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        baseScale = transform.localScale;
        StartAnimation();
    }
    private void Update()
    {
        if (timeToDestroy <= 0) ActionLogger.Instance.RemoveLog(this.gameObject);
        if (timeToDestroy <= disapearTime + 1) FadeText();
        if (timeToDestroy < scaleTime + 0.15f && scaleanim != null && transform.localScale != scaleVector) transform.localScale = scaleVector; 
        timeToDestroy -= Time.deltaTime;
    }
    private void StartAnimation()
    {
        scaleanim =  LeanTween.scale(gameObject, scaleVector, scaleTime).setEase(easeType);
        LeanTween.alphaCanvas(canvasGroup, 1, appearTime);
    }
    private void FadeText()
    {
        LeanTween.alphaCanvas(canvasGroup, 0, disapearTime);
    }
}
