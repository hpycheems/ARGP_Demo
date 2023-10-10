using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGameInitSceneManager : MonoBehaviour
{
    private void Awake()
    {
        GameManager.Instance.OnSceneLoadSuccess();
    }

    private void Start()
    {
        //设置该场景为 活动场景
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("NewGameInitScene"));
        //卸载场景
        StartCoroutine(SceneLoadingManager.Instance.UnloadLastScene());
        //设置当前场景
        SceneLoadingManager.Instance.currentScene = LoadingScene.NewGameInitScene;
    }
}
