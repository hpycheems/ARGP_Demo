using UnityEngine;


public class EquipmentWindowUI : MonoBehaviour
{
    public bool rightHandSlot01Select;
    public bool rightHandSlot02Select;
    public bool leftHandSlot01Select;
    public bool leftHandSlot02Select;

    public HandEquipmentSlotUI[] handEquipmentSlotUI;
    
    public void LoadWeaponsOnEquipmentScreen(PlayerInventoryManager playerInventoryManager)
    {
        for (int i = 0; i < handEquipmentSlotUI.Length; i++)
        {
            if (handEquipmentSlotUI[i].rightHandSlot01)
            {
                handEquipmentSlotUI[i].AddItem(playerInventoryManager.weaponsInRightHandSlots[0]);
            }
            else if (handEquipmentSlotUI[i].rightHandSlot02)
            {
                handEquipmentSlotUI[i].AddItem(playerInventoryManager.weaponsInRightHandSlots[1]);
            }
            else if (handEquipmentSlotUI[i].leftHandSlot01)
            {
                handEquipmentSlotUI[i].AddItem(playerInventoryManager.weaponsInLeftHandSlots[0]);
            }
            else if (handEquipmentSlotUI[i].leftHandSlot02)
            {
                handEquipmentSlotUI[i].AddItem(playerInventoryManager.weaponsInLeftHandSlots[1]);
            }
        }
    }

    public void SelectRightHandSlot01()
    {
        rightHandSlot01Select = true;
        handEquipmentSlotUI[0].SelectThisSlot();
    }

    public void SelectRightHandSlot02()
    {
        rightHandSlot02Select = true;
        handEquipmentSlotUI[1].SelectThisSlot();
    }

    public void SelectLeftHandSlot01()
    {
        leftHandSlot01Select = true;
        handEquipmentSlotUI[2].SelectThisSlot();
    }

    public void SelectLeftHandSlot02()
    {
        leftHandSlot02Select = true;
        handEquipmentSlotUI[3].SelectThisSlot();
    }
}
