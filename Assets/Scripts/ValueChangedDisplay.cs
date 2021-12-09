using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ValueChangedDisplay : MonoBehaviour
{
    [Header("Value colors")]
    [SerializeField] private Color positiveValueColor;
    [SerializeField] private Color negativeValueColor;

    [Header("Value text")]
    [SerializeField] private GameObject textPrefab;


    [SerializeField] private List<GameObject> valueList;
    [SerializeField] private Transform foodParent;
    [SerializeField] private Transform woodParent;
    [SerializeField] private Transform populationParent;

    #region Instance
    private static ValueChangedDisplay _instance;
    public static ValueChangedDisplay Instance
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
    #endregion

    private void Start()
    {
        valueList = new List<GameObject>();
    }
    public void OnNewValue(int value,int type)
    {
        Transform parentCanvas;

        if (type == 0) parentCanvas = foodParent;
        else if (type == 1) parentCanvas = woodParent;
        else if (type == 2) parentCanvas = populationParent;
        else return;

        GameObject newValueobj = Instantiate(textPrefab, parentCanvas);
        TextMeshProUGUI txt = newValueobj.GetComponent<TextMeshProUGUI>();

        if (value < 0)
        {
            txt.color = negativeValueColor;
            txt.text =value.ToString();
        }

        else 
        {
            txt.color = positiveValueColor;
            txt.text = "+" + value.ToString();
        }

        valueList.Add(newValueobj);

    }
    public void RemoveValue(GameObject newValueObj)
    {
        if (valueList.Contains(newValueObj)) valueList.Remove(newValueObj);

        else Debug.LogError("newValueObj to remove does not exist");

        Destroy(newValueObj);
    }
}
