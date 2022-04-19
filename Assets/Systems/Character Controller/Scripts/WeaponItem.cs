using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapon Item")]
public class WeaponItem : Item
{
    public GameObject modelPrefab;
    public bool isUnarmed;

    [Header("Animações De Idle")]
    public string right_hand_idle;
    public string left_hand_idle;

    [Header("Ataques De Uma Mão")]
    public string oh_light_attack_01;
    public string oh_light_attack_02;
    public string oh_heavy_attack_01;

    [Header("Custos De Stamina")]
    public int baseStamina;
    public float lightAttackMultiplier;
    public float heavyAttackMultiplier;

}
