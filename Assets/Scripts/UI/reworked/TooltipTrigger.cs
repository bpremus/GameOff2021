using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static LTDescr delay;
    public string content;
    public string header;
    public void OnPointerEnter(PointerEventData eventData)
    {
        UIBugUpgradeButton uIBugUpgradeButton = GetComponent<UIBugUpgradeButton>();
        if (uIBugUpgradeButton != null)
        {
            if(uIBugUpgradeButton.isRestricted())
                return;
        }

       delay = LeanTween.delayedCall(0.3f, () => 
        { 
            TooltipSystem.Show(content, header);
        });
      
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        LeanTween.cancel(delay.uniqueId);
        TooltipSystem.Hide();
    }

    public void OnMouseEnter()
    {
        delay = LeanTween.delayedCall(0.3f, () =>
        {
            TooltipSystem.Show(content, header);
        });
    }
    public void OnMouseExit()
    {
        LeanTween.cancel(delay.uniqueId);
        TooltipSystem.Hide();
    }
    void OnDisable()
    {
        if(delay != null)
         LeanTween.cancel(delay.uniqueId);
    }
}
