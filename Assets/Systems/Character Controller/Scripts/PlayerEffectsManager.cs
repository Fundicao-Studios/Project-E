using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectsManager : MonoBehaviour
{
    PlayerStatsManager playerStatsManager;
    PlayerWeaponSlotManager weaponSlotManager;
    
    public GameObject currentParticleFX; //As pertículas que serão usadas no efeito atual que está a afetar o jogador (beber poção de vida, veneno etc)
    public GameObject instantiadedFXModel;
    public int amountToBeHealed;

    private void Awake()
    {
        playerStatsManager = GetComponentInParent<PlayerStatsManager>();
        weaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
    }

    public void HealPlayerFromEffect()
    {
        playerStatsManager.HealPlayer(amountToBeHealed);
        GameObject healParticles = Instantiate(currentParticleFX, playerStatsManager.transform);
        Destroy(instantiadedFXModel.gameObject, 1f);
        weaponSlotManager.LoadBothWeaponsOnSlots();
    }
}
