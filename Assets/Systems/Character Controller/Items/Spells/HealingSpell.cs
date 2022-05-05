using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Healing Spell")]
public class HealingSpell : SpellItem
{
    public int healAmount;

    public override void AttemptToCastSpell(PlayerAnimatorManager animatorManager, PlayerStats playerStats)
    {
        base.AttemptToCastSpell(animatorManager, playerStats);
        GameObject instantiateWarmUpSpellFX = Instantiate(spellWarmUpFX, animatorManager.transform);
        animatorManager.PlayTargetAnimation(spellAnimation, true);
        Debug.Log("A tentar lançar a magia...");
    }

    public override void SuccessfullyCastSpell(PlayerAnimatorManager animatorManager, PlayerStats playerStats)
    {
        base.SuccessfullyCastSpell(animatorManager, playerStats);
        GameObject instantiatedSpellFX = Instantiate(spellCastFX, animatorManager.transform);
        playerStats.HealPlayer(healAmount);
        Debug.Log("Lançamento da magia feito com sucesso!");
    }
}
