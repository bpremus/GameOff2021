using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIEvolutionCost : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI foodCost;
    [SerializeField] private TextMeshProUGUI woodCost;

    [SerializeField] private Color canAfford;
    [SerializeField] private Color cantAfford;



    public void SetCosts(int food,int wood,bool enoughFood,bool enoughWood)
    {
        foodCost.text = food.ToString();
        woodCost.text = wood.ToString();

        if (enoughFood) foodCost.color = canAfford;
        else foodCost.color = cantAfford;
        // --           --             --        
        if (enoughWood) woodCost.color = canAfford;
        else woodCost.color = cantAfford;
    }
}
