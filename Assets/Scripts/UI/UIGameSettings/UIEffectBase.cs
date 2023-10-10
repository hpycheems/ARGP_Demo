using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class UIEffectBase : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public GameObject effectObject;
    protected virtual void AddButtonListen(Button button)
    {
        if (button.name == "Right_Button")
        {
            Debug.Log("Right_Button");
        }
        else if (button.name == "Left_Button")
        {
            Debug.Log("Left_Button");
        }
    }
    
    #region Event

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        AudioClip clip = AudioManager.Instance.onButtonEnterClip;
        AudioManager.Instance.PlayAudio(clip, true, null);
        effectObject.SetActive(true);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        effectObject.SetActive(false);
    }
    
    #endregion
}
