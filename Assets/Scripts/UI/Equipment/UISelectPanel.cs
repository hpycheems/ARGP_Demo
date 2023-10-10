using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UISelectPanel : MonoBehaviour
{
    public UIManager uiManager;
    public UnityAction<Item> onChangeClick;
    public Transform instantiateParent;
    public GameObject instantiatePrefab;
    
    private void Awake()
    {
        uiManager = GetComponentInParent<UIManager>();
    }
    
    /// <summary>
    /// 生成筛选出来的装备
    /// </summary>
    /// <param name="item"></param>
    public void InitItem(Item[] items)
    {
        //先清除
        ClearInstantiate();
        //显示窗口
        gameObject.SetActive(true);
        foreach (Item item in items)
        {
            GameObject obj = Instantiate(instantiatePrefab, instantiateParent);
            UISelectItem selectItem = obj.GetComponent<UISelectItem>();
            selectItem.item = item;
            selectItem.UpdateUI();
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))//右键退出选择面板
        {
            ClosePanel();
        }
    }

    public void SelectItem(Item item)
    {
        onChangeClick(item);
        ClosePanel();
    }

    void ClearInstantiate()
    {
        for (int i = 0; i < instantiateParent.childCount; i++)
        {
            Destroy(instantiateParent.transform.GetChild(i).gameObject);
        }
    }

    void ClosePanel()
    {
        uiManager.ResetAllSelectedSlots();
        uiManager.OpenEquipmentPanel();
        gameObject.SetActive(false);
    }
}
