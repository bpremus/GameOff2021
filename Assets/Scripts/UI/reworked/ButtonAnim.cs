using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class ButtonAnim : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] private Vector3 baseScale = new Vector3(0.85f,0.85f,0.85f);
    [SerializeField] private Vector3 targetScale = Vector3.one;
    [SerializeField] private float animInTime = 0.2f;
    private LTDescr anim;
    public void OnPointerEnter(PointerEventData eventData)
    {
        anim = LeanTween.scale(gameObject, targetScale, animInTime);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        LeanTween.cancel(anim.uniqueId);
        LeanTween.scale(gameObject, baseScale, animInTime/2);
    }
}
