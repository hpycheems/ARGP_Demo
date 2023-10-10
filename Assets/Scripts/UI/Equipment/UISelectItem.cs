using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISelectItem : UIBaseDisplay
{
    public Item item;
    private UISelectPanel selectPanel;
    private Button button;
    private void Awake()
    {
        selectPanel = GetComponentInParent<UISelectPanel>();
        button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            selectPanel.SelectItem(item);
        });
    }
    public void UpdateUI()
    {
        if (item != null)
        {
            name.text = item.itemName;
            displayText.text = item.display;
            icon.sprite = item.itemIcon;
        }
    }
}
