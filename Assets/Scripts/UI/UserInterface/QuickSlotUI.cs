using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlotUI : MonoBehaviour
{
    public Image currentSpellIcon;
    public Image currentConsumableIcon;
    public Image leftWeaponIcon;
    public Image rightWeaponIcon;
    public TMP_Text consumableAmount;
    
    /// <summary>
    /// 更新 武器槽 到 Quick Slot
    /// </summary>
    /// <param name="weapon"></param>
    /// <param name="isLeft"></param>
    public void UpdateWeaponQuickSlotsUI(WeaponItem weapon, bool isLeft)
    {
        if (isLeft)
        {
            if (weapon != null)
            {
                leftWeaponIcon.sprite = weapon.itemIcon;
                leftWeaponIcon.enabled = true;
            }
            else
            {
                leftWeaponIcon.sprite = null;
                leftWeaponIcon.enabled = false;
            }
        }
        else
        {
            if (weapon != null)
            {
                rightWeaponIcon.sprite = weapon.itemIcon;
                rightWeaponIcon.enabled = true;
            }
            else
            {
                rightWeaponIcon.sprite = null;
                rightWeaponIcon.enabled = false;
            }
            
        }
    }

    /// <summary>
    /// 更新 Quick Slot Consumable
    /// </summary>
    /// <param name="consumable"></param>
    public void UpdateCurrentConsumableIcon(ConsumableItem consumable)
    {
        if (consumable == null) return;
        
        if (consumable.itemIcon != null)
        {
            currentConsumableIcon.sprite = consumable.itemIcon;
            currentConsumableIcon.enabled = true;
            consumableAmount.gameObject.SetActive(true);
            consumableAmount.text = consumable.currentItemAmount.ToString();
            consumable.OnChangeAmount += UpdateConsumableAmount;
        }
        else
        {
            currentConsumableIcon.sprite = null;
            currentConsumableIcon.enabled = false;
            consumableAmount.gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// 回调函数
    /// </summary>
    /// <param name="amount"></param>
    void UpdateConsumableAmount(string amount)
    {
        consumableAmount.text = amount;
    }
    
    
    
    public void UpdateCurrentSpellIcon(SpellItem spell)
    {
        if (spell == null) return;
        
        if (spell.itemIcon != null)
        {
            consumableAmount.gameObject.SetActive(true);
            currentSpellIcon.sprite = spell.itemIcon;
            currentSpellIcon.enabled = true;
        }
        else
        {
            currentSpellIcon.sprite = null;
            currentSpellIcon.enabled = false;
        }
    }
}
