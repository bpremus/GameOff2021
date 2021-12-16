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




    private EvolutionUI evolutionUI;
    CoreBug bug;

    private void Start()
    {
        evolutionUI = levelUpPanel.GetComponent<EvolutionUI>();
    }
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
        evolutionUI.SetBug(bug);
        evolutionUI.OnAffordButtonActive();
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
                evolutionUI.LevelUpBug();
                
                return;
            }

            if (levelUpPanel.activeInHierarchy)
            {
                levelUpPanel.SetActive(false);
                
            }

            else
            {
                levelUpPanel.SetActive(true);
                
            }
               


        }
        

    }
    public void RestrictUnits(List<int> restrictedUnits,bool restricted)
    {
        evolutionUI.SetUnitRestriction(restrictedUnits, restricted);

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
        bug_name.text = Formatter_BugName.Instance.GetBugName(cb.bug_evolution);
    }
    public void SetTextCurrentState(CoreBug cb)
    {
        bugTask_Txt.text = Formatter_BugName.Instance.GetBugTask(cb.bugTask);
    }
    public void Update()
    {
        if (UIController.instance.isBuildMenuActive()) Hide();
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




}
