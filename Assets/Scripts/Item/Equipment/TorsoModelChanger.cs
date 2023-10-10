using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public class TorsoModelChanger : MonoBehaviour
{
    [FormerlySerializedAs("helmetModels")] public List<GameObject> torsoModels;

    private void Awake()
    {
        GetAllTorsoModels();
    }
    
    void GetAllTorsoModels()
    {
        int childrenGameObjects = transform.childCount;
        for (int i = 0; i < childrenGameObjects; i++)
        {
            torsoModels.Add(transform.GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// 卸载下所有头盔
    /// </summary>
    public void UnEquipAllTorsoModels()
    {
        foreach (GameObject item in torsoModels)
        {
            item.SetActive(false);
        }
    }

    /// <summary>
    /// 通过名字装备 头盔
    /// </summary>
    /// <param name="torsoName"></param>
    public void EquipmentTorsoModelByName(string torsoName)
    {
        for (int i = 0; i < torsoModels.Count; i++)
        {
            if (torsoModels[i].name == torsoName)
            {
                torsoModels[i].SetActive(true);
            }
        }
    }
}
