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
            CharacterStatsManager characterStatsManager = collision.GetComponent<CharacterStatsManager>();
            CharacterManager enemyManager = collision.GetComponent<CharacterManager>();
            CharacterEffectsManager enemyEffectsManager = collision.GetComponentInChildren<CharacterEffectsManager>();
            BlockingCollider shield = collision.transform.GetComponentInChildren<BlockingCollider>();

            if (enemyManager != null)
            {
                if (characterStatsManager.teamIDNumber == teamIDNumber)
                    return;

                if (enemyManager.isParrying)
                {
                    characterManager.GetComponentInChildren<CharacterAnimatorManager>().PlayTargetAnimation("Parried", true);
                    return;
                }
                else if (shield != null && enemyManager.isBlocking)
                {
                    float physicalDamageAfterBlock = physicalDamage - (physicalDamage * shield.blockingPhysicalDamageAbsorption) / 100;
                    float fireDamageAfterBlock = fireDamage - (fireDamage * shield.blockingFireDamageAbsorption) / 100;
                    float shockDamageAfterBlock = shockDamage - (shockDamage * shield.blockingShockDamageAbsorption) / 100;

                    if (characterStatsManager != null)
                    {
                        if (characterStatsManager.currentStamina > 0)
                        {
                            characterStatsManager.TakeDamage(Mathf.RoundToInt(physicalDamageAfterBlock), Mathf.RoundToInt(fireDamageAfterBlock), Mathf.RoundToInt(shockDamageAfterBlock), "Block_Guard");
                        }
                        else
                        {
                            characterStatsManager.TakeDamage(physicalDamage, fireDamage, shockDamage, "Parried");
                        }
                        return;
                    }
                }
            }

            if (characterStatsManager != null)
            {
                if (characterStatsManager.teamIDNumber == teamIDNumber)
                    return;
                    
                characterStatsManager.poiseResetTimer = characterStatsManager.totalPoiseResetTime;
                characterStatsManager.totalPoiseDefense = characterStatsManager.totalPoiseDefense - poiseBreak;

                //DETETAR O PRIMEIRO CONTACTO DA ARMA COM O COLLIDER
                Vector3 contactPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                enemyEffectsManager.PlayBloodSplatterFX(contactPoint);

                if (characterStatsManager.totalPoiseDefense > poiseBreak)
                {
                    characterStatsManager.TakeDamageNoAnimation(physicalDamage, fireDamage, shockDamage);
                }
                else
                {
                    characterStatsManager.TakeDamage(physicalDamage, fireDamage, shockDamage);
                }
            }
        }
    }
}
