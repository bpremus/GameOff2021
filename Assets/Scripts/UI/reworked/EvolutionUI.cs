using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EvolutionUI : MonoBehaviour
{
    [SerializeField] private GameObject superdroneUpgrade;
    [SerializeField] private GameObject warriorUpgrade;
    [SerializeField] private GameObject clawUpgrade;
    [SerializeField] private GameObject rangedUpgrade;
    [SerializeField] private GameObject ccUpgrade;
    [SerializeField] private List<GameObject> bugsButtons;
    CoreBug bug;
    [SerializeField]
    private GameObject upgradeCostPanel;
    private CoreBug.BugEvolution desiredEvol;
    public void SetBug(CoreBug bug) => this.bug = bug;


    private void Awake()
    {
        desiredEvol = CoreBug.BugEvolution.none;
        InitializeUpgradeButtons();
        OnAffordButtonActive();
    }
    private void Update()
    {
        if (!this.gameObject.activeInHierarchy) upgradeCostPanel.SetActive(false);

        if (bug != null)
        {
            ShowAvailableUpgrades();

            if (GameController.Instance.OnFoodValueChanged() || GameController.Instance.OnWoodValueChanged())
            {
                OnAffordButtonActive();
                ShowUpgradeCostsPanel();
            }
        }
        
    }

    //this for event  call
    public void ShowUpgradeCostsPanel()
    {
        upgradeCostPanel.SetActive(true);
        ShowUpgradeCost();
    }
    public void Hide()
    {
        upgradeCostPanel.SetActive(false);
    }
    public void SetUnitRestriction(List<int> restrictedUnits,bool restricted)
    {
        int t_bugindex = 0;
        List<int> t_bugsIndexesArr = new List<int>();
        foreach (GameObject evolutionButton in bugsButtons)
        {
            t_bugsIndexesArr.Add(t_bugindex);
            t_bugindex++;
        }
      
        for(int i = 0; i < bugsButtons.Count; i++)
        {
            if (restrictedUnits.Contains(t_bugsIndexesArr[i]))
            {
                bugsButtons[i].GetComponent<UIBugUpgradeButton>().SetRestriction(restricted);
                Debug.Log("restricting "+ bugsButtons[i].name);
            }
            else
            {
                bugsButtons[i].GetComponent<UIBugUpgradeButton>().SetRestriction(!restricted);
                Debug.Log("unrestricting " + bugsButtons[i].name);
            }
        }


    }
    public void InitializeUpgradeButtons()
    {

        //it needs to be in the same order!
        superdroneUpgrade.GetComponent<UIBugUpgradeButton>().SetEvolutionUI(this);
        warriorUpgrade.GetComponent<UIBugUpgradeButton>().SetEvolutionUI(this);
        clawUpgrade.GetComponent<UIBugUpgradeButton>().SetEvolutionUI(this);
        rangedUpgrade.GetComponent<UIBugUpgradeButton>().SetEvolutionUI(this);
        ccUpgrade.GetComponent<UIBugUpgradeButton>().SetEvolutionUI(this);


    }
    private void AddButtonsToList()
    {
        bugsButtons = new List<GameObject>();
        bugsButtons.Add(superdroneUpgrade);
        bugsButtons.Add(warriorUpgrade);
        bugsButtons.Add(ccUpgrade);
        bugsButtons.Add(rangedUpgrade);
        bugsButtons.Add(ccUpgrade);
    }
    public void ShowAvailableUpgrades()
    {
        if (bug.bug_evolution == CoreBug.BugEvolution.drone)
        {
            superdroneUpgrade.SetActive(true);
            warriorUpgrade.SetActive(true);

            clawUpgrade.SetActive(false);
            rangedUpgrade.SetActive(false);
            ccUpgrade.SetActive(false);

        }
        else if (bug.bug_evolution == CoreBug.BugEvolution.warrior)
        {

            clawUpgrade.SetActive(true);
            rangedUpgrade.SetActive(true);
            ccUpgrade.SetActive(true);

            warriorUpgrade.SetActive(false);
            superdroneUpgrade.SetActive(false);
        }
        else
        {
            superdroneUpgrade.SetActive(false);
            warriorUpgrade.SetActive(false);
            clawUpgrade.SetActive(false);
            rangedUpgrade.SetActive(false);
            ccUpgrade.SetActive(false);
        }



    }
    public void OnAffordButtonActive()
    {
            warriorUpgrade.GetComponent<Button>().interactable = GameController.Instance.CanAffordEvolution(CoreBug.BugEvolution.warrior);
            clawUpgrade.GetComponent<Button>().interactable = GameController.Instance.CanAffordEvolution(CoreBug.BugEvolution.claw);
            rangedUpgrade.GetComponent<Button>().interactable = GameController.Instance.CanAffordEvolution(CoreBug.BugEvolution.range);
            ccUpgrade.GetComponent<Button>().interactable = GameController.Instance.CanAffordEvolution(CoreBug.BugEvolution.cc_bug);
    }
    public void SetDesiredEvolution(CoreBug.BugEvolution desiredEvolution) => desiredEvol = desiredEvolution;
    public void UnSelectDesiredEvolution() => desiredEvol = CoreBug.BugEvolution.none;
    public void LevelUpBug()
    {
        if (bug == null) return;
        if (GameController.Instance.EvolveBug(bug.bug_evolution))
        {
            bug.LevelUp();
        }
        DBG_RoomUI.Instance.Hide();
    }


    public void ShowUpgradeCost()
    {
        if (desiredEvol == CoreBug.BugEvolution.none) return;
         if (bug)
         {
            int neededfood = GetEvolutionCost(desiredEvol)[0];
            int neededwood = GetEvolutionCost(desiredEvol)[1];
            bool enoughfood = GameController.Instance.GetFood() >= neededfood ? true : false;
            bool enoughwood = GameController.Instance.GetWood() >= neededwood ? true : false;

            upgradeCostPanel.GetComponent<UIEvolutionCost>().SetCosts(neededfood, neededwood, enoughfood, enoughwood);
            upgradeCostPanel.GetComponent<UIEvolutionCost>().SetName(desiredEvol);
         }

    }
    private int[] GetEvolutionCost(CoreBug.BugEvolution desiredEvolution)
    {
        return GameController.Instance.GetUpgradeCost(desiredEvolution);
    }
    public void EvolveTo(CoreBug.BugEvolution desiredEvolution)
    {
        if (bug == null) return;
        CoreBug.BugEvolution currentEvolution = bug.bug_evolution;

        if (currentEvolution == CoreBug.BugEvolution.drone)
        {
            if(desiredEvolution == CoreBug.BugEvolution.warrior)
            {
                if(GameController.Instance.EvolveBug(desiredEvolution))
                     ArtPrefabsInstance.Instance.EvolveToLarvaFirst(bug, CoreBug.BugEvolution.warrior);
            }
               
            else if(desiredEvolution == CoreBug.BugEvolution.super_drone)
            {
                // super-drone evolution
            }
        }
        else if(currentEvolution == CoreBug.BugEvolution.warrior)
        {
            if (desiredEvolution == CoreBug.BugEvolution.claw) 
            {
                if (GameController.Instance.EvolveBug(desiredEvolution))
                    ArtPrefabsInstance.Instance.EvolveBug(bug, 2); 
            }
            if (desiredEvolution == CoreBug.BugEvolution.range)
            {
                if (GameController.Instance.EvolveBug(desiredEvolution))
                    ArtPrefabsInstance.Instance.EvolveBug(bug, 4);
                Debug.Log("Evolving to range bug");
            }
            if (desiredEvolution == CoreBug.BugEvolution.cc_bug) 
            {
                if (GameController.Instance.EvolveBug(desiredEvolution))
                    ArtPrefabsInstance.Instance.EvolveBug(bug, 5);
            }
        }
        upgradeCostPanel.SetActive(false);
        DBG_UnitUI.Instance.Hide();
    }
    public void EvolveBug(int index)
    {
        Debug.Log("evolve " + index);

        if (bug == null) return;

        // drone => warrior
        if (bug.bug_evolution == CoreBug.BugEvolution.drone)
        {
            if (GameController.Instance.EvolveBug(CoreBug.BugEvolution.warrior))
                ArtPrefabsInstance.Instance.EvolveToLarvaFirst(bug, CoreBug.BugEvolution.warrior); // => drone to warrior bug
                                                                        // or drone to super drone 

        }

        if (bug.bug_evolution == CoreBug.BugEvolution.super_drone)
        {

        }

        // warrior => ranged, slow claw 
        else
        if (bug.bug_evolution == CoreBug.BugEvolution.warrior)
        {

            if (index == 1)
            {
                if (GameController.Instance.EvolveBug(CoreBug.BugEvolution.claw))
                    ArtPrefabsInstance.Instance.EvolveBug(bug, 2); // => to warrior to claw 
            }

            if (index == 2)
            {
                if (GameController.Instance.EvolveBug(CoreBug.BugEvolution.range))
                    ArtPrefabsInstance.Instance.EvolveBug(bug, 4); // => to warrior to ranged
            }

            if (index == 3)
            {
                if (GameController.Instance.EvolveBug(CoreBug.BugEvolution.cc_bug))
                    ArtPrefabsInstance.Instance.EvolveBug(bug, 5); // => to warrior to cc
            }

        }
        // claw, mele splash => can siege
        else
        if (bug.bug_evolution == CoreBug.BugEvolution.claw)
        {

        }
        // ranged shoot, mele splash => can siege
        else
        if (bug.bug_evolution == CoreBug.BugEvolution.range)
        {

        }
        // cc bug bug slow targets => can siege
        else
        if (bug.bug_evolution == CoreBug.BugEvolution.cc_bug)
        {

        }

        DBG_RoomUI.Instance.Hide();
    }
}
