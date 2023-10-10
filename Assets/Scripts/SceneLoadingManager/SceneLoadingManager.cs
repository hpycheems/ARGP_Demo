using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadingManager : MonoBehaviour
{
    #region Singleton

    private SceneLoadingManager() {}
    private static SceneLoadingManager instance;
    public static SceneLoadingManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SceneLoadingManager>();
                if (instance == null)
                {
                    GameObject newObj = new GameObject("SceneLoadingManager");
                    instance = newObj.AddComponent<SceneLoadingManager>();
                }
            }
            return instance;
        }
    }

    #endregion
    
    public SceneType nextSceneType = SceneType.None;
    public string lastSceneName;

    public LoadingScene currentScene;
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    public void ASyncLoadingScene(SceneType type)
    {
        nextSceneType = type;
        lastSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("LoadingScene", LoadSceneMode.Additive);
    }
    
    public IEnumerator UnloadLastScene()
    {
        AsyncOperation async = SceneManager.UnloadSceneAsync(lastSceneName);
        if (!async.isDone)
        {
            float percent = async.progress;
            //UpdateLoadingImage(percent);
            yield return null;
        }
        lastSceneName = "LoadingScene";
        //加载目标场景
        StartCoroutine(LoadTargetScene());
    }

    public IEnumerator LoadTargetScene()
    {
        string sceneName = "";
        switch (Instance.nextSceneType)
        {
            case SceneType.None:
                break;
            case SceneType.MainMenuScene:
                sceneName = "LoginScene";
                break;
            case SceneType.NewGameInitScene:
                sceneName = "NewGameInitScene";
                break;
        }

        if (sceneName == SceneManager.GetActiveScene().name)
        {
            //啥也不做
        }
        else
        {
            AsyncOperation async = SceneManager.LoadSceneAsync(sceneName,LoadSceneMode.Additive);
            while (!async.isDone)
            {
                yield return null;
            }
        }
    }
}
