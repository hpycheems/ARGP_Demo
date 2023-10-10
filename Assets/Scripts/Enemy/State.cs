using UnityEngine;


/// <summary>
/// Enemy 状态基类
/// </summary>
public abstract class State : MonoBehaviour
{
    /// <summary>
    /// 执行状态
    /// </summary>
    /// <param name="enemyManager"></param>
    /// <param name="enemyStatsManager"></param>
    /// <param name="enemyAnimatorManager"></param>
    /// <returns></returns>
    public abstract State Tick(EnemyManager enemyManager, EnemyStatsManager enemyStatsManager,
        EnemyAnimatorManager enemyAnimatorManager);
}
