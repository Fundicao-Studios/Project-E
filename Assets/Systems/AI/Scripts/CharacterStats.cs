using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public int healthLevel = 10;
    public int maxHealth;
    public int currentHealth;

    public int staminaLevel = 10;
    public float maxStamina;
    public float currenStamina;

    public int manaLevel = 10;
    public float maxManaPoints;
    public float currentManaPoints;

    [Header("Poise")]
    public float totalPoiseDefense; //O TOTAL de poise durante o cálculo
    public float offensivePoiseBonus; //O poise que se GANHA durante um ataque com uma arma
    public float armorPoiseBonus; //O poise que se GANHA por estar a usar o que quer que seja que se tenha equipado
    public float totalPoiseResetTime = 15;
    public float poiseResetTimer = 0;

    [Header("Absorção De Armadura")]
    public float physicalDamageAbsorptionHead;
    public float physicalDamageAbsorptionBody;
    public float physicalDamageAbsorptionLegs;
    public float physicalDamageAbsorptionFeet;

    //Absorção De Fogo
    //Absorção De Choque
    //Absorção De Água
    //Absorção De Escuridão

    public bool isDead;

    protected virtual void Update()
    {
        HandlePoiseResetTimer();
    }

    private void Start()
    {
        totalPoiseDefense = armorPoiseBonus;
    }

    public virtual void TakeDamage(int physicalDamage, string damageAnimation = "Damage_01")
    {
        if (isDead)
            return;

        float totalPhysicalDamageAbsorption = 1 - 
            (1 - physicalDamageAbsorptionHead / 100) * 
            (1 - physicalDamageAbsorptionBody / 100) * 
            (1 - physicalDamageAbsorptionLegs / 100) *
            (1 - physicalDamageAbsorptionFeet / 100);

        physicalDamage = Mathf.RoundToInt(physicalDamage - (physicalDamage * totalPhysicalDamageAbsorption));

        Debug.Log("Total De Absorção De Dano é " + totalPhysicalDamageAbsorption + "%");

        float finalDamage = physicalDamage; //+ fireDamage + darkDamage + lightningDamage + waterDamage

        currentHealth = Mathf.RoundToInt(currentHealth - finalDamage);

        Debug.Log("Dano Total Foi " + finalDamage);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
        }
    }

    public virtual void HandlePoiseResetTimer()
    {
        if (poiseResetTimer > 0)
        {
            poiseResetTimer = poiseResetTimer - Time.deltaTime;
        }
        else
        {
            totalPoiseDefense = armorPoiseBonus;
        }
    }
}
