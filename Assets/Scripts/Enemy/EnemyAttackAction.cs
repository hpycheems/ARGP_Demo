using UnityEngine;


[CreateAssetMenu(menuName = "A.I/Enemy Actions/Attack Action")]
public class EnemyAttackAction : EnemyAction
{
    /// <summary>
    /// 连击 Parameter
    /// </summary>
    public bool canCombo;
    public EnemyAttackAction comboAction;
   
    /// <summary>
    /// 获得新攻击参数
    /// </summary>
    public int attackScore = 3;
    /// <summary>
    /// 攻击冷却
    /// </summary>
    public float recoveryTime = 2;

    
    [Header("Attack Detection Parameter")]
    public float maximumAttackAngle = 35;
    public float minimumAttackAngle = -35;
    public float minimumDistanceNeededToAttack = 0;
    public float maximumDistanceNeededToAttack = 3;

    
}
