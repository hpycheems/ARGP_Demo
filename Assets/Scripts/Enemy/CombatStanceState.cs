using System.Globalization;
using UnityEngine;


public class CombatStanceState : State
{
    //追逐状态
    public PursueTargetState pursueTargetState;
    //攻击状态
    public AttackState attackState;
    //攻击行为
    public EnemyAttackAction[] enemyAttacks;

    private bool randomDestiantionSet = true;
    private float verticalMovementValue;
    private float horizontalMovementValue;
    
    public override State Tick(EnemyManager enemyManager, EnemyStatsManager enemyStatsManager, EnemyAnimatorManager enemyAnimatorManager)
    {
        if (enemyManager.isInteracting)
        {
            enemyAnimatorManager.anim.SetFloat("Vertical", 0);
            enemyAnimatorManager.anim.SetFloat("Horizontal", 0);
            return this;
        }
        attackState.hasPerformedAttack = false;
        
        //计算与target之间的距离
        float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position,
            enemyManager.transform.position);
        
        enemyAnimatorManager.anim.SetFloat("Vertical", verticalMovementValue,0.2f,Time.deltaTime);
        enemyAnimatorManager.anim.SetFloat("Horizontal", horizontalMovementValue,0.2f,Time.deltaTime);
        
        //attackState.hasPerformedAttack = false;
        if (distanceFromTarget > enemyManager.maximumAggroRadius)//距离大于攻击半径，返回追逐状态
        {
            return pursueTargetState;
        }

        if (!randomDestiantionSet)
        {
            randomDestiantionSet = true;
            DecideCirclingAction(enemyAnimatorManager);
        }
        
        HandleRotateTowardsTarget(enemyManager);
        
        //满足攻击条件
        if (enemyManager.currentRecoveryTime <= 0 && attackState.currentAttack != null)
        {
            randomDestiantionSet = false;
            return attackState;
        }
        else
        {
            GetNewAttack(enemyManager);
        }
        
        return this;
    }
    
    /// <summary>
    /// 旋转至 target的方向
    /// </summary>
    /// <param name="enemyManager"></param>
    void HandleRotateTowardsTarget(EnemyManager enemyManager)
    {
        if (enemyManager.isPerformingAction)
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
                enemyManager.rotationSpeed / Time.deltaTime);
        }
        else
        {
            Vector3 relativeDirection = transform.InverseTransformDirection(enemyManager.navMeshAgent.desiredVelocity);
            Vector3 targetVelocity =  enemyManager.enemyRigidbody.velocity;

            enemyManager.navMeshAgent.enabled = true;
            enemyManager.navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
            enemyManager.enemyRigidbody.velocity = targetVelocity;

            Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
            Quaternion qt = Quaternion.LookRotation(targetDirection);
            enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, qt,
                enemyManager.rotationSpeed / Time.deltaTime);
            
        }
    }

    #region Cricling

    void DecideCirclingAction(EnemyAnimatorManager enemyAnimatorManager)
    {
        WalkAroundTarget(enemyAnimatorManager);
    }
    void WalkAroundTarget(EnemyAnimatorManager enemyAnimatorManager)
    {
        /*verticalMovementValue = Random.Range(-1,1);

        if (verticalMovementValue <= 1 && verticalMovementValue > 0)
        {
            verticalMovementValue = 0.5f;
        }
        else if (verticalMovementValue >= -1 && verticalMovementValue < 0)
        {
            verticalMovementValue = -0.5f;
        }*/

        horizontalMovementValue = Random.Range(-1,1);

        if (horizontalMovementValue <= 1 && horizontalMovementValue >= 0)
        {
            horizontalMovementValue = 0.5f;
        }
        else if(horizontalMovementValue >= -1 && horizontalMovementValue < 0)
        {
            horizontalMovementValue = -0.5f;
        }
    }

    #endregion
    
    /// <summary>
    /// 获得一个新的攻击Action
    /// </summary>
    protected virtual void GetNewAttack(EnemyManager enemyManager)
    {
        //计算方向
        Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
        //计算角度
        float viewableAngle = Vector3.Angle(targetDirection, transform.forward);
        //计算距离
        float distanceFromTarget =
            Vector3.Distance(enemyManager.currentTarget.transform.position, transform.position);

        int maxScore = 0;
        for (int i = 0; i < enemyAttacks.Length; i++)
        {
            EnemyAttackAction enemyAttackAction = enemyAttacks[i];
            if (distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
                && distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
            {
                if (viewableAngle <= enemyAttackAction.maximumAttackAngle)
                {
                    maxScore += enemyAttackAction.attackScore;
                }
            }
        }

        int randomValue = Random.Range(0, maxScore);//[0,6]
        int temporaryScore = 0;
        for (int i = 0; i < enemyAttacks.Length; i++)
        {
            EnemyAttackAction enemyAttackAction = enemyAttacks[i];
            if (distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
                && distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
            {
                if (viewableAngle <= enemyAttackAction.maximumAttackAngle)
                {
                    if (attackState.currentAttack != null)
                        return;

                    temporaryScore += enemyAttackAction.attackScore;
                    if (temporaryScore > randomValue)
                        attackState.currentAttack = enemyAttackAction;
                }
            }
        }
    }
}
