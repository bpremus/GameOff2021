using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CostTooltip : MonoBehaviour
{
    private static CostTooltip currentcostTooltip;
    [SerializeField] private Tooltip_Costs tooltipCosts;

    private void Awake()
    {
        currentcostTooltip = this;
    }
    private void OnEnable()
    {
        transform.position = Input.mousePosition + new Vector3(0, 20, 0);
    }
    public static void Show(string upgradeName,int foodCost,int woodCost)
    {
      //  Debug.Log(upgradeName+" " +foodCost+" " +woodCost);
        currentcostTooltip.tooltipCosts.SetText(upgradeName, foodCost, woodCost);
        currentcostTooltip.tooltipCosts.gameObject.SetActive(true);

    }
    public static void Hide()
    {
        currentcostTooltip.tooltipCosts.gameObject.SetActive(false);

    }


}
