using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public class UpperRightArmModelChanger : MonoBehaviour
{
    public List<GameObject> upperArmModels;

    private void Awake()
    {
        GetAllUpperArmModels();
    }
    
    void GetAllUpperArmModels()
    {
        int childrenGameObjects = transform.childCount;
        for (int i = 0; i < childrenGameObjects; i++)
        {
            upperArmModels.Add(transform.GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// 卸载下所有头盔
    /// </summary>
    public void UnEquipAllUpperArmModels()
    {
        foreach (GameObject item in upperArmModels)
        {
            item.SetActive(false);
        }
    }

    /// <summary>
    /// 通过名字装备 头盔
    /// </summary>
    /// <param name="helmetName"></param>
    public void EquipmentUpperArmModelByName(string helmetName)
    {
        for (int i = 0; i < upperArmModels.Count; i++)
        {
            if (upperArmModels[i].name == helmetName)
            {
                upperArmModels[i].SetActive(true);
            }
        }
    }
}
