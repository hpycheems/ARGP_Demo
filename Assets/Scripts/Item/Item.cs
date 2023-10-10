using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 物品类基类
/// </summary>
[Serializable]
public class Item : ScriptableObject
{
    [Header("Item Information")] 
    public Sprite itemIcon;
    public string itemName;
    [Header("描述"),TextArea] 
    public string display;
}
