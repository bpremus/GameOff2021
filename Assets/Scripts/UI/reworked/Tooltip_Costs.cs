using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
[ExecuteInEditMode()]
public class Tooltip_Costs : MonoBehaviour
{
    public TextMeshProUGUI upgradeNameTxt;
    public TextMeshProUGUI foodCostTxt;
    public TextMeshProUGUI woodCostTxt;

    [SerializeField] private Vector3 offsetDisplay;
    Transform originPos;
    public void SetText(string upgradeName, int foodCost,int woodCost)
    {
        upgradeNameTxt.text = upgradeName;
        foodCostTxt.text = foodCost.ToString();
        woodCostTxt.text = woodCost.ToString();

        SetPosition();
        GetComponent<UIFader>().ReplayAnimation();
    }

    private void Update()
    {
       SetPosition();
    }
    private void SetPosition()
    {
        transform.position = Input.mousePosition;
    }

}
