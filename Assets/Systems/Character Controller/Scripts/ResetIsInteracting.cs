using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetIsInteracting : StateMachineBehaviour
{
    public string isInteractingBool = "isInteracting";
    public bool isInteractingStatus = false;

    public string isFiringSpellBool = "isFiringSpell";
    public bool isFiringSpellStatus = false;

    public string canRotateBool = "canRotate";
    public bool canRotateStatus = true;

    public string isInvulnerableBool = "isInvulnerable";
    public bool isInvulnerableStatus = false;

    public string isPhaseShiftingBool = "isPhaseShifting";
    public bool isPhaseShiftingStatus = false;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(isInteractingBool, isInteractingStatus);
        animator.SetBool(isFiringSpellBool, isFiringSpellStatus);
        animator.SetBool(canRotateBool, canRotateStatus);
        animator.SetBool(isInvulnerableBool, isInvulnerableStatus);
        animator.SetBool(isPhaseShiftingBool, isPhaseShiftingStatus);
    }
}
