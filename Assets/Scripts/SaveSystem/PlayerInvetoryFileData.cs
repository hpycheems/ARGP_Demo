using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class PlayerInvetoryFileData 
{
    //玩家武器背包
    public List<WeaponItem> weaponInventory;
    public List<EquipmentItem> equipmentInventory;
    public List<ConsumableItem> consumableItems;
    
    //当前穿在身上的装备
    //Helmet
    public HelmetEquipment currentHelmetEquipment;
    //Torso
    public TorsoEquipment currentTorsoEquipment;
    //Hip
    public LegEquipment currentHipEquipment;
    //Han
    public HandEquipment currentHandEquipment;

    public ConsumableItem currentConsumable;
    
    [Header("Quick Weapon Slot")]
    public WeaponItem[] weaponsInRightHandSlots = new WeaponItem[1];
    public WeaponItem[] weaponsInLeftHandSlots = new WeaponItem[1];

    [Header("当前武器 下标")]
    public int currentRightWeaponIndex = 0;
    public int currentLeftWeaponIndex = 0;

    [Header("Consumable Slot")] 
    public ConsumableItem[] consumableSlots;
    public int consumableIndex = 0;
}