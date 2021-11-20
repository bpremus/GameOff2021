using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIPopup : MonoBehaviour
{
    [SerializeField] private GameObject popupWindow;
    [Header("Content")]
    [SerializeField] private TextMeshProUGUI headerText;
    [SerializeField] private TextMeshProUGUI contentText;
    [Header("Animations")]
    [SerializeField] private bool enableAnimations = true;
    [SerializeField] private float scaleTime = 0.35f;
    [SerializeField] private float fadeTime = 0.1f;
    [SerializeField] private LeanTweenType ease = LeanTweenType.easeSpring;

    private PopupsHandler popupsHandler;
    private CanvasGroup canvasGroup;
    public enum AnimType
    {
        Fade,
        Scale,
        Both
    }
    public AnimType onEnableAnim = AnimType.Scale;
    public AnimType OnDisableAnim = AnimType.Fade;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        popupsHandler = FindObjectOfType<PopupsHandler>();
    }
    [ContextMenu("Show window")]
    public void ShowWindow()
    {


        if (enableAnimations)
        {


            if (onEnableAnim == AnimType.Scale)
            {
                popupWindow.transform.localScale = Vector3.zero;
                canvasGroup.alpha = 1;


                LeanTween.scale(popupWindow, new Vector3(1, 1, 1), scaleTime).setEase(ease);
            }
            else if (onEnableAnim == AnimType.Fade && canvasGroup)
            {
                canvasGroup.alpha = 0;
                popupWindow.transform.localScale = Vector3.one;
                LeanTween.alphaCanvas(canvasGroup, 1, fadeTime).setEase(ease);
            }
            else if (onEnableAnim == AnimType.Both)
            {
                canvasGroup.alpha = 0;
                popupWindow.transform.localScale = Vector3.zero;

                LeanTween.scale(popupWindow, new Vector3(1, 1, 1), scaleTime).setEase(ease).setEase(ease);
                LeanTween.alphaCanvas(canvasGroup, 1, fadeTime);
            }
        }


    }
    [ContextMenu("Hide window")]
    public void HideWindow()
    {
        if (enableAnimations)
        {
            if (OnDisableAnim == AnimType.Scale)
            {
                popupWindow.transform.localScale = Vector3.one;

                // canvasGroup.alpha = 0;
                LeanTween.scale(popupWindow, Vector3.zero, scaleTime).setEase(ease).setOnComplete(DestroySelf);
            }
            else if (OnDisableAnim == AnimType.Fade && canvasGroup)
            {
                canvasGroup.alpha = 1;
                popupWindow.transform.localScale = Vector3.one;

                LeanTween.alphaCanvas(canvasGroup, 0, fadeTime).setOnComplete(DestroySelf);
            }
            else if(OnDisableAnim == AnimType.Both)
            {
                canvasGroup.alpha = 1;
                popupWindow.transform.localScale = Vector3.one;

                LeanTween.scale(popupWindow, Vector3.zero, scaleTime).setEase(ease).setOnComplete(DestroySelf);
                LeanTween.alphaCanvas(canvasGroup, 0, fadeTime);
            }
        }
    }



    public void SetHeaderText(string text)
    {
        if (headerText != null) headerText.text = text;
    }
    public void SetContentText(string text)
    {
        if (contentText != null) contentText.text = text;
    }

    private void OnEnable()
    {
        ShowWindow();
    }
    void OnDisable()
    {
        HideWindow();
    }
    public void DestroySelf()
    {
        if (popupsHandler)
        {
            popupsHandler.DeleteCurrentPopup();
        }
        Destroy(gameObject);
    }
    public void ReturnFalse()
    {
        popupsHandler.GetChoice(false);
        HideWindow();
    }
    public void ReturnTrue()
    {
        popupsHandler.GetChoice(true);
        HideWindow();
    }
}
