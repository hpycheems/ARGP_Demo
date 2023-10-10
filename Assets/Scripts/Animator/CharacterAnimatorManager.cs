using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Serialization;

/// <summary>
/// AnimatorManager 基类
/// </summary>
public class CharacterAnimatorManager : MonoBehaviour
{
    //Components
    public Animator anim;
    RigBuilder rigBuilder;
    private CharacterManager characterManager;
    
    [Header("Player Stats")]
    public bool canRotate;
    [Header("IK Control")]
    public TwoBoneIKConstraint leftHandConstraint;
    public TwoBoneIKConstraint rightHandConstraint;

    private bool handIKWeightsReset;

    protected virtual void Awake()
    {
        rigBuilder = GetComponent<RigBuilder>();
        
        characterManager = GetComponentInParent<CharacterManager>();
    }

    /// <summary>
    /// 播放目标动画 使用动画过度
    /// </summary>
    /// <param name="targetAnim">目标动画名称</param>
    /// <param name="isInteracting">是否是交换</param>
    public void PlayTargetAnimation(string targetAnim, bool isInteracting,float crossTime = 0.2f, bool canRotate = false, 
        bool isMirror = false)
    {
        anim.applyRootMotion = isInteracting;
        anim.SetBool("canRotate", canRotate);
        anim.SetBool("isInteracting", isInteracting);
        anim.SetBool("isMirrored", isMirror);
        anim.CrossFade(targetAnim, crossTime);
    }
    
    public void PlayTargetAnimationWithRootRotation(string targetAnim, bool isInteracting)
    {
        anim.applyRootMotion = isInteracting;
        anim.SetBool("isInteracting", isInteracting);
        anim.SetBool("isRotatingWithRootMotion", true);
        anim .CrossFade(targetAnim, 0.2f);
    }

    /// <summary>
    /// 刺击
    /// </summary>
    public virtual void TakeCriticalDamage(){}
    
    // IK
    public virtual void SetHandIKForWeapon(RightHandIKTarget rightHandIKTarget, 
        LeftHandIKTarget leftHandIKTarget, bool isTwoHandingWeapon)
    {
        if (isTwoHandingWeapon)
        {
            if (rightHandConstraint != null)
            {
                rightHandConstraint.data.target = rightHandIKTarget.transform;
                rightHandConstraint.data.targetPositionWeight = 1;
                rightHandConstraint.data.targetRotationWeight = 1;
            }

            if (leftHandConstraint != null)
            {
                leftHandConstraint.data.target = leftHandIKTarget.transform;
                leftHandConstraint.data.targetPositionWeight = 1;
                leftHandConstraint.data.targetRotationWeight = 1;
            }
        }
        else
        {
            rightHandConstraint.data.target = null;
            leftHandConstraint.data.target = null;
        }

        rigBuilder.Build();
    }

    public virtual void CheckHandIKWeight(RightHandIKTarget rightHandIK, LeftHandIKTarget leftHandIK,
        bool isTwoHandingWeapon)
    {
        if (characterManager.isInteracting)
            return;

        if (isTwoHandingWeapon)
        {
            handIKWeightsReset = false;

            if (rightHandConstraint.data.target != null)
            {
                rightHandConstraint.data.target = rightHandIK.transform;
                rightHandConstraint.data.targetPositionWeight = 1;
                rightHandConstraint.data.targetRotationWeight = 1;
            }

            if (leftHandConstraint.data.target != null)
            {
                leftHandConstraint.data.target = leftHandIK.transform;
                leftHandConstraint.data.targetPositionWeight = 1;
                leftHandConstraint.data.targetRotationWeight = 1;
            }
        }
    }

    public virtual void EraseHandIKForWeapon()
    {
        handIKWeightsReset = true;

        if (rightHandConstraint.data.target != null)
        {
            rightHandConstraint.data.targetPositionWeight = 0;
            rightHandConstraint.data.targetRotationWeight = 0;
        }

        if (leftHandConstraint.data.target != null)
        {
            leftHandConstraint.data.targetPositionWeight = 0;
            leftHandConstraint.data.targetRotationWeight = 0;
        }
    }
    
    #region Animation Event Signal

    public virtual void AwardSoulsOnDeath(){}
    
    public virtual void CanRotate()
    {
        anim.SetBool("canRotate", true); }

    public virtual void StopRotation()
    {
        anim.SetBool("canRotate", false);
    }
    
    public virtual void EnableCombo()
    {
        anim.SetBool("canDoCombo",true);
    }
    
    public virtual void DisableCombo()
    {
        anim.SetBool("canDoCombo",false);
    }

    public virtual void EnableIsInvulerable()
    {
        anim.SetBool("isInvulerable", true);
    }
    public virtual void DisableIsInvulerable()
    {
        anim.SetBool("isInvulerable", false);
    }
    
    public virtual void DrainStaminaLightAttack()
    {
    }

    public virtual void DrainStaminaHeavyAttack()
    {
    }

    #endregion
}
