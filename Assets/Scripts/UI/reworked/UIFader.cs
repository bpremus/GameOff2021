using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UIFader : MonoBehaviour
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
    private LTDescr scaling = null;
    private LTDescr fading = null;
    private float timeActive;
    [SerializeField] private bool ignoredepends = true;
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        scaleFrom = transform.localScale;
    }
    private void Update()
    {
        CorrectDisplaying();
    }

    private void CorrectDisplaying()
    {
        if(timeActive>fadeInTime)
        {
            if (canvasGroup.alpha < 1) canvasGroup.alpha = 1;
        }

        if (gameObject.activeInHierarchy)
            timeActive += Time.deltaTime;
    }

    public void Show()
    {
        timeActive = 0;
        if (!ignoredepends)
        {
          
            if (transform.localScale.x > scaleFrom.x)
            {
                ResetScale();
            }
          

            if (fading != null)
            {
                LeanTween.cancel(fading.uniqueId);
                fading = null;
            }

        }
        canvasGroup.alpha = 0;
        fading = LeanTween.alphaCanvas(canvasGroup, 1, fadeInTime);
        fading.setOnComplete(() => canvasGroup.alpha = 1);

        if (enableScaleUp)
        {
           scaling = LeanTween.scale(gameObject, scaleTo, fadeInTime).setEase(ease);
        }
    }
    public void ReplayAnimation()
    {
        OnReplayHide();
    }
    private void OnReplayHide()
    {
        canvasGroup.alpha = 0;

        Show();

    }
    public void Hide()
    {
     fading = LeanTween.alphaCanvas(canvasGroup, 0, fadeOutTime).setOnComplete(() =>ResetScale());
    }
    private void ResetScale()
    {
        if (scaling != null)
        {
            LeanTween.cancel(scaling.uniqueId);
            transform.localScale = scaleFrom;
        }

    }
    private void OnEnable() => Show();
    private void OnDisable() => Hide();
}
