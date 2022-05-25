using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrocSpellItem : Item
{
    public GameObject spellWarmUpFX;
    public GameObject spellCastFX;
    public string spellAnimation;

    [Header("Tipo De Magia")]
    public bool isShockSpell;
    public bool isFireSpell;
    public bool isWaterSpell;

    [Header("Descrição Da Magia")]
    [TextArea]
    public string spellDescription;

    public virtual void AttemptToCastSpell(
        CrocAnimatorManager animatorManage, 
        CrocStats playerStats, 
        CrocWeaponSlotManager weaponSlotManager)
    {
        Debug.Log("Tentou lançar uma magia!");
    }

    public virtual void SuccessfullyCastSpell(
        CrocAnimatorManager animatorManager, 
        CrocStats playerStats,
        CrocWeaponSlotManager weaponSlotManager)
    {
        Debug.Log("Conseguiu lançar uma magia com sucesso!");
    }
}
