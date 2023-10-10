using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEquipmentDisplay : UIBaseDisplay
{
    public void UpdateUI(EquipmentItem item)
    {
        name.text = item.itemName;//物品名称
        icon.sprite = item.itemIcon;//物品图标
        displayText.text = item.display;//物品描述
    }
}
