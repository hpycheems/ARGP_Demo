using System.Collections.Generic;
using UnityEngine;


public class LowerRightArmModelChanger : MonoBehaviour
{
    public List<GameObject> lowerArmModels;

    private void Awake()
    {
        GetAllLowerArmModels();
    }
    
    void GetAllLowerArmModels()
    {
        int childrenGameObjects = transform.childCount;
        for (int i = 0; i < childrenGameObjects; i++)
        {
            lowerArmModels.Add(transform.GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// 卸载下所有头盔
    /// </summary>
    public void UnEquipAllLowerArmModels()
    {
        foreach (GameObject item in lowerArmModels)
        {
            item.SetActive(false);
        }
    }

    /// <summary>
    /// 通过名字装备 头盔
    /// </summary>
    /// <param name="helmetName"></param>
    public void EquipmentLowerArmModelByName(string helmetName)
    {
        for (int i = 0; i < lowerArmModels.Count; i++)
        {
            if (lowerArmModels[i].name == helmetName)
            {
                lowerArmModels[i].SetActive(true);
            }
        }
    }
}
