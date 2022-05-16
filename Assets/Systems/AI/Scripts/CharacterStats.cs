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

    public bool isDead;

    public virtual void TakeDamage(int damage, string damageAnimation = "Damage_01")
    {

    }
}
