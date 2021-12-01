using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class RestartButton : MonoBehaviour
{
    [SerializeField] private GameObject sceneFader;
    public void Assure()
    {
        UIController.instance.HideAllUI();
        SceneFader.instance.EnableFade(gameObject);
        
    }
    public void RestartGame()
    {
            SceneManager.LoadScene(0); // default game scene
    }
}
