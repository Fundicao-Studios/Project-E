using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrocSpellItem : Item
{
    public GameObject spellWarmUpFX;
    public GameObject spellCastFX;
    public string spellAnimation;

    [Header("Substituidor De Animações")]
    public AnimatorOverrideController spellController;
    public string offHandIdleAnimation = "Left_Arm_Idle_01";

    [Header("Tipos De Armas")]
    public WeaponType weaponType;

    [Header("Descrição Da Magia")]
    [TextArea]
    public string spellDescription;

    public virtual void AttemptToCastSpell(
        CrocAnimatorManager animatorManage, 
        CrocStatsManager playerStats, 
        CrocWeaponSlotManager weaponSlotManager)
    {
        Debug.Log("Tentou lançar uma magia!");
    }

    public virtual void SuccessfullyCastSpell(
        CrocAnimatorManager animatorManager, 
        CrocStatsManager playerStats,
        CrocWeaponSlotManager weaponSlotManager)
    {
        Debug.Log("Conseguiu lançar uma magia com sucesso!");
    }
}
