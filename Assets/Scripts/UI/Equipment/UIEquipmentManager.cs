using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class UIEquipmentManager : MonoBehaviour
{
    private UIManager uiManager;
    public UISelectPanel selectPanel;
    
    public UISlotSward rightSlot01;
    public UISlotSward rightSlot02;
    public UISlotSward leftSlot03;
    public UISlotSward leftSlot04;

    public UISlotConsumable consumableSlot01;
    public UISlotConsumable consumableSlot02;
    public UISlotConsumable consumableSlot03;
    public UISlotConsumable consumableSlot04;

    public UISlotEquipment helmetSlot;
    public UISlotEquipment TorsoSlot;
    public UISlotEquipment handSlot;
    public UISlotEquipment hipSlot;

    public UnityAction<SelectEquipmentSlotType> OnEquipmentSlotClick;
    
    WeaponItem[] weaponsInRightHandSlots;
    WeaponItem[] weaponsInLeftHandSlots;

    ConsumableItem[] consumableSlots;
    
    EquipmentItem helmetEquipment;
    EquipmentItem torsoEquipment;
    EquipmentItem handEquipment;
    EquipmentItem hipEquipment;
    private void Awake()
    {
        uiManager = GetComponentInParent<UIManager>();
    }

    private void Start()
    {
        OnEquipmentSlotClick += (SelectEquipmentSlotType type) =>
        {
            //先把场景干掉
            ClosePanel();
            switch (type)
            {
                case SelectEquipmentSlotType.Weapon:
                    WeaponItem[] weaponItems = uiManager.playerInventoryManager.weaponInventory.ToArray();
                    selectPanel.InitItem(weaponItems);
                    break;
                case SelectEquipmentSlotType.Consumable:
                    ConsumableItem[] consumableItems = uiManager.playerInventoryManager.consumableInventory.ToArray();
                    selectPanel.InitItem(consumableItems);
                    break;
                case SelectEquipmentSlotType.Equipment:
                    selectPanel.InitItem(LoadEquipments());
                    break;
            }
        };
    }

    EquipmentItem[] LoadEquipments()
    {
        List<EquipmentItem> equipmentItems = new List<EquipmentItem>();
        EquipmentType type = EquipmentType.None;
        if (uiManager.isHelmet)
        {
            type = EquipmentType.Helmet;
        }
        else if (uiManager.isTorso)
        {
            type = EquipmentType.Torso;
        }
        else if (uiManager.isHand)
        {
            type = EquipmentType.Hand;
        }
        else if (uiManager.isHip)
        {
            type = EquipmentType.Hip;
        }

        foreach (EquipmentItem item in uiManager.playerInventoryManager.equipmentInventory)
        {
            if (item.type == type)
            {
                equipmentItems.Add(item);
            }
        }

        return equipmentItems.ToArray();
    }

    public void OpenEquipmentPanel()
    {
        PlayerInventoryManager inventory = uiManager.playerInventoryManager;
        //加载数据
        weaponsInRightHandSlots = inventory.weaponsInRightHandSlots;
        weaponsInLeftHandSlots = inventory.weaponsInLeftHandSlots;
        consumableSlots = inventory.consumableSlot;
        helmetEquipment = inventory.currentHelmetEquipment;
        torsoEquipment = inventory.currentTorsoEquipment;
        handEquipment = inventory.currentHandEquipment;
        hipEquipment = inventory.currentHipEquipment;
        UpdateUI();
    }

    void UpdateUI()
    {
        rightSlot01.UpdateIcon(weaponsInRightHandSlots[0]);
        rightSlot02.UpdateIcon(weaponsInRightHandSlots[1]);
        leftSlot03.UpdateIcon(weaponsInLeftHandSlots[0]);
        leftSlot04.UpdateIcon(weaponsInLeftHandSlots[1]);

        consumableSlot01.UpdateIcon(consumableSlots[0]);
        consumableSlot02.UpdateIcon(consumableSlots[1]);
        consumableSlot03.UpdateIcon(consumableSlots[2]);
        consumableSlot04.UpdateIcon(consumableSlots[3]);

        helmetSlot.UpdateIcon(helmetEquipment);
        TorsoSlot.UpdateIcon(torsoEquipment);
        handSlot.UpdateIcon(handEquipment);
        hipSlot.UpdateIcon(hipEquipment);
    }

    public void ChangeWeapon(WeaponItem item, int number)
    {
        switch (number)
        {
            case 0:
                rightSlot01.UpdateIcon(item);
                break;
            case 1:
                rightSlot02.UpdateIcon(item);
                break;
            case 2:
                leftSlot03.UpdateIcon(item);
                break;
            case 3:
                leftSlot04.UpdateIcon(item);
                break;
        }
    }

    public void ChangeConsumable(ConsumableItem item, int number)
    {
        switch (number)
        {
            case 0:
                consumableSlot01.UpdateIcon(item);
                break;
            case 1:
                consumableSlot02.UpdateIcon(item);
                break;
            case 2:
                consumableSlot03.UpdateIcon(item);
                break;
            case 3:
                consumableSlot04.UpdateIcon(item);
                break;
        }
    }

    public void ChangeEquipment(EquipmentItem item, EquipmentType type)
    {
        switch (type)
        {
            case EquipmentType.Helmet:
                helmetSlot.UpdateIcon(item);
                break;
            case EquipmentType.Torso:
                TorsoSlot.UpdateIcon(item);
                break;
            case EquipmentType.Hand:
                handSlot.UpdateIcon(item);
                break;
            case EquipmentType.Hip:
                hipSlot.UpdateIcon(item);
                break;
        }
    }

    void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    public void OpenPanel()
    {
        gameObject.SetActive(true);
    }
}
