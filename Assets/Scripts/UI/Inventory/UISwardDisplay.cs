using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISwardDisplay : UIBaseDisplay
{
    public void UpdateUI(WeaponItem item)
    {
        name.text = item.itemName;//物品名称
        icon.sprite = item.itemIcon;//物品图标
        displayText.text = item.display;//物品描述
    }
}
