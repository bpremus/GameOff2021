using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneFader : MonoBehaviour
{
    public static SceneFader instance;
    [SerializeField] private float fadeTime = 0.3f;
    private CanvasGroup canvasGroup;
    private bool isFadeFinished;
    private LTDescr fade; 
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(this);
        }
        canvasGroup.alpha = 1;
        DisableFade();
    }
    private void OnEnable()
    {
       // EnableFade();
    }
    private void OnDisable()
    {
      // DisableFade();
    }
    public void EnableFade(GameObject reference = default)
    {
        if (fade != null) LeanTween.cancel(fade.uniqueId);

        if(reference != null)
             fade = LeanTween.alphaCanvas(canvasGroup, 1, fadeTime ).setOnComplete(() => CheckForGameReset(reference));

        else fade = LeanTween.alphaCanvas(canvasGroup, 1, fadeTime ).setOnComplete(() => fade = null);
    }
    public void CheckForGameReset(GameObject referemceObj)
    {

        fade = null;
        var obj = referemceObj.GetComponent<RestartButton>();
        if (obj)
        {
            obj.RestartGame();
        }
    }
    public void DisableFade()
    {
        if (fade != null) LeanTween.cancel(fade.uniqueId);
        fade =  LeanTween.alphaCanvas(canvasGroup, 0, fadeTime).setOnComplete(() => fade = null);
       
    }
}
