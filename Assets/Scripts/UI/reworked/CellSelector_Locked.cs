using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellSelector_Locked : MonoBehaviour
{

    [SerializeField] private float enableAnimTime;
    [SerializeField] private float disableAnimTime;
    [SerializeField] private Vector3 scaleVector = new Vector3(.3f, .3f, .3f);
    [SerializeField] private LeanTweenType animCurveType;

    private Vector3 startingScale = new Vector3(1.8f,1.8f,1.8f);
    private LTDescr scaleTo;
    private LTDescr idleAnim;
    private Vector3 spawnedPos;
    private void Awake()
    {
        startingScale = transform.localScale;
    }


    public void ScaleUp()
    {
        transform.localScale = startingScale;
        if (idleAnim != null) LeanTween.cancel(idleAnim.uniqueId);
        scaleTo = LeanTween.scale(gameObject, startingScale + scaleVector, enableAnimTime).setEase(animCurveType);
    }
    private void ScaleDown()
    {
        transform.localScale = startingScale;
    }
    private void OnEnable()
    {
        ScaleUp();
    }
    private void OnDisable()
    {
        ScaleDown();
    }
}
