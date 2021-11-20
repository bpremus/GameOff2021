using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Card_BuildMenu : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{

  [SerializeField]  private Vector3 scaleVector = new Vector3(1.1f,1.1f,1.1f);
    private float pointerEnterTime  = 0.2f;
    private float pointerExitTime = 0.15f;
    private LeanTweenType ease = LeanTweenType.easeSpring;

    private LTDescr scaleup;
    public void OnPointerEnter(PointerEventData eventData)
    {
     scaleup = LeanTween.scale(gameObject, scaleVector, pointerEnterTime).setEase(ease);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        LeanTween.cancel(scaleup.uniqueId);
        LeanTween.scale(gameObject, Vector3.one, pointerExitTime);  
    }
    void OnDisable()
    {
        LeanTween.scale(gameObject, Vector3.one, pointerExitTime);
    }
    void OnEnable()
    {
        LeanTween.scale(gameObject, Vector3.one, pointerExitTime);
    }
}
