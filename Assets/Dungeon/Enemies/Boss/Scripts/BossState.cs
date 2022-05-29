using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossState : MonoBehaviour
{
    public abstract BossState Tick(BossManager golemManager, BossStatsManager enemyStats, BossAnimatorManager enemyAnimatorManager);
}
