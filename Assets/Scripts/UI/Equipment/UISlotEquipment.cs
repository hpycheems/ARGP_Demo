using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISlotEquipment : UISlotBase
{
    public bool isHelmet;
    public bool isTorso;
    public bool isHand;
    public bool isHip;

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
        uiManager.isHelmet  = isHelmet;
        uiManager.isTorso  = isTorso;
        uiManager.isHand = isHand;
        uiManager.isHip = isHip;
        uiManager.ListenChangeEquipment();
        equipmentManager.OnEquipmentSlotClick(SelectEquipmentSlotType.Equipment);
    }
}
