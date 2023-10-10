using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceenManager : MonoBehaviour
{
    private void Start()
    {
        //设置该场景为 活动场景
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("LoadingScene"));
        //卸载场景
        StartCoroutine(SceneLoadingManager.Instance.UnloadLastScene());
    }
    
    
}
