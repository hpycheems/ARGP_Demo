using System;
using UnityEngine;

/// <summary>
/// 装备 Item 基类
/// </summary>
[Serializable]
public class EquipmentItem : Item
{
    public EquipmentType type;
    //抵消的伤害
    public float physicalDefense;
}
