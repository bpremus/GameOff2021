using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIEvolutionCost : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI evolutionName;
    [SerializeField] private TextMeshProUGUI foodCost;
    [SerializeField] private TextMeshProUGUI woodCost;

    [SerializeField] private Color canAfford;
    [SerializeField] private Color cantAfford;


    public void SetName(CoreBug.BugEvolution evolutionName)
    {
        this.evolutionName.text = Formatter_BugName.Instance.GetBugName(evolutionName).ToString();
    }
    public void SetCosts(int food,int wood,bool enoughFood,bool enoughWood)
    {

        foodCost.text = food.ToString();
        woodCost.text = wood.ToString();

        if (enoughFood) foodCost.color = canAfford;
        else foodCost.color = cantAfford;
        // --           --             --        
        if (enoughWood) woodCost.color = canAfford;
        else woodCost.color = cantAfford;


        if (!enoughFood || !enoughWood) evolutionName.color = cantAfford;
        else evolutionName.color = canAfford;
    }
}
