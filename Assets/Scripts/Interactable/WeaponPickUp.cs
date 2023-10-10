using UnityEngine;


public class WeaponPickUp : Interactable
{
    //物品
    public WeaponItem weapon;
    
    public override void Interact(PlayerManager playerManager)
    {
        base.Interact(playerManager);

        PickUpItem(playerManager);
    }

    /// <summary>
    /// 拾取物品
    /// </summary>
    /// <param name="playerManager"></param>
    void PickUpItem(PlayerManager playerManager)
    {
        //获取必须的 Components
        PlayerInventoryManager playerInventoryManager = playerManager.GetComponent<PlayerInventoryManager>();
        PlayerAnimatorManager playerAnimatorManager = playerManager.GetComponentInChildren<PlayerAnimatorManager>();
        PlayerLocomotionManager playerLocomotionManager = playerManager.GetComponent<PlayerLocomotionManager>();

        //把物品加入到背包
        playerInventoryManager.weaponInventory.Add(weapon);
        //停止移动
        playerLocomotionManager.rigidbody.velocity = Vector3.zero;
        //播放对应动画
        playerAnimatorManager.PlayTargetAnimation("Pick Up Item", true);
        //设置显示UI
        playerManager.itemInteractableGameObject.SetActive(true);
        playerManager.itemInteractableGameObject.GetComponentInParent<InteractableUI>()
            .itemName.text = weapon.name;
        playerManager.itemInteractableGameObject.GetComponentInParent<InteractableUI>()
            .rawImage.texture = weapon.itemIcon.texture;
        //销毁物品
        Destroy(gameObject);
    }
}
