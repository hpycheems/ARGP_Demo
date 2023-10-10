using UnityEngine;


/// <summary>
/// 武器槽 脚本挂在在 玩家模型手上
/// </summary>
public class WeaponHolderSlot : MonoBehaviour
{
    //武器加载的父级物体
    public Transform parentOverride;
    public WeaponItem currentWeapon;//当前武器
    public GameObject currentWeaponModel;//当前武器模型
    
    //Weapon Type
    public bool isLeftHandleSlot;
    public bool isRightHandleSlot;
    public bool isBackSlot;

    public void UnloadWeapon()//卸载武器
    {
        if (currentWeaponModel != null)
        {
            currentWeaponModel.SetActive(false);
        }
    }

    public void UnloadWeaponAndDestroy()//卸载 并删除 武器
    {
        UnloadWeapon();
        if (currentWeaponModel != null)
        {
            Destroy(currentWeaponModel);
        }
    }

    /// <summary>
    /// 加载武器
    /// </summary>
    /// <param name="weaponItem"></param>
    public void LoadWeaponModel(WeaponItem weaponItem)
    {
        
        // UnLoadWeapon
        UnloadWeaponAndDestroy();
        if (weaponItem == null) return;
        if(weaponItem.isUnarmed) return;

        GameObject model = Instantiate(weaponItem.modelPrefab);
        if (model != null)
        {
            if (parentOverride != null)
            {
                model.transform.parent = parentOverride;
            }
            else
            {
                model.transform.parent = transform;
            }
            //初始化加载出来的武器模型
            model.transform.localPosition = Vector3.zero;
            model.transform.localRotation = Quaternion.identity;
            model.transform.localScale = Vector3.one;
        }

        currentWeaponModel = model;
    }
}
