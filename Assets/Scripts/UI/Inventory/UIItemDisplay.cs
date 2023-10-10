using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIItemDisplay : UIBaseDisplay
{
    public TMP_Text count;
    
    public void UpdateUI(ConsumableItem item)
    {
        name.text = item.itemName;//物品名称
        count.text = item.currentItemAmount.ToString();//物品数量
        icon.sprite = item.itemIcon;//物品图标
        displayText.text = item.display;//物品描述
    }
}
