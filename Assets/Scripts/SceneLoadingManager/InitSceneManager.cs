using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏程序启动场景管理器
/// 开始游戏 就会加载到 MainMenu场景
/// </summary>
public class InitSceneManager : MonoBehaviour
{
    private void Start()
    {
        SceneLoadingManager.Instance.ASyncLoadingScene(SceneType.MainMenuScene);
    }
}
