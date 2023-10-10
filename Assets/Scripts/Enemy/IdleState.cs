using UnityEngine;


public class IdleState : State
{
    // 追逐状态
    public State pursueTargetState;
    //检测层
    public LayerMask detectionLayer;
    
    public override State Tick(EnemyManager enemyManager, EnemyStatsManager enemyStatsManager, EnemyAnimatorManager enemyAnimatorManager)
    {
        //检测附近是否存在target
        Collider[] colliders = Physics.OverlapSphere(enemyManager.transform.position, enemyManager.detectionRadius, detectionLayer);
        
        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterStatsManager characterStatsManager = colliders[i].GetComponent<CharacterStatsManager>();
            //如果存在target
            if (characterStatsManager != null)
            {
                if (characterStatsManager.teamIDNumber != enemyStatsManager.teamIDNumber)
                {
                    //计算方向
                    Vector3 targetDirection = characterStatsManager.transform.position - enemyManager.transform.position;
                    //计算角度
                    float viewableAngle = Vector3.Angle(targetDirection, enemyManager.transform.forward);//取值为[0,180]

                    //可被发现
                    if (viewableAngle < enemyManager.maximumDetectionAngle)
                    {
                        enemyManager.currentTarget = characterStatsManager;//发现目标
                    }
                }
            }
        }

        if (enemyManager.currentTarget != null)
        {
            return pursueTargetState;
        }
        else
        {
            return this;
        }
    }
}
