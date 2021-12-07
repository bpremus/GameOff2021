using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameLog : MonoBehaviour
{
    // there is a much more elegant way to do this, feel free to override it 



    [SerializeField]
    TextMeshProUGUI text;

    Queue<string> strings = new Queue<string>();
    public void WriteLine(string str)
    {
        strings.Enqueue(str);
    }

    protected void Writer()
    {
        if (strings.Count > 0)
        {
            text.text += "\n" + strings.Dequeue();
            _t = 0;
        }
        
    }

    float _t = 0;
    protected void Update()
    {
        _t += Time.deltaTime;
        Writer();

        if (_t > 3)
        {
            _t = 0;
            PopLine();
        }
    }

    protected void PopLine()
    {
        for (int i = 0; i < text.text.Length; i++)
        {
            if (text.text[i] == '\n')
            {
                text.text = text.text.Substring(i + 1);
                return;
            }
        }
    }


    private static GameLog _instance;
    public static GameLog Instance
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

}
