using System;
using UnityEngine;


public class CharacterInventoryManager : MonoBehaviour
{
    //Components 
    protected CharacterWeaponSlotManager characterWeaponSlotManager;
    protected PlayerEquipmentManager playerEquipmentManager;
    
    [Header("正在攻击的武器")] 
    public WeaponItem currentItemBeingUsed;
    
    [Header("当前装备的")]
    //当前装备的道具
    public SpellItem currentSpell;
    //右手武器
    public WeaponItem rightWeapon;
    //左手武器
    public WeaponItem leftWeapon;
    //当前装备的消耗品
    public ConsumableItem currentConsumableItem;
    
    [Header("Equipments")]
    //Helmet
    public HelmetEquipment currentHelmetEquipment;
    //Torso
    public TorsoEquipment currentTorsoEquipment;
    //Hip
    public LegEquipment currentHipEquipment;
    //Han
    public HandEquipment currentHandEquipment;
    
    [Header("Quick Weapon Slot")]
    public WeaponItem[] weaponsInRightHandSlots = new WeaponItem[2];
    public WeaponItem[] weaponsInLeftHandSlots = new WeaponItem[2];

    [Header("当前武器 下标")]
    public int currentRightWeaponIndex = 0;
    public int currentLeftWeaponIndex = 0;

    [Header("Quick Consumable Slot")] 
    public ConsumableItem[] consumableSlot = new ConsumableItem[4];
    [Header("current consumable index")] 
    public int consumableIndex = 0;

    private void Awake()
    {
        characterWeaponSlotManager = GetComponent<CharacterWeaponSlotManager>();
        playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
    }

    protected virtual void Start()
    {
        characterWeaponSlotManager.LoadBothWeaponOnSlots();
        playerEquipmentManager.EquipAllEquipmentModelsOnStart();
        //更新UI
        (characterWeaponSlotManager as PlayerWeaponSlotManager).UpdateQuickSlotUI(consumableSlot[consumableIndex]);
    }

    
}