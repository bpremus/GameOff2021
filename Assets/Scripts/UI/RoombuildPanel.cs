using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoombuildPanel : MonoBehaviour
{
    private float animTime = 0.1f;
    private Vector3 scaleSize = new Vector3(1.1f,1.1f,1.1f);
    private LeanTweenType ease = LeanTweenType.easeSpring;
    public int roomID = 0;
    //  0 - corridor    1 - storage    2 - barracks    3 - resource room    4 - queen room

    #region Logic
    public void TryToBuy()
    {
        //find and ask manager if player has needed resources for id room

        //if so proceed and tell buildmanager to place room with this id

        //play anim
        Clicked_Bought();
        //hide build menu


        GetComponentInParent<UIPopup>().DestroySelf();
        //if no 
        //play animation 
      //  Clicked_CantBuy();
    }

    #endregion

    #region Animations
    public void MouseEnter()
    {
        LeanTween.scale(gameObject, scaleSize, animTime);
    }
    public void MouseExit()
    {
        LeanTween.scale(gameObject, new Vector3(1,1,1), animTime - animTime/3);
    }
    private void Clicked_Bought()
    {
        LeanTween.scale(gameObject, scaleSize + new Vector3(0.05f,0.05f,0.05f), animTime).setEase(ease);
    }
    private void Clicked_CantBuy()
    {

    }
    #endregion
}
