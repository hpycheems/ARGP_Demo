using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{

    #region Singleton

    private static SaveManager instance;

    public static SaveManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SaveManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("SaveManager");
                    instance = obj.AddComponent<SaveManager>();
                }
            }
            return instance;
        }
    }

    #endregion

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// 保存数据到文件中
    /// </summary>
    /// <param name="fileName">文件名</param>
    /// <param name="data"></param>
    public void Save(string fileName, object data)
    {
        string path = Path.Combine(Application.persistentDataPath, fileName);
        try
        {
            string jsonData = JsonUtility.ToJson(data);
            File.WriteAllText(path, jsonData);
            Debug.Log("data Save to : "+path);
        }
        catch (Exception e)
        {
            Debug.Log("Save Data fail path:" + path);
            throw;
        }
        
    }

    public void SaveGameData(string fileName, object data)
    {
        try
        {
            string jsonData = JsonUtility.ToJson(data);
            File.WriteAllText(fileName, jsonData);
            //将数据存入继续游戏的存档
            PlayerPrefs.SetString(SystemDefine.continueGameDataName, jsonData);
            PlayerPrefs.Save();
            
            Debug.Log("data Save to : "+fileName);
        }
        catch (Exception e)
        {
            Debug.Log("Save Data fail path:" + fileName);
            throw;
        }
    }

    /// <summary>
    /// 从文件中加载数据
    /// </summary>
    /// <param name="fileName">文件名</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T Load<T>(string fileName)
    {
        string path = Path.Combine(Application.persistentDataPath, fileName);
        string jsonData = "";
        try
        {
            jsonData = File.ReadAllText(path);
        }
        catch (Exception e)
        {
            Debug.Log("Load Data fail Path："+ path);
        }
        T data = JsonUtility.FromJson<T>(jsonData);
        return data;
    }
}
