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

    [Header("Abrorção De Armadura")]
    public float physicalDamageAbsorptionHead;
    public float physicalDamageAbsorptionBody;
    public float physicalDamageAbsorptionLegs;
    public float physicalDamageAbsorptionFeet;

    //Absorção De Fogo
    //Absorção De Choque
    //Absorção De Água
    //Absorção De Escuridão

    public bool isDead;

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
}
