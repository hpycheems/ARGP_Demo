using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIBaseDisplay : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler
{

    public GameObject Effect;
    public Image icon;
    public TMP_Text name;
    public TMP_Text displayText;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        Effect.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Effect.gameObject.SetActive(false);
    }
}
