using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


public class EnemyManager : CharacterManager
{
    //Components
    public Rigidbody enemyRigidbody;
    public NavMeshAgent navMeshAgent;
    private EnemyStatsManager enemyStatsManager;
    private EnemyEffectManager enemyEffectManager;
    private EnemyLocomotionManager enemyLocomotionManager;
    private EnemyAnimatorManager enemyAnimatorManager;
    public EnemyInventoryManager enemyInventoryManager;
    
    public State currentState;//当前状态
    public CharacterStatsManager currentTarget;//当前目标 玩家
    
    public bool isPerformingAction;//是否正在 执行攻击等动画
    
    //NvaMesh Parameter
    public float stoppingDistance = 0.5f;
    public float rotationSpeed;
    public float maximumAggroRadius = 1.5f;//最大攻击半径
    
    //Enemy Action
    [SerializeField] private EnemyAttackAction[] enemyAttacks;
    [SerializeField] private EnemyAttackAction currentAttack;
    
    [Header("A.I Stats")] 
    public float detectionRadius = 20;
    public float maximumDetectionAngle = 50;
    public float minimumDetectionAngle = -50;

    //当前攻击重置计时
    public float currentRecoveryTime = 0;

    [Header("AI Combo Parameter")]
    public bool allowAIToPerformCombos;
    public float comboLikelyHood;

    protected override void Awake()
    {
        base.Awake();
        enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
        enemyStatsManager = GetComponent<EnemyStatsManager>();
        enemyRigidbody = GetComponent<Rigidbody>();
        enemyEffectManager = GetComponent<EnemyEffectManager>();
        enemyInventoryManager = GetComponent<EnemyInventoryManager>();
        
        navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
    }

    private void Start()
    {
        navMeshAgent.enabled = false;
        enemyRigidbody.isKinematic = false;
    }

    protected override void FixedUpdate()
    {
        enemyEffectManager.HandleAllBuildUpEffects();
    }

    private void Update()
    {
        HandleRecoveryTimer();
        HandleStateMachine();
        
        isRotatingWithRootMotion = enemyAnimatorManager.anim.GetBool("isRotatingWithRootMotion");
        isInteracting = enemyAnimatorManager.anim.GetBool("isInteracting");
        canDoCombo = enemyAnimatorManager.anim.GetBool("canDoCombo");
        canRotate = enemyAnimatorManager.anim.GetBool("canRotate");
        enemyAnimatorManager.anim.SetBool("isDead", enemyStatsManager.isDead);
    }

    void LateUpdate()
    {
        navMeshAgent.transform.localPosition = Vector3.zero;
        navMeshAgent.transform.localRotation = Quaternion.identity;
    }
    
    
    /// <summary>
    /// 更新 执行状态
    /// </summary>
    void HandleStateMachine()
    {
        if (currentState != null)
        {
            State nextState = currentState.Tick(this, enemyStatsManager, enemyAnimatorManager);

            if (nextState != null)
            {
                SwitchToNextState(nextState);
            }
        }
    }

    /// <summary>
    /// 更换状态
    /// </summary>
    /// <param name="nextState"></param>
    private void SwitchToNextState(State nextState)
    {
        currentState = nextState;
    }

    /// <summary>
    /// 更新攻击重置
    /// </summary>
    void HandleRecoveryTimer()
    {
        if (currentRecoveryTime > 0)
        {
            currentRecoveryTime -= Time.deltaTime;
        }

        if (isPerformingAction)
        {
            if (currentRecoveryTime <= 0)
            {
                isPerformingAction = false;
            }
        }
    }
}
