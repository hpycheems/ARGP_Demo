using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 按任意键继续游戏
/// </summary>
public class UIAnyShingStarGame : MonoBehaviour
{
    private MainMenuUIManager mainMenuUIManager;
    private void Awake()
    {
        mainMenuUIManager = FindObjectOfType<MainMenuUIManager>();
    }
    void Update()
    {
        if (Input.anyKeyDown)
        {
            AudioManager.Instance.OnButtonClickAudio();
            mainMenuUIManager.AnyButtonClick();
            Destroy(gameObject);
        }
    }
}
