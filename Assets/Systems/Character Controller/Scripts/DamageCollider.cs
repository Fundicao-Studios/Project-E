using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    public CharacterManager characterManager;
    protected Collider damageCollider;
    public bool enabledDamageColliderOnStartUp = false;

    [Header("ID De Equipa")]
    public int teamIDNumber = 0;

    [Header("Poise")]
    public float poiseBreak;
    public float offensivePoiseBonus;

    [Header("Dano")]
    public int physicalDamage;
    public int fireDamage;
    public int shockDamage;
    public int waterDamage;
    public int darkDamage;

    protected virtual void Awake()
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
        if (collision.tag == "Character")
        {
            CharacterStatsManager enemyStatsManager = collision.GetComponent<CharacterStatsManager>();
            CharacterManager enemyManager = collision.GetComponent<CharacterManager>();
            CharacterEffectsManager enemyEffectsManager = collision.GetComponentInChildren<CharacterEffectsManager>();
            BlockingCollider shield = collision.transform.GetComponentInChildren<BlockingCollider>();

            if (enemyManager != null)
            {
                if (enemyStatsManager.teamIDNumber == teamIDNumber)
                    return;

                if (enemyManager.isParrying)
                {
                    characterManager.GetComponentInChildren<AnimatorHandler>().PlayTargetAnimation("Parried", true);
                    return;
                }
                else if (shield != null && enemyManager.isBlocking)
                {
                    float physicalDamageAfterBlock = physicalDamage - (physicalDamage * shield.blockingPhysicalDamageAbsorption) / 100;
                    float fireDamageAfterBlock = fireDamage - (fireDamage * shield.blockingFireDamageAbsorption) / 100;

                    if (enemyStatsManager != null)
                    {
                        enemyStatsManager.TakeDamage(Mathf.RoundToInt(physicalDamageAfterBlock), 0, "Block_Guard");
                        return;
                    }
                }
            }

            if (enemyStatsManager != null)
            {
                if (enemyStatsManager.teamIDNumber == teamIDNumber)
                    return;
                    
                enemyStatsManager.poiseResetTimer = enemyStatsManager.totalPoiseResetTime;
                enemyStatsManager.totalPoiseDefense = enemyStatsManager.totalPoiseDefense - poiseBreak;

                //DETETAR O PRIMEIRO CONTACTO DA ARMA COM O COLLIDER
                Vector3 contactPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                enemyEffectsManager.PlayBloodSplatterFX(contactPoint);

                if (enemyStatsManager.totalPoiseDefense > poiseBreak)
                {
                    enemyStatsManager.TakeDamageNoAnimation(physicalDamage, 0);
                }
                else
                {
                    enemyStatsManager.TakeDamage(physicalDamage, 0);
                }
            }
        }
    }
}
