using UnityEngine;


/// <summary>
/// Enemy 伏击状态
/// </summary>
public class AmbushState : State
{
    /// <summary>
    /// 追逐状态
    /// </summary>
    public PursueTargetState pursueTargetState;
    /// <summary>
    /// 检测层
    /// </summary>
    public LayerMask detectionLayer;
    
    public bool isSleeping;//是否正在伏击
    public float detectionRadius = 2f;//检测半径
    
    //Animations
    public string sleepAnimation;
    public string wakeAnimation;
    
    public override State Tick(EnemyManager enemyManager, EnemyStatsManager enemyStatsManager, EnemyAnimatorManager enemyAnimatorManager)
    {
        if (isSleeping && enemyManager.isInteracting == false)
        {
            //如果正在伏击 播放伏击动画
            enemyAnimatorManager.PlayTargetAnimation(sleepAnimation, true);
        }

        #region Handle Target Detection
        //检测半径为 detectionRadius 的圆范围内的玩家
        Collider[] colliders = Physics.OverlapSphere(enemyManager.transform.position, detectionRadius, detectionLayer);
        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterStatsManager characterStatsManager = colliders[i].GetComponent<CharacterStatsManager>();
            if (characterStatsManager != null)//如果玩家存在
            {
                //与玩家的 方向
                Vector3 targetDirection = characterStatsManager.transform.position - enemyManager.transform.position;
                //计算出 与玩家的角度
                float viewableAngle = Vector3.Angle(targetDirection, enemyManager.transform.forward);

                //如果玩家可被发现
                if (viewableAngle > enemyManager.minimumDetectionAngle
                    && viewableAngle < enemyManager.maximumDetectionAngle)
                {
                    enemyManager.currentTarget = characterStatsManager;
                    isSleeping = false;
                    enemyAnimatorManager.PlayTargetAnimation(wakeAnimation, true);
                }
            }
        }
        #endregion

        #region Handle State Change

        if (enemyManager.currentTarget != null)
        {
            //更换状态
            return pursueTargetState;
        }
        else
        {
            return this;
        }

        #endregion
    }
}
