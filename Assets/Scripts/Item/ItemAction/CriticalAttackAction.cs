using UnityEngine;

[CreateAssetMenu(menuName = "Item Actions/Critical Action")]
public class CriticalAttackAction : ItemAction
{
    public override void PerformAction(PlayerManager player)
    {
        if(player.isInteracting)
            return;
        
        player.playerCombatManager.AttemptBackStabOrRiposte();
    }
}