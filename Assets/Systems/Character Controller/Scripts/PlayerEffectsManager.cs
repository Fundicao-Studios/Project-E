using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectsManager : MonoBehaviour
{
    PlayerStats playerStats;
    WeaponSlotManager weaponSlotManager;
    public GameObject currentParticleFX; //As pertículas que serão usadas no efeito atual que está a afetar o jogador (beber poção de vida, veneno etc)
    public GameObject instantiadedFXModel;
    public int amountToBeHealed;

    private void Awake()
    {
        playerStats = GetComponentInParent<PlayerStats>();
        weaponSlotManager = GetComponent<WeaponSlotManager>();
    }

    public void HealPlayerFromEffect()
    {
        playerStats.HealPlayer(amountToBeHealed);
        GameObject healParticles = Instantiate(currentParticleFX, playerStats.transform);
        Destroy(instantiadedFXModel.gameObject, 1f);
        weaponSlotManager.LoadBothWeaponsOnSlots();
    }
}
