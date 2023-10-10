using UnityEngine;

[CreateAssetMenu(menuName = "Item Actions/Pyromancy Spell Action")]
public class PyromancySpellAction : ItemAction
{
    public override void PerformAction(PlayerManager player)
    {
        if (player.isInteracting) return;
        
        if (player.playerInventoryManager.currentSpell != null && player.playerInventoryManager.currentSpell.isFaithSpell)//当前装备的是新年魔法
        {
            if (player.playerStatsManager.currentFocusPoints >= player.playerInventoryManager.currentSpell.focusPointCost)//有足够的魔法
            {
                player.playerInventoryManager.currentSpell.AttemptToCastSpell(
                    player.playerAnimatorManager,
                    player.playerStatsManager,
                    player.playerWeaponSlotManager,
                    player.isUsingLeftHand);
            }
            else
            {
                player.playerAnimatorManager.PlayTargetAnimation("Shrug",true);
            }
        }
    }
}