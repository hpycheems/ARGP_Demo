using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameFileData
{
    //存档的名称
    public string fileName;

    public bool sex;
    
    public string time ;//存档的时间
    public LoadingScene scene ;//玩家所在的场景
    public PlayerStateFileData statsData ;//玩家的状态
    public PlayerInvetoryFileData inventoryData ;//玩家的背包

    public Vector3 position = Vector3.zero;//玩家位置
    public Quaternion rotation = Quaternion.identity;//玩家旋转
}
