using System.Collections.Generic;
using UnityEngine;


public class RightLegModelChanger : MonoBehaviour
{
    public List<GameObject> legModels;

    private void Awake()
    {
        GetAllLegModels();
    }
    
    void GetAllLegModels()
    {
        int childrenGameObjects = transform.childCount;
        for (int i = 0; i < childrenGameObjects; i++)
        {
            legModels.Add(transform.GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// 卸载下所有头盔
    /// </summary>
    public void UnEquipAllLegModels()
    {
        foreach (GameObject item in legModels)
        {
            item.SetActive(false);
        }
    }

    /// <summary>
    /// 通过名字装备 头盔
    /// </summary>
    /// <param name="helmetName"></param>
    public void EquipmentLegModelByName(string helmetName)
    {
        for (int i = 0; i < legModels.Count; i++)
        {
            if (string.Equals( legModels[i].name , helmetName))
            {
                legModels[i].SetActive(true);
            }
        }
    }
}
