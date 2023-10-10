using System;
using UnityEngine;
using UnityEngine.UI;


public class WeaponInventorySlot : MonoBehaviour
{
    //Components
    private PlayerInventoryManager playerInventoryManager;
    private PlayerWeaponSlotManager playerWeaponSlotManager;
    private UIManager uiManager;
    
    public Image icon;
    [SerializeField] private WeaponItem item;

    private void Awake()
    {
        playerInventoryManager = FindObjectOfType<PlayerInventoryManager>();
        playerWeaponSlotManager = FindObjectOfType<PlayerWeaponSlotManager>();
        uiManager = GetComponentInParent<UIManager>();
    }

    public void AddItem(WeaponItem newItem)
    {
        item = newItem;
        icon.sprite = item.itemIcon;
        icon.enabled = true;
        gameObject.SetActive(true);
    }

    public void ClearInventorySlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        gameObject.SetActive(false);
    }
    
    public void EquipThisItem()
    {
        if (uiManager.rightHandSlot01Selected)
        {
            if (playerInventoryManager.weaponsInRightHandSlots[0] != null)
            {
                playerInventoryManager.weaponInventory.Add(playerInventoryManager.weaponsInRightHandSlots[0]);
            }

            playerInventoryManager.weaponsInRightHandSlots[0] = item;
            playerInventoryManager.weaponInventory.Remove(item);
            playerInventoryManager.rightWeapon = item;
        }
        else if (uiManager.rightHandSlot02Selected)
        {
            if (playerInventoryManager.weaponsInRightHandSlots[1] != null)
            {
                playerInventoryManager.weaponInventory.Add(playerInventoryManager.weaponsInRightHandSlots[1]);
            }

            playerInventoryManager.weaponsInRightHandSlots[1] = item;
            playerInventoryManager.weaponInventory.Remove(item);
        }
        else if (uiManager.leftHandSlot01Selected)
        {
            if (playerInventoryManager.weaponsInLeftHandSlots[0] != null)
            {
                playerInventoryManager.weaponInventory.Add(playerInventoryManager.weaponsInLeftHandSlots[0]);
            }

            playerInventoryManager.weaponsInLeftHandSlots[0] = item;
            playerInventoryManager.weaponInventory.Remove(item);
            playerInventoryManager.leftWeapon = item;
        }
        else if (uiManager.leftHandSlot02Selected)
        {
            if (playerInventoryManager.weaponsInLeftHandSlots[1] != null)
            {
                playerInventoryManager.weaponInventory.Add(playerInventoryManager.weaponsInLeftHandSlots[1]);
            }

            playerInventoryManager.weaponsInLeftHandSlots[1] = item;
            playerInventoryManager.weaponInventory.Remove(item);
        }
        else
        {
            return;
        }

        //Debug.Log("!!! " + playerInventoryManager.rightWeapon.isUnarmed);
        
        if (!playerInventoryManager.rightWeapon.isUnarmed)
        {
            playerInventoryManager.rightWeapon =
                playerInventoryManager.weaponsInRightHandSlots[playerInventoryManager.currentRightWeaponIndex];
            playerWeaponSlotManager.LoadWeaponOnSlot(playerInventoryManager.rightWeapon, false);
        }
        else if (!playerInventoryManager.leftWeapon.isUnarmed)
        {
            playerInventoryManager.leftWeapon =
                playerInventoryManager.weaponsInLeftHandSlots[playerInventoryManager.currentLeftWeaponIndex];
            playerWeaponSlotManager.LoadWeaponOnSlot(playerInventoryManager.leftWeapon, true);
        }

        uiManager.equipmentWindowUI.LoadWeaponsOnEquipmentScreen(playerInventoryManager);
        uiManager.ResetAllSelectedSlots();
        uiManager.CloseInventoryWindowAndOpenEquipmentWindow();
    }
}
