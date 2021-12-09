using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ActionLogger : MonoBehaviour
{

    [Header("Logs colors")]
    [SerializeField] private Color normalColor;
    [SerializeField] private Color attentionColor;
    [SerializeField] private Color criticalColor;

    [Header("Logs text")]
    [SerializeField] private GameObject textPrefab;


    [SerializeField] private List<GameObject> logsList;
    [SerializeField] private Transform parentCanvas;


    #region Instance
    private static ActionLogger _instance;
    public static ActionLogger Instance
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
        logsList = new List<GameObject>();

    }

    private void Update()
    {
        
    }

    public void AddLog(string text,int colorIndex = default)
    {
       GameObject newLog = Instantiate(textPrefab, parentCanvas);
       TextMeshProUGUI txt = newLog.GetComponent<TextMeshProUGUI>();
        txt.text = "> "+text;
        if (colorIndex != default)
        {
            Color color;
            if (colorIndex == 0) color = normalColor;
            else if (colorIndex == 1) color = attentionColor;
            else if (colorIndex == 2) color = criticalColor;
            else color = normalColor;

            txt.color = color;
        }
        AddToList(newLog);
       
    }

    private void AddToList(GameObject newLog)
    {
        logsList.Add(newLog);
    }

    public void RemoveLog(GameObject log)
    {
        if (logsList.Contains(log)) logsList.Remove(log);

        else Debug.LogError("Log to remove does not exist");

        Destroy(log);
    }
}
