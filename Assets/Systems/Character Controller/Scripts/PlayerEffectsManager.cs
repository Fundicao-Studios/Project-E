using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectsManager : CharacterEffectsManager
{
    PlayerStatsManager playerStatsManager;
    PlayerWeaponSlotManager weaponSlotManager;

    BurnBuildUpBar burnBuildUpBar;
    BurnAmountBar burnAmountBar;
    
    public GameObject currentParticleFX; //As pertículas que serão usadas no efeito atual que está a afetar o jogador (beber poção de vida, veneno etc)
    public GameObject instantiadedFXModel;
    public int amountToBeHealed;

    protected override void Awake()
    {
        base.Awake();
        playerStatsManager = GetComponentInParent<PlayerStatsManager>();
        weaponSlotManager = GetComponent<PlayerWeaponSlotManager>();

        burnBuildUpBar = FindObjectOfType<BurnBuildUpBar>();
        burnAmountBar = FindObjectOfType<BurnAmountBar>();
    }

    public void HealPlayerFromEffect()
    {
        playerStatsManager.HealPlayer(amountToBeHealed);
        GameObject healParticles = Instantiate(currentParticleFX, playerStatsManager.transform);
        Destroy(instantiadedFXModel.gameObject, 1f);
        weaponSlotManager.LoadBothWeaponsOnSlots();
    }

    protected override void HandleBurnBuildup()
    {
        if (burnBuildup <= 0)
        {
            burnBuildUpBar.gameObject.SetActive(false);
        }
        else
        {
            burnBuildUpBar.gameObject.SetActive(true);
        }

        base.HandleBurnBuildup();
        burnBuildUpBar.SetBurnBuildUpAmount(Mathf.RoundToInt(burnBuildup));
    }

    protected override void HandleIsBurningEffect()
    {
        if (isBurning == false)
        {
            burnAmountBar.gameObject.SetActive(false);
        }
        else
        {
            burnAmountBar.gameObject.SetActive(true);
        }

        base.HandleIsBurningEffect();
        burnAmountBar.SetBurnAmount(Mathf.RoundToInt(burnAmount));
    }
}
