using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISlotSward : UISlotBase
{
    public bool isSwardSlot01;//right 1
    public bool isSwardSlot02;//right 2
    public bool isSwardSlot03;//left 1
    public bool isSwardSlot04;//left 2
    
    private Button button;

    protected override void Awake()
    {
        base.Awake();
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }

    protected override void OnButtonClick()
    {
        base.OnButtonClick();
        uiManager.rightHandSlot01Selected = isSwardSlot01;
        uiManager.rightHandSlot02Selected = isSwardSlot02;
        uiManager.leftHandSlot01Selected = isSwardSlot03;
        uiManager.leftHandSlot02Selected = isSwardSlot04;
        uiManager.ListenChangeWeapon();
        equipmentManager.OnEquipmentSlotClick(SelectEquipmentSlotType.Weapon);
    }
}
