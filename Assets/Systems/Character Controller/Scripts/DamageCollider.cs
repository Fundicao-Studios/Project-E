using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    public CharacterManager characterManager;
    Collider damageCollider;
    public bool enabledDamageColliderOnStartUp = false;

    [Header("Poise")]
    public float poiseBreak;
    public float offensivePoiseBonus;

    [Header("Dano")]
    public int currentWeaponDamage = 25;

    private void Awake()
    {
        damageCollider = GetComponent<Collider>();
        damageCollider.gameObject.SetActive(true);
        damageCollider.isTrigger = true;
        damageCollider.enabled = enabledDamageColliderOnStartUp;
    }

    public void EnableDamageCollider()
    {
        damageCollider.enabled = true;
    }

    public void DisableDamageCollider()
    {
        damageCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {
            PlayerStatsManager playerStats = collision.GetComponent<PlayerStatsManager>();
            CharacterManager enemyCharacterManager = collision.GetComponent<CharacterManager>();
            BlockingCollider shield = collision.transform.GetComponentInChildren<BlockingCollider>();

            if (enemyCharacterManager != null)
            {
                if (enemyCharacterManager.isParrying)
                {
                    characterManager.GetComponentInChildren<AnimatorHandler>().PlayTargetAnimation("Parried", true);
                    return;
                }
                else if (shield != null && enemyCharacterManager.isBlocking)
                {
                    float physicalDamageAfterBlock =
                        currentWeaponDamage - (currentWeaponDamage * shield.blockingPhysicalDamageAbsorption) / 100;
                    if (playerStats != null)
                    {
                        playerStats.TakeDamage(Mathf.RoundToInt(physicalDamageAfterBlock), "Block_Guard");
                        return;
                    }
                }
            }

            if (playerStats != null)
            {
                playerStats.poiseResetTimer = playerStats.totalPoiseResetTime;
                playerStats.totalPoiseDefense = playerStats.totalPoiseDefense - poiseBreak;
                Debug.Log("Poise do jogador está atualmente em " + playerStats.totalPoiseDefense);

                if (playerStats.totalPoiseDefense > poiseBreak)
                {
                    playerStats.TakeDamageNoAnimation(currentWeaponDamage);
                }
                else
                {
                    playerStats.TakeDamage(currentWeaponDamage);
                }
            }
        }

        if (collision.tag == "Enemy")
        {
            EnemyStatsManager enemyStats = collision.GetComponent<EnemyStatsManager>();
            GolemStatsManager golemStats = collision.GetComponent<GolemStatsManager>();
            CrocStatsManager crocStats = collision.GetComponent<CrocStatsManager>();
            BossStatsManager bossStats = collision.GetComponent<BossStatsManager>();
            CharacterManager enemyCharacterManager = collision.GetComponent<CharacterManager>();

            if (enemyCharacterManager != null)
            {
                if (enemyCharacterManager.isParrying)
                {
                    characterManager.GetComponentInChildren<AnimatorHandler>().PlayTargetAnimation("Parried", true);
                    return;
                }
            }

            if (enemyStats != null)
            {
                enemyStats.poiseResetTimer = enemyStats.totalPoiseResetTime;
                enemyStats.totalPoiseDefense = enemyStats.totalPoiseDefense - poiseBreak;
                Debug.Log("Poise do inimigo está atualmente em " + enemyStats.totalPoiseDefense);

                if (enemyStats.totalPoiseDefense > poiseBreak)
                {
                    enemyStats.TakeDamageNoAnimation(currentWeaponDamage);
                }
                else
                {
                    enemyStats.TakeDamage(currentWeaponDamage);
                }
            }
            else if (golemStats != null)
            {
                golemStats.poiseResetTimer = golemStats.totalPoiseResetTime;
                golemStats.totalPoiseDefense = golemStats.totalPoiseDefense - poiseBreak;
                Debug.Log("Poise do golem está atualmente em " + golemStats.totalPoiseDefense);

                if (golemStats.totalPoiseDefense > poiseBreak)
                {
                    golemStats.TakeDamageNoAnimation(currentWeaponDamage);
                }
                else
                {
                    golemStats.TakeDamage(currentWeaponDamage);
                }
            }
            else if (crocStats != null)
            {
                crocStats.poiseResetTimer = crocStats.totalPoiseResetTime;
                crocStats.totalPoiseDefense = crocStats.totalPoiseDefense - poiseBreak;
                Debug.Log("Poise do crocodilo está atualmente em " + crocStats.totalPoiseDefense);

                if (crocStats.totalPoiseDefense > poiseBreak)
                {
                    crocStats.TakeDamageNoAnimation(currentWeaponDamage);
                }
                else
                {
                    crocStats.TakeDamage(currentWeaponDamage);
                }
            }
            else if (bossStats != null)
            {
                bossStats.poiseResetTimer = bossStats.totalPoiseResetTime;
                bossStats.totalPoiseDefense = bossStats.totalPoiseDefense - poiseBreak;
                Debug.Log("Poise do boss está atualmente em " + bossStats.totalPoiseDefense);

                if (bossStats.totalPoiseDefense > poiseBreak)
                {
                    bossStats.TakeDamageNoAnimation(currentWeaponDamage);
                }
                else
                {
                    bossStats.TakeDamageNoAnimation(currentWeaponDamage);
                    bossStats.BreakGuard();
                }
            }
        }
    }
}
