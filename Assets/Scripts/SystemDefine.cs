using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 系统定义的字通用数据
/// </summary>
public static class SystemDefine
{
    //存档文件
    public static string settingSaveDataFile = "SettingData.sav";
    
    //视图
    public static int defaultResolutionIndex = 2;
    public static bool defaultIsFullScreen = true;
    
    //音频
    public static int defaultmusicVolume = 50;
    public static int defaultsfxVolume = 50;
    
    //游戏存档 路径
    public static string filePath = Application.persistentDataPath + "/GameFile/";
    public static string continueGameDataName = "continueGameData";
}

public enum ResourceType
{
    UI
}

public enum SceneType
{
    None,
    MainMenuScene,
    LoadingScene,
    NewGameInitScene,
}

public enum LoadingScene
{
    None,
    NewGameInitScene,
}

public enum SelectEquipmentSlotType
{
    Weapon,
    Consumable,
    Equipment
}

public enum EquipmentType{
    None,
    Helmet,
    Torso,
    Hand,
    Hip
}
