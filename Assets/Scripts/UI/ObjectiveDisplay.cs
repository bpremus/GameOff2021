using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ObjectiveDisplay : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject objectiveDisplay;
    [SerializeField] private GameObject normalObjectiveHolder;
    [SerializeField] private TextMeshProUGUI tasksHeader;
    [SerializeField] private Transform parent;


    [SerializeField]
    private List<string> objectivesDescriptions;
    [SerializeField]
    private List<GameObject> activeObjectives;
    [Header("Objective Completed")]
    [SerializeField] private Sprite notCompletedSprite;
    [SerializeField] private Sprite completedSprite;
    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private Color completedColor = Color.green;
    [SerializeField] private Color[] customDescriptionColor;

    [Header("Animations")]
    private CanvasGroup canvasGroup;
    [SerializeField] private float animTime = 0.5f;
    [SerializeField] private float objectiveAnimTime = 0.5f;
    [SerializeField] private LeanTweenType animEase = LeanTweenType.easeSpring;
    #endregion
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


        objectivesDescriptions = new List<string>();
        activeObjectives = new List<GameObject>();
        canvasGroup = objectiveDisplay.GetComponent<CanvasGroup>();
    }
    #endregion
    public bool areObjectivesDisplayed() { return objectiveDisplay.activeInHierarchy; }


    private void Update()
    {
        if (activeObjectives.Count > 0)
        {
            DisplayObjectivesPanel();
        }
        else HideObjectivesPanel();
    }
    [ContextMenu("Toggle panel")]
    public void ToggleObjectiveDisplayPanel()
    {
        Debug.Log("Toggling");
        if (areObjectivesDisplayed()) HideObjectivesPanel();
        else DisplayObjectivesPanel();
    }


    public bool objectiveExist(string description)
    {
        if (objectivesDescriptions.Contains(description)) return true;
        return false;
    }
    public void SetTaskHeader(string text = default)
    {
        if (text == "") text = "Objectives";
        tasksHeader.text = text;
    }
    public void AddObjective(string objectiveText,int importanceLevel = default)
    {
        if (objectiveExist(objectiveText)) return;

        GameObject obj = Instantiate(normalObjectiveHolder, parent);
        activeObjectives.Add(obj);
        string description = obj.GetComponentInChildren<TextMeshProUGUI>().text = objectiveText;
        objectivesDescriptions.Add(description);
        SetObjectiveStateGFX(obj, false, importanceLevel);

        //animation
        StartCoroutine(FadeInObjective(obj));

        //
    }
    private IEnumerator FadeInObjective(GameObject obj)
    {
        yield return new WaitForSeconds(objectivesDescriptions.Count - (objectivesDescriptions.Count * 0.7f));
        obj.GetComponent<CanvasGroup>().alpha = 0;
        LeanTween.alphaCanvas(obj.GetComponent<CanvasGroup>(), 1, objectiveAnimTime);
    }
    private void SetObjectiveStateGFX(GameObject objective,bool completed,int importanceLevel = default)
    {
        //visual changes when 

        //completed task
        if (completed)
        {
            objective.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = completedSprite;
            objective.gameObject.transform.GetChild(0).GetComponent<Image>().color = completedColor;
            objective.gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Strikethrough;
        }
        else //not completed
        {
            objective.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = notCompletedSprite;
            if (importanceLevel != 0)
            {
                if(importanceLevel < customDescriptionColor.Length)
                    objective.gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = customDescriptionColor[importanceLevel];
                else objective.gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = customDescriptionColor[0];
            }
            else objective.gameObject.transform.GetChild(0).GetComponent<Image>().color = defaultColor;

            objective.gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Normal;
        }
    }
    public void ObjectiveCompleted(string objectiveText)
    {
        GameObject objective = FindObjectiveFromString(objectiveText);
        if (objective != null)
        {
            SetObjectiveStateGFX(objective, true);
        }
    }

    private void DestroyObjective(GameObject objective)
    {
        string objectiveDesc = objective.GetComponentInChildren<TextMeshProUGUI>().text;
        //double check?
        if (objectiveExist(objectiveDesc))
        {
            Destroy(objective.gameObject);
            activeObjectives.Remove(objective);
            objectivesDescriptions.Remove(objectiveDesc);
            Debug.Log("Objective destroyed sucessfully");
        }
    }
    public void AllCompleted()
    {
        foreach(GameObject objective in activeObjectives)
        {
            DestroyObjective(objective);
        }
  //      for(int i = 0; i < activeObjectives.Count; i++)
   //     {
 //          DestroyObjective(activeObjectives[i].gameObject);
  //      }
        activeObjectives.Clear();
        objectivesDescriptions.Clear();
    }

    public void DisplayObjectivesPanel()
    {
        if (!objectiveDisplay.activeInHierarchy)
        {
          //  canvasGroup.alpha = 0;
            objectiveDisplay.SetActive(true);
          //  LeanTween.alphaCanvas(canvasGroup, 1, animTime);
            Debug.Log("Dispalying");

        }
            
    }
    public void HideObjectivesPanel()
    {
        if (objectiveDisplay.activeInHierarchy)
        {
            objectiveDisplay.SetActive(false);
           // LeanTween.alphaCanvas(canvasGroup, 0, animTime).setOnComplete(() => objectiveDisplay.SetActive(false));
            Debug.Log("Hiding");
        }
    }


    private GameObject FindObjectiveFromString(string objectiveText)
    {
        if (objectiveExist(objectiveText))
        {
            foreach(GameObject objective in activeObjectives)
            {
                if(objective.GetComponentInChildren<TextMeshProUGUI>().text == objectiveText)
                {
                   return objective;
                }
            }
     //       for (int i = 0; i < activeObjectives.Count; i++)
   //         {
  //              if (activeObjectives[i].gameObject.GetComponentInChildren<TextMeshProUGUI>().text == objectiveText)
 //               {
 //                   return activeObjectives[i].gameObject;
//            }
        }
        return null;
    }
}
