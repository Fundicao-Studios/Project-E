using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapon Item")]
public class WeaponItem : Item
{
    public GameObject modelPrefab;
    public bool isUnarmed;

    [Header("Dano")]
    public int baseDamage = 25;
    public int criticalDamageMultiplier = 4;

    [Header("Absorção")]
    public float physicalDamageAbsorption;

    [Header("Animações De Idle")]
    public string right_hand_idle;
    public string left_hand_idle;
    public string th_idle;

    [Header("Animações De Ataque")]
    public string oh_light_attack_01;
    public string oh_light_attack_02;
    public string oh_heavy_attack_01;
    public string th_light_attack_01;
    public string th_light_attack_02;
    public string weapon_art;

    [Header("Custos De Stamina")]
    public int baseStamina;
    public float lightAttackMultiplier;
    public float heavyAttackMultiplier;

    [Header("Tipo De Arma")]
    public bool isShockCaster;
    public bool isFireCaster;
    public bool isWaterCaster;
    public bool isMeleeWeapon;
    public bool isShieldWeapon;
}
