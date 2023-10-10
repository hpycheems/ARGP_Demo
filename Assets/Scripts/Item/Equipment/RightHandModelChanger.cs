using System.Collections.Generic;
using UnityEngine;


public class RightHandModelChanger : MonoBehaviour
{
    public List<GameObject> handModels;

    private void Awake()
    {
        GetAllHandModels();
    }
    
    void GetAllHandModels()
    {
        int childrenGameObjects = transform.childCount;
        for (int i = 0; i < childrenGameObjects; i++)
        {
            handModels.Add(transform.GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// 卸载下所有头盔
    /// </summary>
    public void UnEquipAllHandModels()
    {
        foreach (GameObject item in handModels)
        {
            item.SetActive(false);
        }
    }

    /// <summary>
    /// 通过名字装备 头盔
    /// </summary>
    /// <param name="helmetName"></param>
    public void EquipmentHandModelByName(string helmetName)
    {
        for (int i = 0; i < handModels.Count; i++)
        {
            if (handModels[i].name == helmetName)
            {
                handModels[i].SetActive(true);
            }
        }
    }
}
