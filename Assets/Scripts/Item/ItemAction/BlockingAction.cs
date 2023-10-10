using UnityEngine;

[CreateAssetMenu(menuName = "Item Actions/Blocking Attack Action")]
public class BlockingAction :ItemAction
{
    public override void PerformAction(PlayerManager player)
    {
        if (player.isInteracting)
            return;
        if (player.isBlocking)
            return;
        
        player.playerAnimatorManager.PlayTargetAnimation("Blocking_Start", false,0.2f,true);
        player.playerEquipmentManager.OpenBlockingCollider();
        player.isBlocking = true;
    }
}