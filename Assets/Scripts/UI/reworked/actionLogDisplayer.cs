using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class actionLogDisplayer : MonoBehaviour
{
    [SerializeField] private bool enableMovingUp;
    [SerializeField] private LeanTweenType easeType;
    [SerializeField] private Vector3 scaleVector;
    [SerializeField] private float timeToDestroy;
    [SerializeField] private float scaleTime;
    [SerializeField] private float appearTime;
    [SerializeField] private float disapearTime;
    [SerializeField] private float offsetscaleTime;
    [SerializeField] private float timeTostartFade;
    [Header("Moving up")]
    [SerializeField] private float moveTo_f;
    [SerializeField] private float moveTo_Time;
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
        if (timeToDestroy <= 0) if (this.name == "newValue(Clone)") ValueChangedDisplay.Instance.RemoveValue(this.gameObject); else ActionLogger.Instance.RemoveLog(this.gameObject);
        if (timeToDestroy <= disapearTime + timeTostartFade) FadeText();
        if (timeToDestroy < scaleTime + offsetscaleTime && scaleanim != null && transform.localScale != scaleVector) transform.localScale = scaleVector; 
        timeToDestroy -= Time.deltaTime;
    }
    private void StartAnimation()
    {
        scaleanim =  LeanTween.scale(gameObject, scaleVector, scaleTime).setEase(easeType);
        LeanTween.alphaCanvas(canvasGroup, 1, appearTime);

        if (enableMovingUp)
            LeanTween.moveLocalY(gameObject, moveTo_f, moveTo_Time).setEase(LeanTweenType.linear);
    }
    private void FadeText()
    {
        LeanTween.alphaCanvas(canvasGroup, 0, disapearTime);
    }
}
