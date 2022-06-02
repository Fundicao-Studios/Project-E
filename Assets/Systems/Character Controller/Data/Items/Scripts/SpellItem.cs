using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellItem : Item
{
    public GameObject spellWarmUpFX;
    public GameObject spellCastFX;
    public string spellAnimation;

    [Header("Custo Da Magia")]
    public int manaPointCost;

    [Header("Substituidor De Animações")]
    public AnimatorOverrideController spellController;
    public string offHandIdleAnimation = "Left_Arm_Idle_01";

    [Header("Tipos De Armas")]
    public WeaponType weaponType;

    [Header("Descrição Da Magia")]
    [TextArea]
    public string spellDescription;

    public virtual void AttemptToCastSpell(
        PlayerAnimatorManager animatorManage, 
        PlayerStatsManager playerStats, 
        PlayerWeaponSlotManager weaponSlotManager)
    {
        Debug.Log("Tentaste lançar uma magia!");
    }

    public virtual void SuccessfullyCastSpell(
        PlayerAnimatorManager animatorManager, 
        PlayerStatsManager playerStats,
        CameraManager cameraManager,
        PlayerWeaponSlotManager weaponSlotManager)
    {
        Debug.Log("Conseguiste lançar uma magia com sucesso!");
        playerStats.DeductManaPoints(manaPointCost);
    }
}
