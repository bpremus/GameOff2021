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
        this.transform.GetChild(0).gameObject.SetActive(true);
        bug = cb;

        bug_name.text = bug.name;

    }
    public void Hide()
    {
        this.transform.GetChild(0).gameObject.SetActive(false);
    }
    public void FollowUnit()
    {
        FindObjectOfType<CameraController>().SetTarget(bug.transform);
        Hide();
    }
    public void Update()
    {

        if (UIController.instance.isBuildMenuActive()) Hide();

        if (Input.GetKey(KeyCode.A))
        {
            CellSelectProto.Instance.SetAssignBugState();
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

}
