using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //Components
    public PlayerInventoryManager playerInventoryManager;
    public EquipmentWindowUI equipmentWindowUI;
    public QuickSlotUI quickSlotUI;
    
    [Header("UI Windows")] 
    public GameObject quickSlotWindow;
    public GameObject topUIWindow;
    public GameObject weaponInventoryWindow;
    public GameObject equipmentWindow;
    public GameObject selectWindow;
    
    [Header("Equipment Window Slot Selected")]
    public bool rightHandSlot01Selected;
    public bool rightHandSlot02Selected;
    public bool leftHandSlot01Selected;
    public bool leftHandSlot02Selected;
    
    [Header("Consumable Window Slot Selected")]
    public bool consumableSlot01;
    public bool consumableSlot02;
    public bool consumableSlot03;
    public bool consumableSlot04;
    
    [Header("Equipment Window Slot Selected")]
    public bool isHelmet;
    public bool isTorso;
    public bool isHand;
    public bool isHip;

    [Header("Weapon Inventory")]
    public GameObject weaponInventorySlotPrefab;
    public Transform weaponInventorySlotParent;
    
    [Header("Player Interactable")] 
    public GameObject interactableOpoUp;
    public GameObject itemInteractableOpoUp;
    [Header("PoisonBar")] 
    public PoisonAmountBar poisonAmountBar;
    public PoisonBuildUpBar poisonBuildUpBar;
    
    [Header("Bag Window")]
    public BonfirePanelManager bonfirePanelManager;
    public GameObject bagAndEquipmentPanel;
    [SerializeField] private UIBagPanelManager bagPanelManager;
    [SerializeField] private UIEquipmentManager equipmentPanelManager;
    [SerializeField] private UISelectPanel uiSelectPanel;
    
    [Header("Icon")] 
    public Image icon;
    public TMP_Text tipTxt;
    public Sprite[] iconSprites;
    public void OpenBonFirePanel()
    {
        bonfirePanelManager.gameObject.SetActive(true);
    }
    public void CloseBonFirePanel()
    {
        bonfirePanelManager.gameObject.SetActive(false);
    }

    private void Awake()
    {
        quickSlotUI = GetComponentInChildren<QuickSlotUI>();
        //uiSelectPanel = GetComponentInChildren<UISelectPanel>();
    }
    private void Start()
    {
        playerInventoryManager = FindObjectOfType<PlayerInventoryManager>();
        
        equipmentWindowUI.LoadWeaponsOnEquipmentScreen(playerInventoryManager);
        //quickSlotUI.UpdateCurrentConsumableIcon(playerInventoryManager.currentConsumableItem);
        //quickSlotUI.UpdateCurrentSpellIcon(playerInventoryManager.currentSpell);
    }
    
    /// <summary>
    /// 打开背包
    /// </summary>
    public void OpenInventoryWindow()
    {
        //打开面板
        //更新数据
        bagAndEquipmentPanel.SetActive(true);
        bagPanelManager.OpenItemInventory();
        
        //UpdateUI();
        //selectWindow.SetActive(false);
        //quickSlotWindow.SetActive(false);
        //weaponInventoryWindow.SetActive(true);
    }
    /// <summary>
    /// 打开装备栏
    /// </summary>
    public void OpenEquipmentSlotWindow()
    {
        bagAndEquipmentPanel.SetActive(true);
        equipmentPanelManager.OpenEquipmentPanel();
    }

    /// <summary>
    /// 更新图标 背包、装备栏
    /// </summary>
    /// <param name="i"></param>
    public void UpdateIcon(int i)
    {
        switch (i)
        {
            case 0:
                icon.sprite = iconSprites[0];
                tipTxt.text = "背包";
                break;
            case 1:
                icon.sprite = iconSprites[1];
                tipTxt.text = "装备栏";
                break;
            case 2:
                icon.sprite = iconSprites[2];
                tipTxt.text = "设置";
                break;
        }
    }
    /// <summary>
    /// 关闭所有窗口
    /// </summary>
    public void CloseAllWindow()
    {
        selectWindow.SetActive(false);   
        quickSlotWindow.SetActive(true);
        topUIWindow.SetActive(true);
        
        ResetAllSelectedSlots();
        CloseInventoryWindow();
        equipmentWindow.SetActive(false);
        uiSelectPanel.gameObject.SetActive(false);
    }
    /// <summary>
    /// 初始化 所有选择
    /// </summary>
    public void ResetAllSelectedSlots()
    {
        rightHandSlot01Selected = false;
        rightHandSlot02Selected = false;
        leftHandSlot01Selected = false;
        leftHandSlot02Selected = false;
        consumableSlot01 = false;
        consumableSlot02 = false;
        consumableSlot03 = false;
        consumableSlot04 = false;
        isHelmet = false;
        isTorso = false;
        isHand = false;
        isHip = false;
    }
    /// <summary>
    /// 关闭背包面板
    /// </summary>
    void CloseInventoryWindow()
    {
        bagAndEquipmentPanel.SetActive(false);
        quickSlotWindow.SetActive(true);
        topUIWindow.SetActive(true);
        weaponInventoryWindow.SetActive(false);
    }

    /// <summary>
    /// 更换武器
    /// </summary>
    public void ListenChangeWeapon()
    {
        uiSelectPanel.onChangeClick += ChangeWeapon;
    }
    void ChangeWeapon(Item item)
    {
        WeaponItem weapon = (WeaponItem)item;
        if (rightHandSlot01Selected)
        {
            //当 slot上存在武器时 互换 武器
            if (playerInventoryManager.weaponsInRightHandSlots[0] != null && !playerInventoryManager.weaponsInRightHandSlots[0].isUnarmed)
            {
                WeaponItem removeWeapon = playerInventoryManager.weaponsInRightHandSlots[0];
                playerInventoryManager.weaponInventory.Add(removeWeapon);
            }
            playerInventoryManager.weaponsInRightHandSlots[0] = weapon;
            playerInventoryManager.weaponInventory.Remove(weapon);
            //切换UI
            equipmentPanelManager.ChangeWeapon(weapon, 0);
        }
        else if (rightHandSlot02Selected)
        {
            //当 slot上存在武器时 互换 武器
            if (playerInventoryManager.weaponsInRightHandSlots[1] != null && !playerInventoryManager.weaponsInRightHandSlots[1].isUnarmed)
            {
                WeaponItem removeWeapon = playerInventoryManager.weaponsInRightHandSlots[1];
                playerInventoryManager.weaponInventory.Add(removeWeapon);
            }
            playerInventoryManager.weaponsInRightHandSlots[1] = weapon;
            playerInventoryManager.weaponInventory.Remove(weapon);
            
            equipmentPanelManager.ChangeWeapon(weapon, 1);
        }
        else if (leftHandSlot01Selected)
        {
            //当 slot上存在武器时 互换 武器
            if (playerInventoryManager.weaponsInLeftHandSlots[0] != null && !playerInventoryManager.weaponsInLeftHandSlots[0].isUnarmed)
            {
                WeaponItem removeWeapon = playerInventoryManager.weaponsInLeftHandSlots[0];
                playerInventoryManager.weaponInventory.Add(removeWeapon);
            }
            playerInventoryManager.weaponsInLeftHandSlots[0] = weapon;
            playerInventoryManager.weaponInventory.Remove(weapon);
            
            equipmentPanelManager.ChangeWeapon(weapon, 2);
        }
        else if (leftHandSlot02Selected)
        {
            //当 slot上存在武器时 互换 武器
            if (playerInventoryManager.weaponsInLeftHandSlots[1] != null && !playerInventoryManager.weaponsInLeftHandSlots[1].isUnarmed)
            {
                WeaponItem removeWeapon = playerInventoryManager.weaponsInLeftHandSlots[1];
                playerInventoryManager.weaponInventory.Add(removeWeapon);
            }
            playerInventoryManager.weaponsInLeftHandSlots[1] = weapon;
            playerInventoryManager.weaponInventory.Remove(weapon);
            //更新装备栏
            equipmentPanelManager.ChangeWeapon(weapon, 3);
        }
        //实时更新玩家的装备
        playerInventoryManager.UpdateWeaponOnSlot();
        
        ResetAllSelectedSlots();
        //打开EquipmentPanel
        equipmentPanelManager.OpenPanel();
        uiSelectPanel.onChangeClick -= ChangeWeapon;
    }
    public void ListenChangeConsumable()
    {
        uiSelectPanel.onChangeClick += ChangConsumable;
    }
    private void ChangConsumable(Item item)
    {
        ConsumableItem consumableItem = (ConsumableItem)item;
        if (consumableSlot01)
        {
            Debug.Log(consumableItem.name);
            //判断Slot中是装备了东西
            if (playerInventoryManager.consumableSlot[0] != null)
            {
                //加入
                playerInventoryManager.consumableInventory.Add(playerInventoryManager.consumableSlot[0]);
            }
            playerInventoryManager.consumableSlot[0] = consumableItem;
            playerInventoryManager.consumableInventory.Remove(consumableItem);
            equipmentPanelManager.ChangeConsumable(consumableItem, 0);
            if (playerInventoryManager.consumableIndex == 0)//只有小标是 相互匹配的时候才更新
            {
                playerInventoryManager.UpdateConsumableOnQuickSlot(consumableItem);
            }
        }
        else if (consumableSlot02)
        {
            //判断Slot中是装备了东西
            if (playerInventoryManager.consumableSlot[1] != null)
            {
                //加入
                playerInventoryManager.consumableInventory.Add(playerInventoryManager.consumableSlot[1]);
            }
            playerInventoryManager.consumableSlot[1] = consumableItem;
            playerInventoryManager.consumableInventory.Remove(consumableItem);
            equipmentPanelManager.ChangeConsumable(consumableItem, 1);
            if (playerInventoryManager.consumableIndex == 1)
            {
                playerInventoryManager.UpdateConsumableOnQuickSlot(consumableItem);
            }
        }
        else if (consumableSlot03)
        {
            //判断Slot中是装备了东西
            if (playerInventoryManager.consumableSlot[2] != null)
            {
                //加入
                playerInventoryManager.consumableInventory.Add(playerInventoryManager.consumableSlot[2]);
            }
            playerInventoryManager.consumableSlot[2] = consumableItem;
            playerInventoryManager.consumableInventory.Remove(consumableItem);
            equipmentPanelManager.ChangeConsumable(consumableItem, 2);
            if (playerInventoryManager.consumableIndex == 2)
            {
                playerInventoryManager.UpdateConsumableOnQuickSlot(consumableItem);
            }
        }
        else if (consumableSlot04)
        {
            //判断Slot中是装备了东西
            if (playerInventoryManager.consumableSlot[3] != null)
            {
                //加入
                playerInventoryManager.consumableInventory.Add(playerInventoryManager.consumableSlot[3]);
            }
            playerInventoryManager.consumableSlot[3] = consumableItem;
            playerInventoryManager.consumableInventory.Remove(consumableItem);
            equipmentPanelManager.ChangeConsumable(consumableItem, 3);
            if (playerInventoryManager.consumableIndex == 3)
            {
                playerInventoryManager.UpdateConsumableOnQuickSlot(consumableItem);
            }
        }
        //打开EquipmentPanel
        equipmentPanelManager.OpenPanel();
        ResetAllSelectedSlots();
        //最后移除监听
        uiSelectPanel.onChangeClick -= ChangConsumable;
    }
    public void ListenChangeEquipment()
    {
        uiSelectPanel.onChangeClick += ChangeEquipment;
    }
    private void ChangeEquipment(Item item)
    {
        if (isHelmet)
        {
            HelmetEquipment helmetEquipment = item as HelmetEquipment;
            //如果装备了 则更换下来
            if (playerInventoryManager.currentHelmetEquipment != null)
            {
                playerInventoryManager.equipmentInventory.Add(playerInventoryManager.currentHelmetEquipment);                
            }
            //更换
            playerInventoryManager.currentHelmetEquipment = helmetEquipment;
            playerInventoryManager.equipmentInventory.Remove(helmetEquipment);
            equipmentPanelManager.ChangeEquipment(helmetEquipment, EquipmentType.Helmet);
            //通过 更换玩家身上的装备
            playerInventoryManager.ChangeHelmet();
        }
        else if (isTorso)
        {
            TorsoEquipment TorsoEquipment = item as TorsoEquipment;
            //如果装备了 则更换下来
            if (playerInventoryManager.currentTorsoEquipment != null)
            {
                playerInventoryManager.equipmentInventory.Add(playerInventoryManager.currentTorsoEquipment);                
            }
            //更换
            playerInventoryManager.currentTorsoEquipment = TorsoEquipment;
            playerInventoryManager.equipmentInventory.Remove(TorsoEquipment);
            equipmentPanelManager.ChangeEquipment(TorsoEquipment, EquipmentType.Torso);
            //通过 更换玩家身上的装备
            playerInventoryManager.ChangeTorso();
        }
        else if (isHand)
        {
            HandEquipment HandEquipment = item as HandEquipment;
            //如果装备了 则更换下来
            if (playerInventoryManager.currentHandEquipment != null)
            {
                playerInventoryManager.equipmentInventory.Add(playerInventoryManager.currentHandEquipment);                
            }
            //更换
            playerInventoryManager.currentHandEquipment = HandEquipment;
            playerInventoryManager.equipmentInventory.Remove(HandEquipment);
            equipmentPanelManager.ChangeEquipment(HandEquipment, EquipmentType.Hand);
            //通过 更换玩家身上的装备
            playerInventoryManager.ChangeHand();
        }
        else if (isHip)
        {
            LegEquipment HipEquipment = item as LegEquipment;
            //如果装备了 则更换下来
            if (playerInventoryManager.currentHipEquipment != null)
            {
                playerInventoryManager.equipmentInventory.Add(playerInventoryManager.currentHipEquipment);                
            }
            //更换
            playerInventoryManager.currentHipEquipment = HipEquipment;
            playerInventoryManager.equipmentInventory.Remove(HipEquipment);
            equipmentPanelManager.ChangeEquipment(HipEquipment, EquipmentType.Hip);
            //通过 更换玩家身上的装备
            playerInventoryManager.ChangeHip();
        }
        
        uiSelectPanel.onChangeClick -= ChangeEquipment;
    }

    public void OpenEquipmentPanel()
    {
        equipmentPanelManager.OpenPanel();
    }

    void UpdateUI()
    {
        ClearInstanceSlot();
        Debug.Log(playerInventoryManager.weaponInventory.Count);
        for (int i = 0; i < playerInventoryManager.weaponInventory.Count; i++)
        {
            if (playerInventoryManager.weaponInventory[i] == null) return;
            GameObject obj = Instantiate(weaponInventorySlotPrefab, weaponInventorySlotParent);
            WeaponInventorySlot slot = obj.GetComponent<WeaponInventorySlot>();
            slot.AddItem(playerInventoryManager.weaponInventory[i]);
        }
    }
    void ClearInstanceSlot()
    {
        int count = weaponInventorySlotParent.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            Destroy(weaponInventorySlotParent.transform.GetChild(0).gameObject);
        }
    }
    public void OpenSelectWindow()
    {
        quickSlotWindow.SetActive(false);
        topUIWindow.SetActive(false);
        selectWindow.SetActive(true);
    }
    public void OpenDefaultWindow()
    {
        quickSlotWindow.SetActive(true);
        topUIWindow.SetActive(true);
    }
    public void CloseDefaultWindow()
    {
        quickSlotWindow.SetActive(false);
        topUIWindow.SetActive(false);
    }
    public void CloseSelectWindow()
    {
        quickSlotWindow.SetActive(true);
        topUIWindow.SetActive(true);
        selectWindow.SetActive(false);   
    }
    public void CloseInventoryWindowAndOpenEquipmentWindow()
    {
        equipmentWindow.SetActive(true);
        weaponInventoryWindow.SetActive(false);
    }
}
