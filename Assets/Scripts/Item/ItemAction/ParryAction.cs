using UnityEngine;

[CreateAssetMenu(menuName = "Item Actions/Parry Action")]
public class ParryAction : ItemAction
{
    public override void PerformAction(PlayerManager player)
    {
        if (player.isInteracting)
            return;
        
        player.playerAnimatorManager.EraseHandIKForWeapon();
        
        WeaponItem parringWeapon = player.playerInventoryManager.currentItemBeingUsed as WeaponItem;
        if (parringWeapon.weaponType == WeaponType.SmallShield)
        {
            player.playerAnimatorManager.PlayTargetAnimation("Parry", true);
        }
        else if(parringWeapon.weaponType != WeaponType.Shield)
        {   player.playerAnimatorManager.PlayTargetAnimation("Parry", true);
        }
    }
}