using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBagPanelManager : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    public Transform container;
    public Button itemButton;
    public Button swardButton; 
    public Button equipmentButton;
    
    public GameObject itemScrollView;
    
    public UIBaseDisplay itemDisplay;
    public UIBaseDisplay swardDisplay;
    public UIBaseDisplay equipmentDisplay;


    List<ConsumableItem> ConsumableInventory;
    List<WeaponItem> WeaponInventory;
    List<EquipmentItem> EquipmentInventory;
    private void Awake()
    {
        uiManager = GetComponentInParent<UIManager>();
    }
    private void Start()
    {
        itemButton.onClick.AddListener(() =>
        {
            //按下次按钮 首先删除原先的物件
            deleteOldItem();
            //从玩家的背包中
            //加载出新的物件
            instantiateItem();
        });
        swardButton.onClick.AddListener(() =>
        {
            //按下次按钮 首先删除原先的物件
            deleteOldItem();
            //从玩家的背包中
            //加载出新的物件
            instantiateSward();
        });
        equipmentButton.onClick.AddListener(() =>
        {
            //按下次按钮 首先删除原先的物件
            deleteOldItem();
            //从玩家的背包中
            //加载出新的物件
            instantiateEquipment();
        });
    }
    public void OpenItemInventory()
    {
        deleteOldItem();
        instantiateItem();
    }
    
    void deleteOldItem()
    {
        int count = container.childCount;
        for (int i = 0; i < count; i++)
        {
            Destroy(container.GetChild(i).gameObject);
        }
    }
    void instantiateItem()
    {
        //通过玩家背包中的item inventory 加载
        //Debug.Log("物品从玩家的背包中加载！");
        ConsumableInventory = uiManager.playerInventoryManager.consumableInventory;
        for (int i = 0; i < ConsumableInventory.Count; i++)
        {
            GameObject obj = Instantiate(itemDisplay.gameObject, container);
            UIItemDisplay display = obj.GetComponent<UIItemDisplay>();
            display.UpdateUI(ConsumableInventory[i]);
        }
        
    }
    void instantiateSward()
    {
        WeaponInventory = uiManager.playerInventoryManager.weaponInventory;
        for (int i = 0; i < WeaponInventory.Count; i++)
        {
            //加载出 预制件
            GameObject obj =Instantiate(swardDisplay.gameObject, container);
            //初始化   
            UISwardDisplay display = obj.GetComponent<UISwardDisplay>();
            //设置为可见
            display.UpdateUI(WeaponInventory[i]);
        }
    }
    void instantiateEquipment()
    {
        EquipmentInventory = uiManager.playerInventoryManager.equipmentInventory;
        for (int i = 0; i < EquipmentInventory.Count; i++)
        {
            //加载出 预制件
            GameObject obj =Instantiate(equipmentDisplay.gameObject, container);
            //初始化   
            UIEquipmentDisplay display = obj.GetComponent<UIEquipmentDisplay>();
            //设置为可见
            display.UpdateUI(EquipmentInventory[i]);
        }
    }
}
