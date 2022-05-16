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

    [Header("Tipo De Magia")]
    public bool isShockSpell;
    public bool isFireSpell;
    public bool isWaterSpell;

    [Header("Descrição Da Magia")]
    [TextArea]
    public string spellDescription;

    public virtual void AttemptToCastSpell(
        PlayerAnimatorManager animatorManage, 
        PlayerStats playerStats, 
        WeaponSlotManager weaponSlotManager)
    {
        Debug.Log("Tentaste lançar uma magia!");
    }

    public virtual void SuccessfullyCastSpell(
        PlayerAnimatorManager animatorManager, 
        PlayerStats playerStats,
        CameraManager cameraManager,
        WeaponSlotManager weaponSlotManager)
    {
        Debug.Log("Conseguiste lançar uma magia com sucesso!");
        playerStats.DeductManaPoints(manaPointCost);
    }
}
