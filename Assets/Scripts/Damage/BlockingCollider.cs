using UnityEngine;

/// <summary>
/// 防御碰撞体
/// </summary>
public class BlockingCollider : MonoBehaviour
{
    public BoxCollider blockingCollider;
    
    public float blockingPhysicalDamageAbsorption;//防御时抵消的伤害百分比
    public float blockingFireDamageAbsorption;

    private void Awake()
    {
        blockingCollider = GetComponent<BoxCollider>();
    }

    public void SetColliderDamageAbsorption(WeaponItem weaponItem)//通过武器Item 设置抵消的伤害百分比
    {
        if (weaponItem != null)
        {
            blockingPhysicalDamageAbsorption = weaponItem.physicalDamageAbsorption;
        }
    }

    #region signal

    public void EnableBlockingCollider()
    {
        blockingCollider.enabled = true;
    }
    public void DisableBlockingCollider()
    {
        blockingCollider.enabled = false;
    }

    #endregion
    
}
