using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ObjectiveDisplay : MonoBehaviour
{


    #region Instance
    private static ObjectiveDisplay _instance;
    public static ObjectiveDisplay Instance
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


        currentObjectivesList = new List<string>();
        activeObjectives = new List<GameObject>();
    }
    #endregion 
    [SerializeField] private GameObject showHideButton;
    [SerializeField] private GameObject objectiveDisplay;
    [SerializeField] private GameObject normalObjectiveHolder;
    [SerializeField] private GameObject BigObjectiveHolder;
    [SerializeField] private Transform parent;
    [SerializeField]
    private List<string> currentObjectivesList;
    [SerializeField]
    private List<GameObject> activeObjectives;
    [Header("Icons swap")]
    [SerializeField] private Sprite hideIcon;
    [SerializeField] private Sprite showIcon;
    public bool isObjectivesDisplayed() { return objectiveDisplay.activeInHierarchy; }


    public void ToggleObjectiveDisplayPanel()
    {
        if (isObjectivesDisplayed()) HideObjectivesPanel();
        else DisplayObjectivesPanel();
    }


    //0 normal 1 bigText
    public void AddToObjectiveList(string objectiveText,int type)
    {
        if (currentObjectivesList.Contains(objectiveText)) return;

        currentObjectivesList.Add(objectiveText);
        DisplayObjective(type, objectiveText);

    }
    public void RemoveFromObjectiveList(string objectiveText)
    {
        if (currentObjectivesList.Contains(objectiveText))
        {
            ObjectiveCompleted(objectiveText);
            currentObjectivesList.Remove(objectiveText);

        }
    }
    public void ObjectiveCompleted(string objectiveText)
    {
        if (currentObjectivesList.Contains(objectiveText))
        {
            Destroy(activeObjectives[currentObjectivesList.IndexOf(objectiveText)]);
            activeObjectives.Remove(activeObjectives[currentObjectivesList.IndexOf(objectiveText)]);
        }
    }
    public void DisplayObjective(int type,string text)
    {
        GameObject objective;
        if (type == 0) objective = normalObjectiveHolder;
        else if (type == 1) objective = BigObjectiveHolder;
        else objective = normalObjectiveHolder;

        GameObject obj = Instantiate(objective, parent);
        activeObjectives.Add(obj);
        obj.GetComponentInChildren<TextMeshProUGUI>().text = "> "+text;
    }
    public void AllCompleted()
    {
        for(int i =0;i< currentObjectivesList.Count; i++)
        {
            currentObjectivesList.Remove(currentObjectivesList[i]);
            Destroy(activeObjectives[i].gameObject);
        }
        objectiveDisplay.SetActive(false);
    }

    public void DisplayObjectivesPanel()
    {
        showHideButton.GetComponentInChildren<Image>().sprite = hideIcon;
    }
    public void HideObjectivesPanel()
    {
        
        showHideButton.GetComponentInChildren<Image>().sprite = showIcon;
    }
    private void Update()
    {
        
    }
}
