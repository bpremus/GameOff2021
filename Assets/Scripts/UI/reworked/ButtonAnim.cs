using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ButtonAnim : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] private Vector3 baseScale = new Vector3(0.85f,0.85f,0.85f);
    [SerializeField] private Vector3 targetScale = Vector3.one;
    [SerializeField] private float animInTime = 0.2f;
    [SerializeField] private bool applyLocally = false;
    private LTDescr anim;
    [SerializeField] private bool ignoreButtonDependency = false;
   [HideInInspector] public bool displayedCorrectly = false;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (ignoreButtonDependency)
        {
            anim = LeanTween.scale(gameObject, targetScale, animInTime);
        }
        else
        {
            if (button)
            {
                if (button.interactable) anim = LeanTween.scale(gameObject, targetScale, animInTime);
            }
        }

    }


    Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (ignoreButtonDependency)
        {
            LeanTween.cancel(anim.uniqueId);
            LeanTween.scale(gameObject, baseScale, animInTime / 2);
        }
        else
        {
            if (button)
            {
                if (button.interactable)
                {
                    LeanTween.cancel(anim.uniqueId);
                    LeanTween.scale(gameObject, baseScale, animInTime / 2);
                }
            }
        }


    }
    public void ForceDisplay()
    {
        gameObject.SetActive(true);
        transform.localScale = baseScale;
        displayedCorrectly = true;
    }
    void OnEnable()
    {
        if (applyLocally)
        {
            transform.localScale = Vector3.zero;
            LeanTween.scale(gameObject, baseScale, animInTime).setOnComplete(() => displayedCorrectly = true); ;
        }

    }
    void OnDisable() => transform.localScale = baseScale;
}
