using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class CostTooltipTrigger : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] private int id;
    private LTDescr delay;
    public string upgradeName;
    public int foodCost;
    public int woodCost;
    public void SetCorrectPrices()
    {
      int[] costs =  DBG_UnitUI.Instance.GetUpgradesCosts(id);
        foodCost = costs[0];
        woodCost = costs[1];
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        SetCorrectPrices();
        CostTooltip.Show(upgradeName, foodCost,woodCost);

    }

    public void OnPointerExit(PointerEventData eventData)
    {
      //  LeanTween.cancel(delay.uniqueId);
        CostTooltip.Hide();
    }

    void OnDisable()
    {
        LeanTween.cancel(gameObject);
    }
    public Transform GetInteractedObjectTransform() { return gameObject.transform; }
}
