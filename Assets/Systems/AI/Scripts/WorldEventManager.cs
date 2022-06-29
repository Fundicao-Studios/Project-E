using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldEventManager : MonoBehaviour
{
    public UIBossHealthBar bossHealthBar;
    public BossManager boss;
    public Animator bossDoor;
    public Animator bossDoorExitAnimation;

    public bool bossFightIsActive;   //Está atualmente a lutar contra o boss
    public bool bossHasBeenAwakened; //Acordou o boss/viu a cutscene mas morreu durante a luta
    public bool bossHasBeenDefeated; //O boss foi derrotado

    public void ActivateBossFight()
    {
        bossFightIsActive = true;
        bossHasBeenAwakened = true;
        bossHealthBar.SetUIHealthBarToActive();
        bossDoor.SetBool("isInBossFight", true);
    }

    public void BossHasBeenDefeated()
    {
        bossHasBeenDefeated = true;
        bossFightIsActive = false;
        bossHealthBar.SetHealthBarToInactive();
        bossDoorExitAnimation.SetBool("isBossDefeated", true);
    }
}
