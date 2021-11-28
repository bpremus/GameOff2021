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
    TextMeshProUGUI bug_level;
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
        SetTextBugLevel(cb);

    }
    public void ShowLevelUpPanel()
    {
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





        //Here should be the formatting of names





        bug_name.text = cb.name;
    }
    public void SetTextBugLevel(CoreBug cb)
    {
        bug_level.text = "Level "+ cb.bug_base_level.ToString();
    }
    public void Update()
    {
        if (UIController.instance.isBuildMenuActive()) Hide();



        if(bug != null)
        {
            if (bug.bug_evolution == CoreBug.BugEvolution.warrior)
            {
                warriorUpgrade.GetComponent<Button>().interactable = false;
                clawUpgrade.GetComponent<Button>().interactable = true;
                rangedUpgrade.GetComponent<Button>().interactable = true;
                ccUpgrade.GetComponent<Button>().interactable = true;
            }
            else
            {
                warriorUpgrade.GetComponent<Button>().interactable = true;
                clawUpgrade.GetComponent<Button>().interactable = false;
                rangedUpgrade.GetComponent<Button>().interactable = false;
                ccUpgrade.GetComponent<Button>().interactable = false;
            }

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

    public void EvolveBug()
    {
        ArtPrefabsInstance.Instance.EvolveBug(bug,0);
        Hide();
    }
    public void EvolveBug(int index)
    {

        if (bug == null) return;

        // drone => warrior
        if (bug.bug_evolution == CoreBug.BugEvolution.drone)
        {
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
                ArtPrefabsInstance.Instance.EvolveBug(bug, 2); // => to warrior to claw 
            if (index == 2)
                ArtPrefabsInstance.Instance.EvolveBug(bug, 5); // => to warrior to ranged
            if (index == 3)
                ArtPrefabsInstance.Instance.EvolveBug(bug, 4); // => to warrior to cc

           
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
