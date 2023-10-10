using System;
using System.Collections.Generic;
using UnityEngine;


public class PlayerInventoryManager : CharacterInventoryManager
{
    //Inventory there
    public List<WeaponItem> weaponInventory;
    public List<ConsumableItem> consumableInventory;
    public List<EquipmentItem> equipmentInventory;
    
    protected override void Start()
    {
        if (GameManager.Instance.haveFile)
        {
            LoadInventoryData(GameManager.Instance.gameFileData.inventoryData);
        }
        else
        {
            LoadInventoryData();
        }
        base.Start();
    }


    public void ChangeHelmet()
    {
        playerEquipmentManager.ChangeHelmet();
    }
    public void ChangeTorso()
    {
        playerEquipmentManager.ChangeTorso();
    }
    public void ChangeHand()
    {
        playerEquipmentManager.ChangeHand();
    }
    public void ChangeHip()
    {
        playerEquipmentManager.ChangeHip();
    }

    public void LoadInventoryData(PlayerInvetoryFileData data = null)
    {
        if (data == null)
        {
            return;
        }
        weaponInventory = data.weaponInventory;
        equipmentInventory = data.equipmentInventory;
        consumableInventory = data.consumableItems;

        currentConsumableItem = data.currentConsumable;
        
        currentHelmetEquipment = data.currentHelmetEquipment;
        currentTorsoEquipment = data.currentTorsoEquipment;
        currentHandEquipment = data.currentHandEquipment;
        currentHipEquipment = data.currentHipEquipment;
        
        weaponsInRightHandSlots = data.weaponsInRightHandSlots;
        weaponsInLeftHandSlots = data.weaponsInLeftHandSlots;
        
        currentRightWeaponIndex = data.currentRightWeaponIndex;
        currentLeftWeaponIndex = data.currentLeftWeaponIndex;

        consumableSlot = data.consumableSlots;
        consumableIndex = data.consumableIndex;
    }

    /// <summary>
    /// 右手武器快捷栏
    /// </summary>
    public void ChangeRightWeapon()
    {
        currentRightWeaponIndex += 1;
        if (currentRightWeaponIndex == 0 && weaponsInRightHandSlots[0] != null)
        {
            rightWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
            characterWeaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
        }
        else if (currentRightWeaponIndex == 0 && weaponsInRightHandSlots[0] == null)
        {
            currentRightWeaponIndex += 1;
        }
        else if(currentRightWeaponIndex == 1 && weaponsInRightHandSlots[1] != null)
        {
            rightWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
            characterWeaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
        }
        else 
        {
            currentRightWeaponIndex += 1;
        }

        if (currentRightWeaponIndex > weaponsInRightHandSlots.Length - 1)
        {
            currentRightWeaponIndex = -1;
            rightWeapon = characterWeaponSlotManager.unarmedWeapon;
            characterWeaponSlotManager.LoadWeaponOnSlot(characterWeaponSlotManager.unarmedWeapon, false);
        }
    }
    
    /// <summary>
    /// 左手武器快捷栏
    /// </summary>
    public void ChangeLeftWeapon()
    {
        currentLeftWeaponIndex = currentLeftWeaponIndex + 1;
        if (currentLeftWeaponIndex == 0 && weaponsInLeftHandSlots[0] != null)
        {
            leftWeapon = weaponsInLeftHandSlots[currentLeftWeaponIndex];
            characterWeaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
        }
        else if (currentLeftWeaponIndex == 0 && weaponsInLeftHandSlots[0] == null)
        {
            currentLeftWeaponIndex += 1;
        }
        else if(currentLeftWeaponIndex == 1 && weaponsInLeftHandSlots[1] != null)
        {
            leftWeapon = weaponsInLeftHandSlots[currentLeftWeaponIndex];
            characterWeaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
        }
        else 
        {
            currentLeftWeaponIndex += 1;
        }

        if (currentLeftWeaponIndex > weaponsInLeftHandSlots.Length - 1)
        {
            currentLeftWeaponIndex = -1;
            leftWeapon = characterWeaponSlotManager.unarmedWeapon;
            characterWeaponSlotManager.LoadWeaponOnSlot(characterWeaponSlotManager.unarmedWeapon, true);
        }
    }

    /// <summary>
    /// 向右更换消耗品 如果只存在1个物品在消耗栏 则不会更换
    /// </summary>
    public void ChangeConsumable()
    {
        int lastConsumableIndex = consumableIndex;
        for (int i = 0; i < 4; i++)
        {
            consumableIndex = (consumableIndex + 1) % 4;
            if (consumableSlot[consumableIndex] != null)
            {
                if (consumableIndex != lastConsumableIndex)
                {
                    //清除 监听
                    consumableSlot[lastConsumableIndex].OnChangeAmount = delegate { };
                    //更换
                    currentConsumableItem = consumableSlot[consumableIndex];
                    //更新UI
                    (characterWeaponSlotManager as PlayerWeaponSlotManager).UpdateQuickSlotUI(
                        consumableSlot[consumableIndex]);
                    
                    break;
                }
            }
        }
    }
    
    public void UpdateWeaponOnSlot()
    {
        characterWeaponSlotManager.LoadBothWeaponOnSlots();
    }
    public void UpdateConsumableOnQuickSlot(ConsumableItem item)
    {
        PlayerWeaponSlotManager playerWeaponSlotManager = characterWeaponSlotManager as PlayerWeaponSlotManager;
        currentConsumableItem = item;
        playerWeaponSlotManager.quickSlotUI.UpdateCurrentConsumableIcon(item);
    }

    public PlayerInvetoryFileData ExportData()
    {
        PlayerInvetoryFileData data = new PlayerInvetoryFileData();
        data.weaponInventory = weaponInventory;
        data.equipmentInventory = equipmentInventory;
        data.consumableItems = consumableInventory;
        
        data.currentConsumable = currentConsumableItem;
        
        data.currentHelmetEquipment = currentHelmetEquipment;
        data.currentTorsoEquipment = currentTorsoEquipment;
        data.currentHipEquipment = currentHipEquipment;
        data.currentHandEquipment = currentHandEquipment;
        
        data.weaponsInRightHandSlots = weaponsInRightHandSlots;
        data.weaponsInLeftHandSlots = weaponsInLeftHandSlots;
        
        data.currentRightWeaponIndex = currentRightWeaponIndex;
        data.currentLeftWeaponIndex = currentLeftWeaponIndex;

        data.consumableSlots = consumableSlot;
        data.consumableIndex = consumableIndex;
        return data;
    }
}
