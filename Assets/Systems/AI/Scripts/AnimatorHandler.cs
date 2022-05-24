using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorHandler : MonoBehaviour
{
    public Animator anim;
    public bool canRotate;

    public void PlayTargetAnimation(string targetAnim, bool isInteracting, bool canRotate = false)
    {
        anim.applyRootMotion = isInteracting;
        anim.SetBool("canRotate", canRotate);
        anim.SetBool("isInteracting", isInteracting);
        anim.CrossFade(targetAnim, 0.2f);
    }

    public void EnableCombo()
    {
        anim.SetBool("canDoCombo", true);
    }

    public void DisableCombo()
    {
        anim.SetBool("canDoCombo", false);
    }

    public void CanRotate()
    {
        anim.SetBool("canRotate", true);
    }

    public void StopRotation()
    {
        anim.SetBool("canRotate", false);
    }

    public void EnableIsInvulnerable()
    {
        anim.SetBool("isInvulnerable", true);
    }

    public void DisableIsInvulnerable()
    {
        anim.SetBool("isInvulnerable", false);
    }

    public virtual void TakeCriticalDamageAnimationEvent()
    {
        
    }
}
