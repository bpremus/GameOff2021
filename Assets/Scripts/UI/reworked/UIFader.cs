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
    [SerializeField] private Vector3 scaleTo = new Vector3(1.1f, 1.1f, 1.1f);
    [SerializeField] private LeanTweenType ease;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        canvasGroup.alpha = 0;
        LeanTween.alphaCanvas(canvasGroup, 1, fadeInTime);
        if (enableScaleUp)
        {
            LeanTween.scale(gameObject, scaleTo, fadeInTime).setEase(ease);
        }
    }
    private void OnDisable()
    {
       
        LeanTween.alphaCanvas(canvasGroup, 0, fadeOutTime).setOnComplete(() => transform.localScale = Vector3.one);
    }
}
