using UnityEngine;


public class PlayerCombatManager : MonoBehaviour
{
    //Components
    private PlayerWeaponSlotManager playerWeaponSlotManager;
    private PlayerInventoryManager playerInventoryManager;
    private PlayerEquipmentManager playerEquipmentManager;
    private PlayerAnimatorManager playerAnimatorManager;
    private PlayerManager playerManager;
    private CameraHandler cameraHandler;
    private InputHandler inputHandler;
    private PlayerStatsManager playerStatsManager;
    private PlayerEffectsManager playerEffectsManager;

    [Header("Attack Animations")]
    public string oh_Light_Attack_01 = "OH_Light_Attack_01";
    public string oh_Light_Attack_02 = "OH_Light_Attack_02";
    public string oh_Heavy_Attack_01 = "OH_Heavy_Attack_01";
    public string oh_Heavy_Attack_02 = "OH_Heavy_Attack_02";
    public string oh_running_attack_01 = "OH_Running_Attack_01";
    public string oh_jumping_attack_01 = "OH_Jumping_Attack_01";
    
    
    public string th_Light_Attack_01 = "TH_Light_Attack_01";
    public string th_Light_Attack_02 = "TH_Light_Attack_02";
    public string th_Heavy_Attack_01 = "TH_Heavy_Attack_01";
    public string th_Heavy_Attack_02 = "TH_Heavy_Attack_02";
    public string th_running_attack_01 = "TH_Running_Attack_01";
    public string th_jumping_attack_01 = "TH_Jumping_Attack_01";
    

    private string weapon_art = "Parry";
    
    //Combo Parameter
    public string lastAttack;
    //detection layer
    private LayerMask backStabLayer = 1 << 12;
    private LayerMask riposteLayer = 1 << 13;
    
    private void Awake()
    {
        playerEffectsManager = GetComponent<PlayerEffectsManager>();
        playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
        playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
        playerManager = GetComponent<PlayerManager>();
        inputHandler = GetComponent<InputHandler>();
        playerInventoryManager = GetComponent<PlayerInventoryManager>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        
        playerAnimatorManager = GetComponentInChildren<PlayerAnimatorManager>();
    }

    private void Start()
    {
        cameraHandler = CameraHandler.singleton;
    }

    /// <summary>
    /// 成功施法回调
    /// </summary>
    public void SuccessfullyCastSpell()
    {
        playerInventoryManager.currentSpell.SuccessfullyCastSpell(playerAnimatorManager, playerStatsManager, playerWeaponSlotManager,cameraHandler, playerManager.isUsingLeftHand);
        playerAnimatorManager.anim.SetBool("isFiringSpell", true);
    }

    /// <summary>
    /// 尝试 背刺
    /// </summary>
    public void AttemptBackStabOrRiposte()
    {
        if (playerStatsManager.currentStamina <= 0) return; //没有体力时停止

        RaycastHit hit;

        if (Physics.Raycast(inputHandler.criticalAttackRayCastStarPoint.position,
                transform.TransformDirection(Vector3.forward),
                out hit, 0.5f, backStabLayer))
        {
            CharacterManager enemyCharacterManager = hit.collider.GetComponentInParent<CharacterManager>();
            DamageCollider rightWeapon = playerWeaponSlotManager.rightHandDamageCollider;

            if (enemyCharacterManager != null)
            {
                //开放玩家可设置position
                playerManager.EnableSetPosition();
                //设置玩家位置
                playerManager.transform.position =
                    enemyCharacterManager.backStabDamageCollider.backStabberStandPoint.position;
                //旋转方向
                Vector3 rotationDirection = playerManager.transform.eulerAngles;
                rotationDirection = hit.transform.position - playerManager.transform.position;
                rotationDirection.y = 0;
                rotationDirection.Normalize();
                Quaternion tr = Quaternion.LookRotation(rotationDirection);
                Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 1);
                //旋转玩家到正确的forward
                playerManager.transform.rotation = targetRotation;
                
                //计算伤害
                int criticalDamage = playerInventoryManager.rightWeapon.criticalDamageMultiplier *
                                     rightWeapon.physicalDamage;
                enemyCharacterManager.pendingCriticalDamage = criticalDamage;

                playerAnimatorManager.PlayTargetAnimation("Back Stab", true);
                enemyCharacterManager.GetComponentInChildren<CharacterAnimatorManager>().PlayTargetAnimation("Back Stabbed", true);
                
                //关闭玩家可设置position 通过动画事件取消
                //playerManager.DisableSetPosition();
            }
        }
        else if (Physics.Raycast(inputHandler.criticalAttackRayCastStarPoint.position,
                     transform.TransformDirection(Vector3.forward), out hit, 0.7f, riposteLayer))
        {
            CharacterManager enemyCharacterManager = hit.collider.GetComponentInParent<CharacterManager>();
            DamageCollider rightWeapon = playerWeaponSlotManager.rightHandDamageCollider;

            if (enemyCharacterManager != null && enemyCharacterManager.canBeRiposte)
            {
                //开放玩家可设置position
                playerManager.EnableSetPosition();
                //设置玩家的位置
                playerManager.transform.position = enemyCharacterManager.riposteDamageCollider.ripostedStandPoint.position;
                
                //旋转方向
                Vector3 rotationDirection = playerManager.transform.root.eulerAngles;
                rotationDirection = hit.transform.position - playerManager.transform.position;
                rotationDirection.y = 0;
                rotationDirection.Normalize();
                Quaternion tr = Quaternion.LookRotation(rotationDirection);
                Quaternion targetRotation =
                    Quaternion.Slerp(playerManager.transform.rotation, tr, 500 * Time.deltaTime);
                //旋转玩家到正确的forward
                playerInventoryManager.transform.rotation = targetRotation;
    
                //计算伤害
                int criticalDamage = playerInventoryManager.rightWeapon.criticalDamageMultiplier
                                     * rightWeapon.physicalDamage;
                enemyCharacterManager.pendingCriticalDamage = criticalDamage;

                playerAnimatorManager.PlayTargetAnimation("Riposte", true);
                enemyCharacterManager.GetComponentInChildren<CharacterAnimatorManager>().PlayTargetAnimation("Riposted", true);
            }
        }
    }
}
