using System.Collections.Generic;
using UnityEngine;


public class HipModelChanger : MonoBehaviour
{
    public List<GameObject> helmetModels;

    private void Awake()
    {
        GetAllHipModels();
    }
    
    void GetAllHipModels()
    {
        int childrenGameObjects = transform.childCount;
        for (int i = 0; i < childrenGameObjects; i++)
        {
            helmetModels.Add(transform.GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// 卸载下所有头盔
    /// </summary>
    public void UnEquipAllHipModels()
    {
        foreach (GameObject item in helmetModels)
        {
            item.SetActive(false);
        }
    }

    /// <summary>
    /// 通过名字装备 头盔
    /// </summary>
    /// <param name="helmetName"></param>
    public void EquipmentHipModelByName(string helmetName)
    {
        for (int i = 0; i < helmetModels.Count; i++)
        {
            if (helmetModels[i].name == helmetName)
            {
                helmetModels[i].SetActive(true);
            }
        }
    }
}
