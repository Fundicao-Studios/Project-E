using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorManager : AnimatorHandler
{
    EnemyManager enemyManager;
    EnemyStats enemyStats;

    public bool isPunch01;

    private void Awake()
    {
        enemyManager = GetComponentInParent<EnemyManager>();
        enemyStats = GetComponentInParent<EnemyStats>();
    }

    public override void TakeCriticalDamageAnimationEvent()
    {
        enemyStats.TakeDamageNoAnimation(enemyManager.pendingCriticalDamage);
        enemyManager.pendingCriticalDamage = 0;
    }

    public void EnableIsParrying()
    {
        enemyManager.isParrying = true;
    }

    public void DisableIsParrying()
    {
        enemyManager.isParrying = false;
    }

    public void EnableCanBeRiposted()
    {
        enemyManager.canBeRiposted = true;
    }

    public void DisableCanBeRiposted()
    {
        enemyManager.canBeRiposted = false;
    }

    public void EnablePunch01()
    {
        isPunch01 = true;
    }

    public void DisablePunch01()
    {
        isPunch01 = false;
    }

    private void OnAnimatorMove()
    {
        float delta = Time.deltaTime;
        enemyManager.enemyRigidBody.drag = 0;
        Vector3 deltaPosition = anim.deltaPosition;
        deltaPosition.y = 0;
        Vector3 velocity = deltaPosition / delta;
        enemyManager.enemyRigidBody.velocity = velocity;
    }
}
