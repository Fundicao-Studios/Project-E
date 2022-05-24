using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GolemState : MonoBehaviour
{
    public abstract GolemState Tick(GolemManager golemManager, GolemStats enemyStats, GolemAnimatorManager enemyAnimatorManager);
}
