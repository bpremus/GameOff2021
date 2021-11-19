using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DBG_RoomUI : MonoBehaviour
{
    // changed to singleton 
    private static DBG_RoomUI _instance;
    public static DBG_RoomUI Instance
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

    HiveCell hiveCell;
    [SerializeField]
    GameObject bug_lis_cell;
    [SerializeField]
    GameObject bug_button_prefab;
    [SerializeField]
    TextMeshProUGUI room_name;

    List<CoreBug> listed_bugs = new List<CoreBug>();
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

    void OnBugSelected(UIBugButton bugbutton)
    {
        CellSelectProto.Instance.SetBugSelection(bugbutton.bug);
    }
   


    public void Update()
    {
        if (Input.GetKeyDown("1"))
        {
            if (listed_bugs.Count > 0)
            {
                CellSelectProto.Instance.SetBugSelection(listed_bugs[0]);
            }
        }
        if (Input.GetKeyDown("2"))
        {
            if (listed_bugs.Count > 1)
            {
                CellSelectProto.Instance.SetBugSelection(listed_bugs[1]);
            }
        }
        if (Input.GetKeyDown("3"))
        {
            if (listed_bugs.Count > 2)
            {
                CellSelectProto.Instance.SetBugSelection(listed_bugs[2]);
            }
        }

    }

    public void OnSendGathering()
    { 
    
    }

    public void OnRoomDestroy()
    {
        CellSelectProto.Instance.SetDestroyRoom(hiveCell);
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
    }

    public void SetTaks()
    {
        CoreRoom room = hiveCell.GetRoom();
        if (room)
        {
            HarversterRoom hrom = room.GetComponent<HarversterRoom>();
            if (hrom)
            {
                Debug.Log("send gathering");
                hrom.SendToCollect();
                OnSendGathering();
            }
        }
    }

}
