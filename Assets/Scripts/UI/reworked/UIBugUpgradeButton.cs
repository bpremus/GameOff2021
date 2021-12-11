using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class UIBugUpgradeButton : MonoBehaviour
{
    private EvolutionUI evolutionUI;
    [SerializeField] private bool restricted;
    [SerializeField] private GameObject restrictedPanel;
    [SerializeField] private CoreBug.BugEvolution evolveTo;

    public bool isRestricted() { return restricted; }
    public void SetEvolutionUI(EvolutionUI evolutionUI)
    {
        this.evolutionUI = evolutionUI;
    }
    public void SetRestriction(bool restricted)
    {
        this.restricted = restricted;
        restrictedPanel.SetActive(this.restricted);
        if(!restricted)
        {
            ActionLogger.Instance.AddLog(Formatter_BugName.Instance.GetBugName(evolveTo) + " evolution is unlocked now!", 1);
        }
            
    }
    public CoreBug.BugEvolution getDesiredEvolution()
    {
        return evolveTo;
    }
    public void Activate()
    {
        if (restricted) return;
        evolutionUI.SetDesiredEvolution(evolveTo);
        evolutionUI.ShowUpgradeCostsPanel();
        evolutionUI.OnAffordButtonActive();
    }
    public void Deactivate()
    {
        evolutionUI.UnSelectDesiredEvolution();
        evolutionUI.Hide();
    }
    public void Clicked()
    {
        if (restricted) return;
        if (gameObject.GetComponent<Button>().interactable)
            evolutionUI.EvolveTo(evolveTo);
    }
}
