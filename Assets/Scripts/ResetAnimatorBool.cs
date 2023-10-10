using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class ResetAnimatorBool : StateMachineBehaviour
{
    public string isUsingRightHand = "isUsingRightHand";
    public bool isUsingRightHandStatus = false;
    
    public string isUsingLeftHand = "isUsingLeftHand";
    public bool isUsingLeftHandStatus = false;
    
    public string isInteractingBoll = "isInteracting";
    public bool isInteractingStatus = false;
    
    public string isFiringSpell = "isFiringSpell";
    public bool isFiringSpellStatus = false;

    public string canRotateBool = "canRotate";
    public bool canRotateStatus = true;
    
    public string isRotatingWithRootMotion = "isRotatingWithRootMotion";
    public bool isRotatingWithRootMotionStatus = false;
    
    public string isMirroredBool = "isMirrored";
    public bool isMirroredStatus = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CharacterManager character = animator.GetComponentInParent<CharacterManager>();
        character.isUsingLeftHand = false;
        character.isUsingRightHand = false;
        
        animator.SetBool(isInteractingBoll, isInteractingStatus);
        animator.SetBool(isFiringSpell, isFiringSpellStatus);
        animator.SetBool(isRotatingWithRootMotion, isRotatingWithRootMotionStatus);
        animator.SetBool(canRotateBool, canRotateStatus);
        animator.SetBool(isMirroredBool, isMirroredStatus);
    }
}
