    using UnityEngine;

    [CreateAssetMenu(menuName = "Item Actions/Heavy Attack Action")]
    public class HeavyAttackAction : ItemAction
    {
        public override void PerformAction(PlayerManager player)
        {
            if (player.playerStatsManager.currentStamina <= 0) return;
            
            player.playerAnimatorManager.EraseHandIKForWeapon();
            player.playerEffectsManager.PlayWeaponFX(player.isUsingLeftHand);
            
            // 奔跑攻击
            //if (player.isSprinting)
            //{
            //    HandleRunningAttack(player.playerInventoryManager.rightWeapon, player);
            //    return;
            //}

            if (player.canDoCombo)
            {
                player.inputHandler.comboFlag = true;
                HandleHeavyWeaponCombo(player);
                player.inputHandler.comboFlag = false;
            }
            else
            {
                if (player.isInteracting)
                    return;
                if (player.canDoCombo)
                    return;
                HandleLightAttack(player);
            }
        }
        void HandleLightAttack(PlayerManager player)
        {
            if (player.isUsingLeftHand)
            {
                player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_Heavy_Attack_01, true, 0.1f,
                    false, true);
                player.playerCombatManager.lastAttack = player.playerCombatManager.oh_Heavy_Attack_01;
            }
            else if(player.isUsingRightHand)
            {
                if (player.inputHandler.twoHandFlag)
                {
                    player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.th_Heavy_Attack_01, true);
                    player.playerCombatManager.lastAttack = player.playerCombatManager.th_Heavy_Attack_01;
                }
                else
                {
                    player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_Heavy_Attack_01, true);
                    player.playerCombatManager.lastAttack = player.playerCombatManager.oh_Heavy_Attack_01;
                }
            }
        }

        void HandleRunningAttack( PlayerManager player)
        {
            if (player.isUsingLeftHand)
            {
                player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_running_attack_01, true, 0.1f,
                    false, true);
                player.playerCombatManager.lastAttack = player.playerCombatManager.oh_running_attack_01;
            }
            else if (player.isUsingRightHand)
            {
                if (player.inputHandler.twoHandFlag)
                {
                    player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.th_running_attack_01, true);
                    player.playerCombatManager.lastAttack = player.playerCombatManager.th_running_attack_01;
                }
                else
                {
                    player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_running_attack_01, true);
                    player.playerCombatManager.lastAttack = player.playerCombatManager.oh_running_attack_01;
                }
            }
        }

        void HandleHeavyWeaponCombo( PlayerManager player)
        {
            if (player.inputHandler.comboFlag)
            {
                player.playerAnimatorManager.anim.SetBool("canDoCombo", false);

                if (player.isUsingLeftHand)
                {
                    if (player.playerCombatManager.lastAttack == player.playerCombatManager.oh_Heavy_Attack_01)
                    {
                        player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_Heavy_Attack_02,
                            true, 0.1f, false, true);
                        player.playerCombatManager.lastAttack = player.playerCombatManager.oh_Heavy_Attack_02;
                    }
                    else
                    {
                        player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_Heavy_Attack_01,
                            true, 0.1f, false, true);
                        player.playerCombatManager.lastAttack = player.playerCombatManager.oh_Heavy_Attack_01;
                    }
                }
                else if (player.isUsingRightHand)
                {
                    if (player.isTwoHandingWeapon)
                    {
                        if (player.playerCombatManager.lastAttack == player.playerCombatManager.th_Heavy_Attack_01)
                        {
                            player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.th_Heavy_Attack_02,
                                true);
                            player.playerCombatManager.lastAttack = player.playerCombatManager.th_Heavy_Attack_02;
                        }
                        else
                        {
                            player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.th_Heavy_Attack_01,
                                true);
                            player.playerCombatManager.lastAttack = player.playerCombatManager.th_Heavy_Attack_01;
                        }
                    }
                    else
                    {
                        if (player.playerCombatManager.lastAttack == player.playerCombatManager.oh_Heavy_Attack_01)
                        {
                            player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_Heavy_Attack_02,
                                true);
                            player.playerCombatManager.lastAttack = player.playerCombatManager.oh_Heavy_Attack_02;
                        }
                        else
                        {
                            player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_Heavy_Attack_01,
                                true);
                            player.playerCombatManager.lastAttack = player.playerCombatManager.oh_Heavy_Attack_01;
                        }
                    }
                }
                
            }
        }
    }