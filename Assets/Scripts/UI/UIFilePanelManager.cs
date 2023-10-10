using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class UIFilePanelManager : MonoBehaviour
{
    public Button closeButton;
    public Sprite Icon;

    private MainMenuUIManager mainMenuOperationManager;

    private FileInfo[] gameDatas;
    private List<GameFileData> datas = new List<GameFileData>(); 
    private int dataCount = 0;

    private List<UIFileData> fileDataComponents = new List<UIFileData>();
    private void Awake()
    {
        mainMenuOperationManager = GetComponentInParent<MainMenuUIManager>();
        gameDatas = GameManager.Instance.LoadGameFile();
        UIFileData[] uiFileDatas = GetComponentsInChildren<UIFileData>();
        for (int i = 0; i < uiFileDatas.Length; i++)
        {
            fileDataComponents.Add(uiFileDatas[i]);
        }
    }

    private void OnEnable()
    {
        HandingFile();
        HandingComponent();
    }

    private void Start()
    {
        closeButton.onClick.AddListener(() =>
        {
            AudioManager.Instance.OnButtonClickAudio();
            closeButton.GetComponent<UIEffectBase>().effectObject.SetActive(false);
            mainMenuOperationManager.CloseFilePanel();
        });
    }

    void HandingFile()
    {
        datas.Clear();
        //再加载
        for (int i = 0; i < gameDatas.Length; i++)
        {
            if (dataCount >= 5)
            {
                return;
            }
            
            if (gameDatas[i].FullName.Contains(".sav"))
            {
                byte[] fileData = new byte[1024];
                int count = 0;
                using (FileStream fileStream = gameDatas[i].OpenRead())
                {
                    count = fileStream.Read(fileData, 0, fileData.Length);
                }

                string data = System.Text.Encoding.UTF8.GetString(fileData,0, count);
                datas.Add(JsonUtility.FromJson<GameFileData>(data));
                dataCount++;
            }
        }
    }

    void HandingComponent()
    {
        for (int i = 0; i < fileDataComponents.Count; i++)
        {
            fileDataComponents[i].UpdateDisplay(null);
        }
        for (int i = 0; i < datas.Count; i++)
        {
            GameFileData data = datas[i];
            fileDataComponents[i].UpdateDisplay(Icon, data);
        }
    }
}
