using UnityEngine;
using UnityEngine.Serialization;


/// <summary>
/// Enemy攻击状态
/// </summary>
public class AttackState : State
{
    /// <summary>
    /// 战斗状态
    /// </summary>
    public CombatStanceState combatStanceState;
    /// <summary>
    /// 当前攻击action
    /// </summary>
    public EnemyAttackAction currentAttack;
    /// <summary>
    /// 追逐状态
    /// </summary>
    public PursueTargetState pursueTargetState;
    /// <summary>
    /// 旋转状态
    /// </summary>
    public RotateTowardsTargetState rotateTowardsTargetState;

    
    public bool hasPerformedAttack;
    [SerializeField] bool willDoComboOnNextAttack;
    
    public override State Tick(EnemyManager enemyManager, EnemyStatsManager enemyStatsManager, EnemyAnimatorManager enemyAnimatorManager)
    {
        enemyAnimatorManager.anim.SetFloat("Vertical", 0);
        enemyAnimatorManager.anim.SetFloat("Horizontal", 0);

        if (enemyManager.isParried) return this;
        
        //计算距离
        float distanceFromTarget =
            Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
        //旋转
        RotateToWardsTargetWhilstAttacking(enemyManager);
        //如果距离大于攻击半径 则转换到追逐状态
        if (distanceFromTarget > enemyManager.maximumAggroRadius)
        {
            return pursueTargetState;
        }
        
        if (!hasPerformedAttack)
        {
            hasPerformedAttack = true;
            AttackTarget(enemyAnimatorManager, enemyManager);
            RollForComboChance(enemyManager);
        }
        
        if (willDoComboOnNextAttack && enemyManager.canDoCombo)
        {
            AttackTargetWithCombo(enemyAnimatorManager, enemyManager);
        }
        
        if (willDoComboOnNextAttack && hasPerformedAttack)
        {
            return this;
        }
        
        return rotateTowardsTargetState;
    }

    void AttackTargetWithCombo(EnemyAnimatorManager enemyAnimatorManager, EnemyManager enemyManager)
    {
        willDoComboOnNextAttack = false;
        enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true, 0.1f);
        enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
        currentAttack = null;
    }

    void AttackTarget(EnemyAnimatorManager enemyAnimatorManager, EnemyManager enemyManager)
    {
        enemyManager.enemyInventoryManager.currentItemBeingUsed = enemyManager.enemyInventoryManager.rightWeapon;
        enemyManager.isUsingRightHand = true;
        enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
        enemyAnimatorManager.PlayWeaponTrial();
        enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
        enemyManager.isPerformingAction = true;
    }
    
    /// <summary>
    /// 随机连击
    /// </summary>
    /// <param name="enemyManager"></param>
   private void RollForComboChance(EnemyManager enemyManager)
   {
       float comboChance = Random.Range(0, 100);
       if (enemyManager.allowAIToPerformCombos && comboChance <= enemyManager.comboLikelyHood)
       {
           if (currentAttack.comboAction != null && currentAttack.canCombo)
           {
               willDoComboOnNextAttack = true;
               currentAttack = currentAttack.comboAction;
           }
           else
           {
               willDoComboOnNextAttack = false;
               currentAttack = null;
           }
       }
   }

    void RotateToWardsTargetWhilstAttacking(EnemyManager enemyManager)
    {
        if (enemyManager.canRotate && enemyManager.isInteracting)
        {
            Vector3 direction = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
            direction.y = 0;
            direction.Normalize();
            
            if (direction == Vector3.zero)
            {
                direction = transform.forward;
            }
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, targetRotation,
                enemyManager.rotationSpeed * Time.deltaTime);
        }
    }
}
