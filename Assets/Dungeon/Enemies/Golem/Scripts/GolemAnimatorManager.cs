using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemAnimatorManager : AnimatorHandler
{
    GolemManager golemManager;

    public bool isPunch01;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        golemManager = GetComponentInParent<GolemManager>();
    }

    public void EnablePunch01()
    {
        isPunch01 = true;
    }

    public void DisablePunch01()
    {
        isPunch01 = false;
    }
}
