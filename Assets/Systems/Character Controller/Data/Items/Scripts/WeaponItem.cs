using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapon Item")]
public class WeaponItem : Item
{
    public GameObject modelPrefab;
    public bool isUnarmed;

    [Header("Substituidor De Animações")]
    public AnimatorOverrideController weaponController;
    public string offHandIdleAnimation = "Left_Arm_Idle_01";

    [Header("Tipos De Armas")]
    public WeaponType weaponType;

    [Header("Dano")]
    public int physicalDamage;
    public int fireDamage;
    public int shockDamage;
    public int criticalDamageMultiplier = 4;

    [Header("Poise")]
    public float poiseBreak;
    public float offensivePoiseBonus;

    [Header("Absorção")]
    public float physicalDamageAbsorption;
    public float fireDamageAbsorption;
    public float shockDamageAbsorption;

    [Header("Custos De Stamina")]
    public int baseStamina;
    public float lightAttackMultiplier;
    public float heavyAttackMultiplier;
    public float shieldParryMultiplier;
}
