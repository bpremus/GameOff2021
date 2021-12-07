using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DBG_UnitUI : MonoBehaviour
{
    // changed to singleton 
    private static DBG_UnitUI _instance;
    public static DBG_UnitUI Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {

        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
    }

    [SerializeField]
    TextMeshProUGUI bug_name;
    [SerializeField]
    TextMeshProUGUI bugTask_Txt;
    [SerializeField]
    private GameObject levelUpPanel;

    [SerializeField] private GameObject warriorUpgrade;
    [SerializeField] private GameObject clawUpgrade;
    [SerializeField] private GameObject rangedUpgrade;
    [SerializeField] private GameObject ccUpgrade;

    CoreBug bug;
    public void Show(CoreBug cb)
    {
        if (this.transform.GetChild(0).gameObject.activeInHierarchy)
        {
            this.transform.GetChild(0).gameObject.GetComponent<UIFader>().Show();
            levelUpPanel.SetActive(false);
        }
        else
            this.transform.GetChild(0).gameObject.SetActive(true);

        bug = cb;

        SetTextBugName(cb);
        SetTextCurrentState(cb);

    }
    public void ShowLevelUpPanel()
    {      
        if (bug)
        {
            if (GameController.Instance.IsLevelUpOnly(bug))
            {
                levelUpPanel.SetActive(false);

                LevelUpBug();
                return;
            }
        }
        
        if(levelUpPanel.activeInHierarchy)
            levelUpPanel.SetActive(false);
        else
            levelUpPanel.SetActive(true);
    }
    public void Hide()
    {
        this.transform.GetChild(0).gameObject.SetActive(false);
        levelUpPanel.SetActive(false);
        CellSelectProto.Instance.HideBugSelector();
        TooltipSystem.Hide();
    }
    public void FollowUnit()
    {
        FindObjectOfType<CameraController>().SetTarget(bug.transform);
        Hide();
    }
    public void SetTextBugName(CoreBug cb)
    {
        string bugName = "Bug";
        if (cb.bug_evolution == CoreBug.BugEvolution.drone) bugName = "Worker";
        else if (cb.bug_evolution == CoreBug.BugEvolution.warrior) bugName = "Warrior";
        else if (cb.bug_evolution == CoreBug.BugEvolution.claw) bugName = "Claw";
        else if (cb.bug_evolution == CoreBug.BugEvolution.range) bugName = "Range";
        else if (cb.bug_evolution == CoreBug.BugEvolution.cc_bug) bugName = "Spike bug";
        else
             bugName = cb.bug_evolution.ToString();
        bug_name.text = bugName;
    }
    public void SetTextCurrentState(CoreBug cb)
    {
        string task;
        if (cb.bugTask == CoreBug.BugTask.none) task = "Idle";
        else if (cb.bugTask == CoreBug.BugTask.fight) task = "Defending";
        else if (cb.bugTask == CoreBug.BugTask.harvesting) task = "Harvesting";
        else if (cb.bugTask == CoreBug.BugTask.salvage) task = "Salvaging";
        else
            task = cb.bugTask.ToString();
        bugTask_Txt.text = task;
    }
    public void Update()
    {
        if (UIController.instance.isBuildMenuActive()) Hide();


        if(bug != null)
        {
            if (bug.bug_evolution == CoreBug.BugEvolution.drone)
            {
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
               
            }
            else
            {
                warriorUpgrade.SetActive(false);
                clawUpgrade.SetActive(false);
                rangedUpgrade.SetActive(false);
                ccUpgrade.SetActive(false);
            }
            OnAffordButtonActive();
        }


    }

    public void SelectRoomFromBug()
    {
        HiveCell cell = bug.asigned_cell;
        CellSelectProto.Instance.SetRoomSelection(cell);
    }

    public void AssignBug()
    {
        // we have a bug selected 
        // we need to trigger bug path
        CellSelectProto.Instance.SetAssignBugState();


    }



    public void LevelUpBug()
    {
        if (bug == null) return;
        if (GameController.Instance.EvolveBug(bug.bug_evolution))
        {
            bug.LevelUp();
        }
    }
    public void OnAffordButtonActive()
    {
        warriorUpgrade.GetComponent<Button>().interactable = GameController.Instance.CanAffordUpgrade(CoreBug.BugEvolution.warrior);
        clawUpgrade.GetComponent<Button>().interactable = GameController.Instance.CanAffordUpgrade(CoreBug.BugEvolution.claw);
        rangedUpgrade.GetComponent<Button>().interactable = GameController.Instance.CanAffordUpgrade(CoreBug.BugEvolution.range);
        ccUpgrade.GetComponent<Button>().interactable = GameController.Instance.CanAffordUpgrade(CoreBug.BugEvolution.cc_bug);
    }
    public void EvolveBug(int index)
    {
        Debug.Log("evolve " + index);

        if (bug == null) return;

        // drone => warrior
        if (bug.bug_evolution == CoreBug.BugEvolution.drone)
        {
            if (GameController.Instance.EvolveBug(CoreBug.BugEvolution.warrior))
                ArtPrefabsInstance.Instance.EvolveBug(bug, 3); // => drone to warrior bug
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


        Hide();
    }

}
