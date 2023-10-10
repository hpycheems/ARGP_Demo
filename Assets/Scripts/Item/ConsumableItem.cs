using UnityEngine;
using UnityEngine.Events;


public class ConsumableItem : Item
{
    [Header("Item Quantity")]
    public int maxItemAmount;
    public int currentItemAmount;
    
    [Header("Item Mode")]
    public GameObject itemModel;

    [Header("Animations")]
    public string consumeAnimation;
    public bool isInteracting;

    public UnityAction<string> OnChangeAmount;

    /// <summary>
    /// 尝试 使用道具
    /// </summary>
    /// <param name="animatorManager"></param>
    /// <param name="playerWeaponSlotManager"></param>
    /// <param name="playerEffectsManager"></param>
    public virtual void AttemptToConsumeItem(PlayerAnimatorManager animatorManager, 
        PlayerWeaponSlotManager playerWeaponSlotManager, PlayerEffectsManager playerEffectsManager)
    {
        if (currentItemAmount > 0)//存在道具
        {
            animatorManager.PlayTargetAnimation(consumeAnimation, isInteracting, 0.2f,true);
            currentItemAmount -= 1;
            if (OnChangeAmount != null)
            {
                OnChangeAmount(currentItemAmount.ToString());
            }
            //OnChangeAmount(currentItemAmount.ToString());
        }
        else//道具为零
        {
            animatorManager.PlayTargetAnimation("Shrug", true);
        }
    }
}
