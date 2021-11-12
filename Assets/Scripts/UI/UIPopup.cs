using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIPopup : MonoBehaviour
{
    [Header("Content")]
    [SerializeField] private TextMeshProUGUI headerText;
    [SerializeField] private TextMeshProUGUI contentText;
    [Header("Animations")]
    [SerializeField] private bool enableAnimations = true;
    [SerializeField] private float scaleTime = 0.35f;
    [SerializeField] private float fadeTime = 0.1f;
    [SerializeField] private LeanTweenType ease = LeanTweenType.easeSpring;

    private PopupController popupController;
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
        popupController = FindObjectOfType<PopupController>();
    }
    [ContextMenu("Show window")]
    public void ShowWindow()
    {


        if (enableAnimations)
        {


            if (onEnableAnim == AnimType.Scale)
            {
                LeanTween.scale(gameObject, Vector3.zero, 0.01f);
                canvasGroup.alpha = 1;


                LeanTween.scale(gameObject, new Vector3(1, 1, 1), scaleTime).setEase(ease);
            }
            else if (onEnableAnim == AnimType.Fade && canvasGroup)
            {
                canvasGroup.alpha = 0;
                LeanTween.scale(gameObject, new Vector3(1, 1, 1),0.01f);

                LeanTween.alphaCanvas(canvasGroup, 1, fadeTime).setEase(ease);
            }
            else if (onEnableAnim == AnimType.Both)
            {
                canvasGroup.alpha = 0;
                LeanTween.scale(gameObject, Vector3.zero, 0.01f);

                LeanTween.scale(gameObject, new Vector3(1, 1, 1), scaleTime).setEase(ease).setEase(ease);
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
                LeanTween.scale(gameObject, new Vector3(1, 1, 1), 0.01f);

                // canvasGroup.alpha = 0;
                LeanTween.scale(gameObject, Vector3.zero, scaleTime).setEase(ease).setOnComplete(DestroySelf);
            }
            else if (OnDisableAnim == AnimType.Fade && canvasGroup)
            {
                canvasGroup.alpha = 1;
                LeanTween.scale(gameObject, new Vector3(1, 1, 1), 0.01f);

                LeanTween.alphaCanvas(canvasGroup, 0, fadeTime).setOnComplete(DestroySelf);
            }
            else if(OnDisableAnim == AnimType.Both)
            {
                canvasGroup.alpha = 1;
                LeanTween.scale(gameObject, new Vector3(1, 1, 1), 0.01f);

                LeanTween.scale(gameObject, Vector3.zero, scaleTime).setEase(ease).setOnComplete(DestroySelf);
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
        if (popupController)
        {
            popupController.DeleteCurrentPopup();
        }
        Destroy(gameObject);
    }

}
