using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 客户端主界面的控制 用于关闭、开启主界面的窗口
/// </summary>
public class MainMenuUIManager : MonoBehaviour
{
    public GameObject settingsPanel;
    public GameObject versionInfoPanel;
    public GameObject operationPanel;
    public GameObject newGamePanel;

    private UIFilePanelManager filePanelManager;
    private UISettingsManager settingsManager;
    private NewGameManager newGameManager;
    public void OpenSettingsPanel()
    {
        //settingsPanel.SetActive(true);
        if (settingsPanel == null)
        {
            settingsManager = ResourceManager.Instance
                .GetGameObject("Settings_Panel", ResourceType.UI, transform).GetComponent<UISettingsManager>();
            settingsPanel = settingsManager.gameObject;
        }
        else
        {
            settingsPanel.SetActive(true);
        }
        settingsManager.LoadSettingDataFormGameManager();
    }
    public void CloseSettingsPanel()
    {
        settingsPanel.SetActive(false);
    }

    public void OpenNewGamePanel()
    {
        if (newGamePanel == null)
        {
            newGameManager = ResourceManager.Instance
                .GetGameObject("NewGame_Panel", ResourceType.UI, transform).GetComponent<NewGameManager>();
            newGamePanel = newGameManager.gameObject;
        }
        else
        {
            newGamePanel.SetActive(true);
        }

        StartCoroutine(GameManager.Instance.MaskFade(1, 0, () =>
        {
            AudioManager.Instance.PlayAudio(AudioManager.Instance.newGamePanelAudioClip,false);
        }));
    }
    public void CloseNewGamePanel()
    {
        StartCoroutine(GameManager.Instance.MaskFade(0, 1, () =>
        {
            newGamePanel.SetActive(false);
            StartCoroutine(GameManager.Instance.MaskFade(1, 0, delegate {  }));
        }));
    }

    public void OpenFilePanel()
    {
        if (filePanelManager == null)
        {
            filePanelManager = ResourceManager.Instance.GetGameObject("File_Panel", ResourceType.UI, transform)
                .GetComponent<UIFilePanelManager>();
        }
        else
        {
            filePanelManager.gameObject.SetActive(true);
        }
        StartCoroutine(GameManager.Instance.MaskFade(1, 0, () => { }));
    }

    public void CloseFilePanel()
    {
        filePanelManager.gameObject.SetActive(false);
        StartCoroutine(GameManager.Instance.MaskFade(1, 0, () =>{}));
        
    }


    public void AnyButtonClick()
    {
        versionInfoPanel.SetActive(true);
        operationPanel = ResourceManager.Instance.GetGameObject("Operation_Panel", ResourceType.UI, transform);
    }
}

