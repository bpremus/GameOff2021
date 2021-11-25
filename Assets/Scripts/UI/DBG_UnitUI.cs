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

    CoreBug bug;
    public void Show(CoreBug cb)
    {
        if (this.transform.GetChild(0).gameObject.activeInHierarchy)
        {
            this.transform.GetChild(0).gameObject.GetComponent<UIFader>().Show();
        }
        else
            this.transform.GetChild(0).gameObject.SetActive(true);

        bug = cb;

        bug_name.text = bug.name;

    }
    public void Hide()
    {
        this.transform.GetChild(0).gameObject.SetActive(false);
        TooltipSystem.Hide();
    }
    public void FollowUnit()
    {
        FindObjectOfType<CameraController>().SetTarget(bug.transform);
        Hide();
    }
    public void Update()
    {
        if (UIController.instance.isBuildMenuActive()) Hide();
        //   if (Input.GetKey(KeyCode.A))
        //   {
        //       CellSelectProto.Instance.SetAssignBugState();
        //   }

        // Debug test of evolving bugs 
        if (Input.GetKey(KeyCode.F1))
        {
            EvolveBug(1);
        }
        if (Input.GetKey(KeyCode.F2))
        {
            EvolveBug(2);
        }
        if (Input.GetKey(KeyCode.F3))
        {
            EvolveBug(3);
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
      //  Debug.Log("evolving a bug " + bug.name);

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
                ArtPrefabsInstance.Instance.EvolveBug(bug, 3); // => to warrior to claw 
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

    }

}
