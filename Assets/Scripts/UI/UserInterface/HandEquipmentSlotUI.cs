using System;
using UnityEngine;
using UnityEngine.UI;


public class HandEquipmentSlotUI : MonoBehaviour
{
    public UIManager uiManager;
    private WeaponItem weapon;
    
    public Image icon;

    public bool rightHandSlot01;
    public bool rightHandSlot02;
    public bool leftHandSlot01;
    public bool leftHandSlot02;

    public void AddItem(WeaponItem newItem)
    {
        if (newItem == null) return;
        weapon = newItem;
        icon.sprite = newItem.itemIcon;
        icon.enabled = true;
        icon.gameObject.SetActive(true);
    }
    public void CloseItem()
    {
        weapon = null;
        icon.sprite = null;
        icon.enabled = false;
        icon.gameObject.SetActive(false);
    }
    
    public void SelectThisSlot()
    {
        if (rightHandSlot01)
        {
            uiManager.rightHandSlot01Selected = true;
        }
        else if (rightHandSlot02)
        {
            uiManager.rightHandSlot02Selected = true;
        }
        else if (leftHandSlot01)
        {
            uiManager.leftHandSlot01Selected = true;
        }
        else
        {
            uiManager.leftHandSlot02Selected = true;
        }
    }
}
