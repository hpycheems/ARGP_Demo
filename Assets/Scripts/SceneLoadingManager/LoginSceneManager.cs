using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginSceneManager : MonoBehaviour
{
   
    void Start()
    {
        //设置该场景为 活动场景
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("LoginScene"));
        //卸载场景
        StartCoroutine(SceneLoadingManager.Instance. UnloadLastScene());
    }
}
