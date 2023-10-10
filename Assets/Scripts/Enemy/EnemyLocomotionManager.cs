using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyLocomotionManager : MonoBehaviour
{
    //Components
    private EnemyAnimatorManager enemyAnimatorManager;
    private EnemyManager enemyManager;
    
    //防止相互 推搡碰撞器
    public CapsuleCollider characterCollider;
    public CapsuleCollider characterCollisionBlockerCollider;
    private void Awake()
    {
        enemyManager = GetComponent<EnemyManager>();
        enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
    }

    private void Start()
    {
        Physics.IgnoreCollision(characterCollider, characterCollisionBlockerCollider);//或略碰撞
    }
    
    public void DrainStaminaLightAttack()
    {
      
    }

    public void DrainStaminaHeavyAttack()
    {
        
    }
    
    public void EnableCombo()
    {
        //anim.SetBool("canDoCombo",true);
    }
    
    public void DisableCombo()
    {
        //anim.SetBool("canDoCombo",false);
    }
}
