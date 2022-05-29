using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWeaponSlotsManager : MonoBehaviour
{
    [Header("Arma Desequipada")]
    public WeaponItem unarmedWeapon;

    [Header("Slots De Armas")]
    public WeaponHolderSlot leftHandSlot;
    public WeaponHolderSlot rightHandSlot;
    public WeaponHolderSlot backSlot;

    [Header("Colliders De Dano")]
    public DamageCollider leftHandDamageCollider;
    public DamageCollider rightHandDamageCollider;
}
