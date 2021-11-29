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
    TextMeshProUGUI room_level;
    [SerializeField]
    GameObject assignedBugsHolder;


    [SerializeField] private GameObject[] customroomButtons;
    // - -  -   -  - - - - - -  -  - - - - -  - - - - 
    [Header("Header color for different room types")]
    [SerializeField] private bool disableColorChange;
    [SerializeField] private GameObject currentRoomHeader;

    [SerializeField] private Color corridorColor;
    [SerializeField] private Color barracksColor;
    [SerializeField] private Color storageColor;
    [SerializeField] private Color harvesterColor;
    [SerializeField] private Color queenColor;
    [SerializeField] private Color hiveColor;
    // - - - - -- - - - - -  - - - -  - --  - -
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

        if (!this.transform.GetChild(0).gameObject.activeInHierarchy) assignedBugsHolder.SetActive(false);
    }
    public void Show(HiveCell hc)
    {
        if (this.transform.GetChild(0).gameObject.activeInHierarchy)
        {
            this.transform.GetChild(0).gameObject.GetComponent<UIFader>().Show();
        }
        else
             this.transform.GetChild(0).gameObject.SetActive(true);

        hiveCell = hc;
        BuildButtons();
        InitializeButtons();
        SetRoomNameText(hc);
        SetHeaderColor();
        SetLevelText(hc);


    }
    public void Hide()
    {
        this.transform.GetChild(0).gameObject.SetActive(false);
        TooltipSystem.Hide();
    }
    public void SetRoomNameText(HiveCell hc)
    {

        string text = hc.GetRoom().name;
       // Debug.Log(text);
        if (text == "HiveCorridor(Clone)") text = "Corridor";
        else if (text == "SalvageRoom(Clone)") text = "Storage";
        else if(text == "WarRoom(Clone)") text = "Barracks";
        else if(text == "HarversterRoom(Clone)") text = "Harvester";
        else if(text == "QueenRoom(Clone)") text = "Queen room";
        else if(text == "HiveRoom(Clone)") text = "Main Hive";
        else text = "Room";
        room_name.text = text;
  
    }
    public void SetLevelText(HiveCell hc)
    {
        room_level.text = "Level " + 1;
    }

    public void SetHeaderColor()
    {
        Color currentColor;
        if (disableColorChange) { currentRoomHeader.GetComponent<Image>().color = corridorColor;return; }
        if (currentRoomHeader == null) return;

       
        switch (room_name.text)
        {
            case "Corridor":
                currentColor = corridorColor;
                break;
            case "Storage":
                currentColor = storageColor;
                break;
            case "Barracks":
                currentColor = barracksColor;
                break;
            case "Harvester":
                currentColor = harvesterColor;
                break;
            case "Queen room":
                currentColor = queenColor;
                break;
            case "Main Hive":
                currentColor = hiveColor;
                break;

            default:
                currentColor = corridorColor;
                break;
        }

        currentRoomHeader.GetComponent<Image>().color = currentColor; 
    }
    public void SetTask()
    {
        Debug.Log("start task");
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

            QueenRoom qroom = room.GetComponent<QueenRoom>();
            if (qroom)
            {
                // we can gather from here also 
                Debug.Log("send gathering");
                qroom.SendToCollect();
            }

            CommandCenter ccroom = room.GetComponent<CommandCenter>();
            if (ccroom)
            {
                // we can gather from here also 
                Debug.Log("send pillage");
                ccroom.SendToCollect();
            }
            HiveCorridor hcorridor = room.GetComponent<HiveCorridor>();
            if (hcorridor)
            {
                hcorridor.UnsiegeBug();
            }

        }
    }

    public void UnsetTask()
    {
        Debug.Log("cancel task");
        CoreRoom room = hiveCell.GetRoom();
        if (room)
        {
            HarversterRoom hrom = room.GetComponent<HarversterRoom>();
            if (hrom)
            {
                hrom.RecallBugs();
                return;
            }

            QueenRoom qroom = room.GetComponent<QueenRoom>();
            if (qroom)
            {
                qroom.RecallBugs();
                return;
            }

            CommandCenter ccroom = room.GetComponent<CommandCenter>();
            if (ccroom)
            {
                ccroom.RecallBugs();
                return;
            }

            WarRoom wroom = room.GetComponent<WarRoom>();
            if (wroom)
            {
                wroom.RecallBugs();
                return;
            }

            SalvageRoom srom = room.GetComponent<SalvageRoom>();
            if (srom)
            {
                srom.RecallBugs();
            }

            HiveCorridor hcorridor = room.GetComponent<HiveCorridor>();
            if (hcorridor)
            {
                hcorridor.SiegeBug();
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
            assignedBugsHolder.SetActive(true);
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
            assignedBugsHolder.SetActive(false);
        }



    }
    public void InitializeButtons()
    {
        CoreRoom room = hiveCell.GetRoom();
        if (room)
        {
            ResetCustomButtons();
            if (room.GetComponent<HiveCorridor>())
            {
                customroomButtons[0].SetActive(true);
                customroomButtons[1].SetActive(true);
            }
            else if (room.GetComponent<HarversterRoom>())
            {
                customroomButtons[2].SetActive(true);
                customroomButtons[3].SetActive(true);
            }
            else if (room.GetComponent<SalvageRoom>())
            {
                customroomButtons[4].SetActive(true);
                customroomButtons[5].SetActive(true);
            }
            
            else if (room.GetComponent<WarRoom>())
            {
                customroomButtons[6].SetActive(true);
                customroomButtons[7].SetActive(true);
            }

            else if (room.GetComponent<QueenRoom>())
            {
                customroomButtons[8].SetActive(true);
                customroomButtons[9].SetActive(true);
            }
            else if (room.GetComponent<HiveRoom>())
            {
                customroomButtons[10].SetActive(true);
                customroomButtons[11].SetActive(true);
            }
        }
    }
    public void ResetCustomButtons()
    {
        foreach(GameObject customButton in customroomButtons)
        {
            customButton.SetActive(false);
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
         CellSelectProto.Instance.DestroyCell(hiveCell);
    }

}
