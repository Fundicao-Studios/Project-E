using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CrocState : MonoBehaviour
{
    public abstract CrocState Tick(CrocManager golemManager, CrocStats enemyStats, CrocAnimatorManager enemyAnimatorManager);
}
