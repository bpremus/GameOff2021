using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
public class Card_BuildMenu : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] private int roomId;
    [Header("Set cost")]
    [SerializeField] private TextMeshProUGUI woodCost;
    [SerializeField] private TextMeshProUGUI foodCost;



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
        SetCosts();

    }
    public void SetCosts()
    {
        int[] cost = GameController.Instance.GetRoomCost(roomId);
        if(woodCost != null && foodCost != null)
        {
            woodCost.text = cost[0].ToString();
            foodCost.text = cost[1].ToString();
        }

    }
}
