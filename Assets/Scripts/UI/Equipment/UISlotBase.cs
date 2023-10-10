using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UISlotBase : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public UIEquipmentManager equipmentManager;
    public UIManager uiManager;
    public Image icon;
    public GameObject effect;

    protected virtual void Awake()
    {
        uiManager = GetComponentInParent<UIManager>();
        equipmentManager = GetComponentInParent<UIEquipmentManager>();
    }

    protected virtual void OnButtonClick()
    {
        effect.SetActive(false);
    }

    public virtual void UpdateIcon(Item item)
    {
        if (item == null)
        {
            icon.gameObject.SetActive(false);
        }
        else
        {
            icon.gameObject.SetActive(true);
            icon.sprite = item.itemIcon;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        effect.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        effect.SetActive(false);
    }
}
