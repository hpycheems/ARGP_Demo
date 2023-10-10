using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 游戏管理类
/// </summary>
public class GameManager : MonoBehaviour
{
    #region Singleton

    private GameManager() {}
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
                if (instance == null)
                {
                    GameObject newObj = new GameObject("GameManager");
                    instance = newObj.AddComponent<GameManager>();
                }
            }
            return instance;
        }
    }

    #endregion

    [Header("游戏设定")] 
    public SettingData settingData;
    public string[] resolutions = { "1440x900", "1600x900", "1920x1080" };
    public GameObject manModel;
    public GameObject womanModel;

    public GameObject manPlayer;
    public GameObject womanPlayer;
    
    //存档
    public GameFileData gameFileData;
    public bool haveFile = false;
    private GameFileData continueGameFileData;//此次游玩的游戏存档，当退出游戏时，下次游玩可继续游戏
    public bool haveContinueData;
    
    public CanvasGroup fadeObj;
    private float timer = 0;
    private float fadeDuration = 0.5f;
    
    public UnityAction<int> OnMusicVolumeChange;
    public UnityAction<int> OnSFXVolumeChange;

    PlayerManager player;
    
    //场景加载完毕后 加载必须的组件
    public UnityAction OnSceneLoadSuccess;
    
    
    public bool playerSex = true;

    public GameObject MainCamera;
    public GameObject PlayerUI;
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Init();
        OnSceneLoadSuccess += LoadPlayer;
    }

    /// <summary>
    /// 初始化 加载玩家的用户设置 并应用
    /// </summary>
    private void Init()
    {
        //加载继续游戏存档
        string gameData = PlayerPrefs.GetString(SystemDefine.continueGameDataName);
        if (string.IsNullOrEmpty(gameData))
        {
            Debug.Log("继续游戏存档为空！");
        }
        else
        {
            haveContinueData = true;
            continueGameFileData = JsonUtility.FromJson<GameFileData>(gameData);
        }
        
        // 客户端个性化设置
        SettingData settingData = SaveManager.Instance.Load<SettingData>(SystemDefine.settingSaveDataFile);
        if (settingData == null)//如果是新用户，则使用初始化数据
        {
            SettingData data = new SettingData();
            data.isFullScreen = SystemDefine.defaultIsFullScreen;
            data.musicVolume = SystemDefine.defaultmusicVolume;
            data.sfxVolume = SystemDefine.defaultsfxVolume;
            data.resolutionIndex = SystemDefine.defaultResolutionIndex;
            settingData = data;
        }
        UpdateSettingData(settingData);
        OnMusicVolumeChange(settingData.musicVolume);
        OnSFXVolumeChange(settingData.sfxVolume);
    }

    /// <summary>
    /// 更新用户设置并保存
    /// </summary>
    /// <param name="data"></param>
    public void UpdateSettingData(SettingData data)
    {
        settingData.isFullScreen = data.isFullScreen;
        settingData.musicVolume = data.musicVolume;
        settingData.sfxVolume = data.sfxVolume;
        settingData.resolutionIndex = data.resolutionIndex;
        SaveManager.Instance.Save(SystemDefine.settingSaveDataFile, settingData);
        
        OnMusicVolumeChange(settingData.musicVolume);
        OnSFXVolumeChange(settingData.sfxVolume);
        switch (settingData.resolutionIndex)
        {
            case 0:
                SetResolution(1440, 900, false);
                break;
            case 1:
                SetResolution(1600, 900, false);
                break;
            case 2:
                SetResolution(1920, 1080, settingData.isFullScreen);
                break;
        }
    }
    
    /// <summary>
    /// 设置游戏分辨率
    /// </summary>
    /// <param name="width">长度</param>
    /// <param name="height">高度</param>
    /// <param name="isFullScreen">是否是全屏</param>
    void SetResolution(int width,int height,bool isFullScreen)
    {
        Screen.SetResolution(width, height, isFullScreen);
    }

    /// <summary>
    /// 把当前的游戏进行存档
    /// </summary>
    public void GamingSaveFile()
    {
        PlayerManager playerManager = FindObjectOfType<PlayerManager>();
        if (string.IsNullOrEmpty(gameFileData.fileName))
        {
            gameFileData = new GameFileData();
            string fileName = System.DateTime.Now.ToString().Replace('/', '-').Replace(':', '_');
            fileName = SystemDefine.filePath + fileName + ".sav";
            gameFileData.fileName = fileName;
            gameFileData.sex = playerSex;
        }
        //存档存在则不用更改存档名称
        
        //存档时间
        gameFileData.time = DateTime.Now.ToString(CultureInfo.InvariantCulture);
        //玩家数据
        gameFileData.statsData = playerManager.playerStatsManager.ExportData();
        gameFileData.inventoryData = playerManager.playerInventoryManager.ExportData();
        //所在场景
        gameFileData.scene = SceneLoadingManager.Instance.currentScene;
        //位置信息
        gameFileData.position = playerManager.transform.position;
        gameFileData.rotation = playerManager.transform.rotation;
        //保存数据
        SaveManager.Instance.SaveGameData(gameFileData.fileName, gameFileData);
    }

    /// <summary>
    /// 开始新游戏时 不需要调用它
    /// </summary>
    public void LoadGameSaveFile(GameFileData data)
    {
        if (data == null)
        {
            Debug.Log("存档加载失败！");
            return;
        }
        haveFile = true;
        gameFileData = data;
        //加载游戏场景
        LoadGameScene(gameFileData.scene);
    }
    
    void LoadGameScene(LoadingScene type)
    {
        switch (type)
        {
            case LoadingScene.None:
                break;
            case LoadingScene.NewGameInitScene:
                SceneLoadingManager.Instance.ASyncLoadingScene(SceneType.NewGameInitScene);
                break;
        }
    }

    public void LoadContinueGameData()
    {
        LoadGameSaveFile(continueGameFileData);
    }

    /// <summary>
    /// 加载个性化存档 分辨率 窗口类型
    /// </summary>
    /// <returns></returns>
    public FileInfo[] LoadGameFile()
    {
        string path = SystemDefine.filePath;
        DirectoryInfo root = new DirectoryInfo(path);
        FileInfo[] files = root.GetFiles();
        return files;
    }
    
    
    /// <summary>
    /// 开启新游戏时 加载初始化玩家
    /// </summary>
    void LoadPlayer()
    {
        if (player != null)
        {
            Debug.Log("玩家已经存在");
            return;
        }
        else
        {
            //实例化玩家
            if (playerSex)
            {
                PlayerManager manager = Instantiate(manPlayer, Vector3.zero, Quaternion.identity).GetComponent<PlayerManager>();
            }
            else
            {
                PlayerManager manager = Instantiate(womanModel, Vector3.zero, Quaternion.identity).GetComponent<PlayerManager>();
            }
        }
    }

    /// <summary>
    /// 渐入 
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public IEnumerator MaskFade(int from, int to, UnityAction action)
    {
        timer = 0;
        while (timer <= fadeDuration)
        {
            timer += Time.deltaTime;
            float lerpVal = Mathf.Lerp(from, to, timer / fadeDuration);
            fadeObj.alpha = lerpVal;
            yield return null;
        }

        timer = 0;
        action();
    }
    
    /// <summary>
    /// 创建角色时， 控制模型
    /// </summary>
    /// <param name="isMan"></param>
    public void ChangeDisplayModel(bool isMan)
    {
        
        if (isMan)
        {
            manModel.SetActive(true);
            womanModel.SetActive(false);
        }
        else
        {
            manModel.SetActive(false);
            womanModel.SetActive(true);
        }
    }
    
    /// <summary>
    /// 退出游戏
    /// </summary>
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
