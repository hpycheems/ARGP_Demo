using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Resource 资源加载管理器
/// </summary>
public class ResourceManager : MonoBehaviour
{
    #region Singleton

    private ResourceManager() {}
    private static ResourceManager instance;
    public static ResourceManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ResourceManager>();
                if (instance == null)
                {
                    GameObject newObj = new GameObject("ResourceManager");
                    instance = newObj.AddComponent<ResourceManager>();
                }
            }
            return instance;
        }
    }

    #endregion
    
    public Dictionary<string, GameObject> gameObjectDic = new Dictionary<string, GameObject>();
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    
    /// <summary>
    /// 给用户提供的 从Resource文件中加载的 游戏对象
    /// </summary>
    /// <param name="name"></param>
    /// <param name="type"></param>
    /// <param name="isCache"></param>
    /// <returns></returns>
    public GameObject GetGameObject(string name, ResourceType type, Transform parent = null,bool isCache = false)
    {
        GameObject resultObj = null;
        if (gameObjectDic.ContainsKey(name))
        {
            resultObj = gameObjectDic[name];
        }
        else
        {
            resultObj = LoadResource(name, type, isCache);
        }
        
        if (parent !=  null)
        {
            return Instantiate(resultObj, parent);
        }
        else
        {
            return Instantiate(resultObj);
        }
    }

    GameObject LoadResource(string name, ResourceType type, bool isCache = false)
    {
        GameObject loadObj = null;
        string path = ChoiceResource(type, name);
        if (!string.IsNullOrEmpty(path))
        {
            loadObj = InstantiateGameObject(path);
        }
        else
        {
            Debug.Log("未能加载到资源！");
        }

        if (isCache)
        {
            gameObjectDic.Add(name, loadObj);
        }

        return loadObj;
    }
    string ChoiceResource(ResourceType type, string name)
    {
        string path = "";
        switch (type)
        {
            case ResourceType.UI:
                path = "UI/" + name;
                break;
        }
        return path;
    }
    GameObject InstantiateGameObject(string path)
    {
        GameObject newObj = null;
        newObj = Resources.Load<GameObject>(path);
        return newObj;
    }
}
