using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
public class Card_BuildMenu : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] private int roomId;
    [SerializeField] private GameObject cantAffordOverlay;
    [SerializeField] private GameObject restrictedOverlay;
    [SerializeField] private TextMeshProUGUI woodCost;
    [SerializeField] private TextMeshProUGUI foodCost;

     [SerializeField] private bool canInteract = true;
     [SerializeField] private bool restricted = false;
    [SerializeField] private Button button;
    [Header("GFX animaitons")]
    [SerializeField]  private Vector3 scaleVector = new Vector3(1.1f,1.1f,1.1f);
    private float pointerEnterTime  = 0.2f;
    private float pointerExitTime = 0.15f;
    private LeanTweenType ease = LeanTweenType.easeSpring;

    private LTDescr scaleup;
    public bool isRestricted() { return restricted; }
    public int getID() { return roomId; }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(canInteract && !restricted)
             scaleup = LeanTween.scale(gameObject, scaleVector, pointerEnterTime).setEase(ease);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(scaleup != null)
           LeanTween.cancel(scaleup.uniqueId);
        if(canInteract && !restricted)
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
    public void SetAvailable(bool canInteract = default) 
    {
        if (this.restricted) return;
        this.canInteract = canInteract;
        button.interactable = this.canInteract;
        cantAffordOverlay.SetActive(!this.canInteract);
    }
    public void SetRestricted(bool restricted = default)
    {
        this.restricted = restricted;
        restrictedOverlay.SetActive(this.restricted);
        button.interactable = !this.restricted;
        if (!restricted)
        {
            ActionLogger.Instance.AddLog(gameObject.name + " is unlocked now!", 1);
        }
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
