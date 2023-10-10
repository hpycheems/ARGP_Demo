using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISlotConsumable : UISlotBase
{
    public bool consumableSlot01;
    public bool consumableSlot02;
    public bool consumableSlot03;
    public bool consumableSlot04;

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
        uiManager.consumableSlot01 = consumableSlot01;
        uiManager.consumableSlot02 = consumableSlot02;
        uiManager.consumableSlot03 = consumableSlot03;
        uiManager.consumableSlot04 = consumableSlot04;
        //打开选择面板
        uiManager.ListenChangeConsumable();
        equipmentManager.OnEquipmentSlotClick(SelectEquipmentSlotType.Consumable);
    }
}
