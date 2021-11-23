using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class Room_UI : MonoBehaviour
{
    private static Room_UI _instance;
    public static Room_UI Instance
    {
        get { return _instance; }
    }

    HiveCell hiveCell;
    [SerializeField]
    GameObject bug_lis_cell;
    [SerializeField]
    GameObject bug_button_prefab;
    [SerializeField]
    TextMeshProUGUI room_name;
    [SerializeField]
    TextMeshProUGUI noBugsText;
    List<CoreBug> listed_bugs = new List<CoreBug>();
    private void Awake()
    {

        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
    }
    private void Update()
    {
        if (UIController.instance.isBuildMenuActive()) Hide();
        if (Input.GetKey(KeyCode.A))
        {
            CellSelectProto.Instance.SetAssignBugState();
        }

    }
    public void Show(HiveCell hc)
    {
        this.transform.GetChild(0).gameObject.SetActive(true);
        hiveCell = hc;
        BuildButtons();

        room_name.name = hc.GetRoom().name;

    }
    public void Hide()
    {
        this.transform.GetChild(0).gameObject.SetActive(false);
        TooltipSystem.Hide();
    }

    public void SetTask()
    {
        CoreRoom room = hiveCell.GetRoom();
        if (room)
        {
            HarversterRoom hrom = room.GetComponent<HarversterRoom>();
            if (hrom)
            {
                Debug.Log("send gathering");
                hrom.SendToCollect();
              //  OnSendGathering();
            }
        }
    }

    public void BuildButtons()
    {
        int size = bug_lis_cell.transform.childCount;
        for (int i = 0; i < size; i++)
        {
            GameObject g = bug_lis_cell.transform.GetChild(i).gameObject;
            Destroy(g);
        }

        listed_bugs.Clear();

        CoreRoom cr = hiveCell.GetRoom();
        List<CoreBug> bugs = cr.GetAssignedBugs();

        if(bugs.Count > 0)
        {
            noBugsText.gameObject.SetActive(false);
            for (int i = 0; i < bugs.Count; i++)
            {
                GameObject g = Instantiate(bug_button_prefab, bug_lis_cell.transform);
                Button b = g.GetComponent<Button>();
                UIBugButton bbc = b.GetComponent<UIBugButton>();
                bbc.bug = bugs[i]; // button will know how to renderitself
                b.onClick.AddListener(delegate { OnBugSelected(bbc); });
                listed_bugs.Add(bugs[i]);
            }
        }
        else
        {
            noBugsText.gameObject.SetActive(true);
        }



    }

    void OnBugSelected(UIBugButton bugbutton)
    {
        CellSelectProto.Instance.SetBugSelection(bugbutton.bug);
    }
    public void OnRoomDestroy()
    {

        UIController.instance.CreatePopup(0, "Are you sure?",null,gameObject);
        Hide();
       
    }
    public void DestroyRoom(bool b)
    {
        if(b)
         CellSelectProto.Instance.SetDestroyRoom(hiveCell);
    }
}
