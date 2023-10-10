using UnityEngine;


public class PursueTargetState : State
{
    //战斗状态
    public float moveSpeed = 10;
    public State combatStanceState;
    public State rotateTowardsTargetState;
    
    public override State Tick(EnemyManager enemyManager, EnemyStatsManager enemyStatsManager, EnemyAnimatorManager enemyAnimatorManager)
    {
        if (enemyManager.isInteracting) 
            return this;
        
        //计算方向
        Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
        //计算距离
        float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
        //计算角度
        float viewableAngle = Vector3.Angle(targetDirection, enemyManager.transform.forward);

        if (viewableAngle > 50)
        {
            return rotateTowardsTargetState;
        }
        
        HandleRotateTowardsTarget(enemyManager);//追逐的时候 转向

        if (distanceFromTarget > enemyManager.maximumAggroRadius)
        {
            enemyAnimatorManager.anim.SetFloat("Horizontal", 0 ,0.1f, Time.deltaTime);
            enemyAnimatorManager.anim.SetFloat("Vertical", 1 ,0.1f, Time.deltaTime);
        }

        if (distanceFromTarget <= enemyManager.maximumAggroRadius)
        {
            return combatStanceState;
        }
        else
        {
            return this;
        }
    }
    
    /// <summary>
    /// 转向目标
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
            Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
            
            enemyManager.navMeshAgent.enabled = true;
            Vector3 target = enemyManager.currentTarget.transform.position;
            target.y = 0;
            enemyManager.navMeshAgent.SetDestination(target);
            
            Vector3 velocity = targetDirection.normalized;
            velocity.y = 0;
            
            Quaternion qt = Quaternion.LookRotation(targetDirection);
            enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, qt,
                enemyManager.rotationSpeed / Time.deltaTime);
            
            //enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, enemyManager.navMeshAgent.transform.rotation,
            //    enemyManager.rotationSpeed / Time.deltaTime);
        }
    }
}
